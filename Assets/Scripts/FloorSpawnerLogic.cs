using System.Collections.Generic;
using UnityEngine;

public class FloorSpawnerLogic : MonoBehaviour
{
    public enum TilePhase
    {
        Normal,
        BossEntrance,
        BossMiddle,
        BossExit
    }

    [Header("Normal Tile")]
    public GameObject normalFloorTilePrefab;
    public float normalTileLength = 30f;

    [Header("Boss Entrance Tile")]
    public GameObject bossEntranceTilePrefab;
    public float bossEntranceTileLength = 20f;

    [Header("Boss Middle Tile")]
    public GameObject bossMiddleTilePrefab;
    public float bossMiddleTileLength = 30f;

    [Header("Boss Exit Tile")]
    public GameObject bossExitTilePrefab;
    public float bossExitTileLength = 20f;

    [Header("Spawner Settings")]
    public int floorCount = 5;
    public float spawnX = -5f;
    public float spawnY = 0f;
    public float spawnStartZ = 0f;

    [Header("Boss Phase")]
    public TilePhase currentPhase = TilePhase.Normal;
    public int bossMiddleTilesToSpawn = 3;

    private int bossMiddleTilesRemaining = 0;
    private bool bossSequenceStarted = false;
    private bool forcingBossExit = false;

    private List<GameObject> floorTiles = new List<GameObject>();

    void Start()
    {
        floorTiles.Clear();

        float currentZ = spawnStartZ;

        for (int i = 0; i < floorCount; i++)
        {
            GameObject tile = SpawnTileAt(currentZ);

            if (tile != null)
            {
                currentZ -= GetLengthFromPrefab(tile.name);
            }
        }
    }

    void Update()
    {
        floorTiles.RemoveAll(tile => tile == null);

        if (floorTiles.Count < floorCount)
        {
            float furthestBackZ = GetFurthestBackMovingTileZ();
            float nextTileLength = GetNextTileLength();
            float newZ = furthestBackZ - nextTileLength;
            SpawnTileAt(newZ);
        }
    }

    GameObject SpawnTileAt(float z)
    {
        GameObject prefabToSpawn = GetNextTilePrefab();

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No tile prefab assigned for phase: " + currentPhase);
            return null;
        }

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, z);
        GameObject tile = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        floorTiles.Add(tile);

        Debug.Log("Spawned: " + tile.name + " at " + tile.transform.position);
        return tile;
    }

    GameObject GetNextTilePrefab()
    {
        if (forcingBossExit)
        {
            forcingBossExit = false;
            currentPhase = TilePhase.Normal;
            return bossExitTilePrefab;
        }

        if (currentPhase == TilePhase.Normal)
        {
            return normalFloorTilePrefab;
        }

        if (currentPhase == TilePhase.BossEntrance)
        {
            currentPhase = TilePhase.BossMiddle;
            bossMiddleTilesRemaining = bossMiddleTilesToSpawn;
            return bossEntranceTilePrefab;
        }

        if (currentPhase == TilePhase.BossMiddle)
        {
            if (bossMiddleTilesRemaining > 0)
            {
                bossMiddleTilesRemaining--;
                return bossMiddleTilePrefab;
            }

            currentPhase = TilePhase.BossExit;
            return bossExitTilePrefab;
        }

        if (currentPhase == TilePhase.BossExit)
        {
            currentPhase = TilePhase.Normal;
            return normalFloorTilePrefab;
        }

        return normalFloorTilePrefab;
    }

    float GetNextTileLength()
    {
        if (forcingBossExit)
        {
            return bossExitTileLength;
        }

        if (currentPhase == TilePhase.Normal)
        {
            return normalTileLength;
        }

        if (currentPhase == TilePhase.BossEntrance)
        {
            return bossEntranceTileLength;
        }

        if (currentPhase == TilePhase.BossMiddle)
        {
            return bossMiddleTileLength;
        }

        if (currentPhase == TilePhase.BossExit)
        {
            return bossExitTileLength;
        }

        return normalTileLength;
    }

    float GetLengthFromPrefab(string tileName)
    {
        string lowerName = tileName.ToLower();

        if (lowerName.Contains("entrance"))
        {
            return bossEntranceTileLength;
        }

        if (lowerName.Contains("exit"))
        {
            return bossExitTileLength;
        }

        if (lowerName.Contains("boss"))
        {
            return bossMiddleTileLength;
        }

        return normalTileLength;
    }

    float GetFurthestBackMovingTileZ()
    {
        float furthestBackZ = spawnStartZ;
        bool foundTile = false;

        foreach (GameObject tile in floorTiles)
        {
            if (tile == null)
            {
                continue;
            }

            Transform movingTile = tile.transform.Find("MovingTile");

            if (movingTile != null)
            {
                if (!foundTile || movingTile.position.z < furthestBackZ)
                {
                    furthestBackZ = movingTile.position.z;
                    foundTile = true;
                }
            }
        }

        return furthestBackZ;
    }

    public void StartBossTunnelPhase()
    {
        if (bossSequenceStarted)
        {
            return;
        }

        currentPhase = TilePhase.BossEntrance;
        bossMiddleTilesRemaining = bossMiddleTilesToSpawn;
        bossSequenceStarted = true;

        Debug.Log("FloorSpawner: Boss tunnel started.");
    }

    public void ForceBossExitPhase()
    {
        forcingBossExit = true;
        currentPhase = TilePhase.Normal;
        bossMiddleTilesRemaining = 0;

        Debug.Log("FloorSpawner: Forcing one boss exit tile.");
    }
}