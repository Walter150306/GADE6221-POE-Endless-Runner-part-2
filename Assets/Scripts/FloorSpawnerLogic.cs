using System.Collections.Generic;
using UnityEngine;

public class FloorSpawnerLogic : MonoBehaviour
{
    [Header("Floor Settings")]
    public GameObject floorTilePrefab;
    public int floorCount = 5;
    public float floorLength = 30f;
    public float spawnX = -5f;
    public float spawnY = 0f;
    public float spawnStartZ = 0f;

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
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, z);
        GameObject tile = Instantiate(floorTilePrefab, spawnPosition, Quaternion.identity);
        floorTiles.Add(tile);

        Debug.Log("Spawned: " + tile.name + " at " + tile.transform.position);
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
}