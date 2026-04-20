using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class FloorSpawnerLogic : MonoBehaviour
{
    public enum GenerationPhase
    {
        Normal,
        BossEntrance,
        BossMiddle,
        BossExit
    }

    [Header("Normal Floor Settings")]
    public GameObject normalFloorTilePrefab;

    [Header("Boss Floor Settings")]
    public GameObject bossEntranceTilePrefab;
    public GameObject bossMiddleTilePrefab;
    public GameObject bossExitTilePrefab;

    [Header("Spawner Settings")]
    public int floorCount = 5;
    public float floorLength = 30f;
    public float spawnX = -5f;
    public float spawnY = 0f;
    public float spawnStartZ = 0f;

    [Header("Current Phase")]
    public GenerationPhase currentPhase = GenerationPhase.Normal;

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

        while (floorTiles.Count < floorCount)
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
            Debug.LogWarning("No tile prefab assigned for current phase.");
            return;
        }

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, z);
        GameObject tile = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        floorTiles.Add(tile);

        Debug.Log("Spawned: " + tile.name + " at " + tile.transform.position);
    }

    GameObject GetNextTilePrefab()
    {
        if (currentPhase == GenerationPhase.Normal)
        {
            return normalFloorTilePrefab;
        }
        else if (currentPhase == GenerationPhase.BossEntrance)
        {
            currentPhase = GenerationPhase.BossMiddle;
            return bossEntranceTilePrefab;
        }
        else if (currentPhase == GenerationPhase.BossMiddle)
        {
            return bossMiddleTilePrefab;
        }
        else if (currentPhase == GenerationPhase.BossExit)
        {
            currentPhase = GenerationPhase.Normal;
            return bossExitTilePrefab;
        }

        return normalFloorTilePrefab;
    }

    float GetFurthestBackMovingTileZ()
    {
        float furthestBackZ = float.PositiveInfinity;

        foreach (GameObject tile in floorTiles)
        {
            if (tile == null) continue;

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

    public void StartBossPhase()
    {
        currentPhase = GenerationPhase.BossEntrance;
    }

    public void EndBossPhase()
    {
        currentPhase = GenerationPhase.BossExit;
    }
}