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
    public float normalTileRotationY = 0f;
    public float normalTileXOffset = 0f;

    [Header("Boss Entrance Tile")]
    public GameObject bossEntranceTilePrefab;
    public float bossEntranceTileLength = 35f;
    public float bossEntranceTileRotationY = 0f;
    public float bossEntranceTileXOffset = 0f;

    [Header("Boss Middle Tile")]
    public GameObject bossMiddleTilePrefab;
    public float bossMiddleTileLength = 40f;
    public float bossMiddleTileRotationY = 0f;
    public float bossMiddleTileXOffset = 0f;

    [Header("Boss Exit Tile")]
    public GameObject bossExitTilePrefab;
    public float bossExitTileLength = 23.15f;
    public float bossExitTileRotationY = 180f;
    public float bossExitTileXOffset = -1f;

    [Header("Spawner Settings")]
    public int floorCount = 5;
    public float spawnX = -6.7f;
    public float spawnY = 0f;
    public float spawnStartZ = 10f;

    [Header("Boss Phase")]
    public TilePhase currentPhase = TilePhase.Normal;
    public int bossMiddleTilesToSpawn = 4;

    private bool bossSequenceStarted = false;

    private List<GameObject> floorTiles = new List<GameObject>();
    private Queue<TileSpawnData> bossTileQueue = new Queue<TileSpawnData>();

    private struct TileSpawnData
    {
        public GameObject prefab;
        public float length;
        public float rotationY;
        public float xOffset;
        public TilePhase phase;

        public TileSpawnData(GameObject prefab, float length, float rotationY, float xOffset, TilePhase phase)
        {
            this.prefab = prefab;
            this.length = length;
            this.rotationY = rotationY;
            this.xOffset = xOffset;
            this.phase = phase;
        }
    }

    void Start()
    {
        floorTiles.Clear();
        bossTileQueue.Clear();

        float currentZ = spawnStartZ;

        for (int i = 0; i < floorCount; i++)
        {
            TileSpawnData tileData = new TileSpawnData(
                normalFloorTilePrefab,
                normalTileLength,
                normalTileRotationY,
                normalTileXOffset,
                TilePhase.Normal
            );

            GameObject tile = SpawnTile(tileData, currentZ);

            if (tile != null)
            {
                currentZ -= normalTileLength;
            }
        }
    }

    void Update()
    {
        floorTiles.RemoveAll(tile => tile == null);

        int safetyCounter = 0;

        while (floorTiles.Count < floorCount && safetyCounter < 10)
        {
            SpawnNextTile();
            safetyCounter++;
        }
    }

    void SpawnNextTile()
    {
        GameObject furthestTile = GetFurthestBackTile();

        float newZ = spawnStartZ;

        if (furthestTile != null)
        {
            Transform movingTransform = GetMovingTransform(furthestTile);
            float previousLength = GetStoredLength(furthestTile);

            newZ = movingTransform.position.z - previousLength;
        }

        TileSpawnData nextTileData = GetNextTileData();
        SpawnTile(nextTileData, newZ);
    }

    TileSpawnData GetNextTileData()
    {
        if (bossTileQueue.Count > 0)
        {
            TileSpawnData nextBossTile = bossTileQueue.Dequeue();
            currentPhase = nextBossTile.phase;
            return nextBossTile;
        }

        currentPhase = TilePhase.Normal;

        return new TileSpawnData(
            normalFloorTilePrefab,
            normalTileLength,
            normalTileRotationY,
            normalTileXOffset,
            TilePhase.Normal
        );
    }

    GameObject SpawnTile(TileSpawnData tileData, float z)
    {
        if (tileData.prefab == null)
        {
            Debug.LogWarning("FloorSpawner: Missing prefab for phase: " + tileData.phase);
            return null;
        }

        Vector3 spawnPosition = new Vector3(spawnX + tileData.xOffset, spawnY, z);
        Quaternion spawnRotation = Quaternion.Euler(0f, tileData.rotationY, 0f);

        GameObject tile = Instantiate(tileData.prefab, spawnPosition, spawnRotation);

        FloorTileLengthMarker marker = tile.GetComponent<FloorTileLengthMarker>();

        if (marker == null)
        {
            marker = tile.AddComponent<FloorTileLengthMarker>();
        }

        marker.tileLength = tileData.length;
        marker.tilePhase = tileData.phase;

        floorTiles.Add(tile);

        Debug.Log(
            "FloorSpawner spawned: " +
            tile.name +
            " | Phase: " +
            tileData.phase +
            " | X: " +
            spawnPosition.x +
            " | Z: " +
            spawnPosition.z +
            " | Rotation Y: " +
            tileData.rotationY
        );

        return tile;
    }

    GameObject GetFurthestBackTile()
    {
        GameObject furthestTile = null;
        float furthestBackZ = 0f;

        foreach (GameObject tile in floorTiles)
        {
            if (tile == null)
            {
                continue;
            }

            Transform movingTransform = GetMovingTransform(tile);
            float tileZ = movingTransform.position.z;

            if (furthestTile == null || tileZ < furthestBackZ)
            {
                furthestBackZ = tileZ;
                furthestTile = tile;
            }
        }

        return furthestTile;
    }

    Transform GetMovingTransform(GameObject tile)
    {
        Transform movingTile = tile.transform.Find("MovingTile");

        if (movingTile != null)
        {
            return movingTile;
        }

        return tile.transform;
    }

    float GetStoredLength(GameObject tile)
    {
        FloorTileLengthMarker marker = tile.GetComponent<FloorTileLengthMarker>();

        if (marker != null)
        {
            return marker.tileLength;
        }

        return normalTileLength;
    }

    public void StartBossTunnelPhase()
    {
        if (bossSequenceStarted)
        {
            Debug.Log("FloorSpawner: Boss sequence already started.");
            return;
        }

        bossSequenceStarted = true;
        bossTileQueue.Clear();

        bossTileQueue.Enqueue(new TileSpawnData(
            bossEntranceTilePrefab,
            bossEntranceTileLength,
            bossEntranceTileRotationY,
            bossEntranceTileXOffset,
            TilePhase.BossEntrance
        ));

        for (int i = 0; i < bossMiddleTilesToSpawn; i++)
        {
            bossTileQueue.Enqueue(new TileSpawnData(
                bossMiddleTilePrefab,
                bossMiddleTileLength,
                bossMiddleTileRotationY,
                bossMiddleTileXOffset,
                TilePhase.BossMiddle
            ));
        }

        bossTileQueue.Enqueue(new TileSpawnData(
            bossExitTilePrefab,
            bossExitTileLength,
            bossExitTileRotationY,
            bossExitTileXOffset,
            TilePhase.BossExit
        ));

        Debug.Log("FloorSpawner: Boss tunnel sequence queued.");
    }

    public void ForceBossExitPhase()
    {
        bossTileQueue.Clear();

        bossTileQueue.Enqueue(new TileSpawnData(
            bossExitTilePrefab,
            bossExitTileLength,
            bossExitTileRotationY,
            bossExitTileXOffset,
            TilePhase.BossExit
        ));

        Debug.Log("FloorSpawner: Boss exit tile forced.");
    }
}

public class FloorTileLengthMarker : MonoBehaviour
{
    public float tileLength;
    public FloorSpawnerLogic.TilePhase tilePhase;
}