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

    [Header("Boss Tiles")]
    public GameObject bossEntranceTilePrefab;
    public GameObject bossMiddleTilePrefab;
    public GameObject bossExitTilePrefab;

    [Header("Floor Settings")]
    public int floorCount = 5;
    public float floorLength = 30f;
    public float spawnX = -5f;
    public float spawnY = 0f;
    public float spawnStartZ = 0f;

    [Header("Boss Phase")]
    public TilePhase currentPhase = TilePhase.Normal;
    public bool bossFightActive = false;

    private List<GameObject> floorTiles = new List<GameObject>();

    void Start()
    {
        floorTiles.Clear();

        for (int i = 0; i < floorCount; i++)
        {
            float z = spawnStartZ - (i * floorLength);
            SpawnTileAt(z);
        }
    }

    void Update()
    {
        floorTiles.RemoveAll(tile => tile == null);

        if (floorTiles.Count < floorCount)
        {
            float furthestBackZ = GetFurthestBackMovingTileZ();
            float newZ = furthestBackZ - floorLength;
            SpawnTileAt(newZ);
        }
    }

    void SpawnTileAt(float z)
    {
        GameObject prefabToSpawn = GetNextTilePrefab();

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No floor tile prefab assigned for phase: " + currentPhase);
            return;
        }

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, z);
        GameObject tile = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        floorTiles.Add(tile);

        Debug.Log("Spawned: " + tile.name + " at " + tile.transform.position);
    }

    GameObject GetNextTilePrefab()
    {
        if (currentPhase == TilePhase.Normal)
        {
            return normalFloorTilePrefab;
        }

        if (currentPhase == TilePhase.BossEntrance)
        {
            currentPhase = TilePhase.BossMiddle;
            bossFightActive = true;
            return bossEntranceTilePrefab;
        }

        if (currentPhase == TilePhase.BossMiddle)
        {
            return bossMiddleTilePrefab;
        }

        if (currentPhase == TilePhase.BossExit)
        {
            currentPhase = TilePhase.Normal;
            bossFightActive = false;
            return bossExitTilePrefab;
        }

        return normalFloorTilePrefab;
    }

    float GetFurthestBackMovingTileZ()
    {
        float furthestBackZ = float.PositiveInfinity;

        foreach (GameObject tile in floorTiles)
        {
            if (tile == null)
            {
                continue;
            }

            Transform movingTile = tile.transform.Find("MovingTile");

            if (movingTile != null && movingTile.position.z < furthestBackZ)
            {
                furthestBackZ = movingTile.position.z;
            }
        }

        if (furthestBackZ == float.PositiveInfinity)
        {
            return spawnStartZ;
        }

        return furthestBackZ;
    }

    public void StartBossTunnelPhase()
    {
        currentPhase = TilePhase.BossEntrance;
    }

    public void EndBossTunnelPhase()
    {
        currentPhase = TilePhase.BossExit;
    }
}