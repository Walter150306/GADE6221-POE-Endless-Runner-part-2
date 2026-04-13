using System;
using System.Threading;
using UnityEngine;

public class ObstacleSpawnerLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 spawnPosition;
    [Header("Spawner Settings")] // various settings for the spawner (
    public GameObject barrelPrefab;
    public GameObject capsuleObstacle;
    public GameObject cylinderObstacle;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnDistance = 20f;
    private float timeCounter = 0;

    [Header("Difficulty Settings")]
    public float minInterval = 0.5f;
    public float difficultyRampSpeed = 0.1f;
    private float timer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            System.Random randObstacle = new System.Random();
            int choice = randObstacle.Next(0, 3);

            if (choice == 0)
            {
                spawnBarrel();
            }
            else if (choice == 1)
            {
                SpawnCapsule();
            }
            else
            {
                spawnCylinder();
            }

            timer = 0f;
        }
    }

    void spawnBarrel()
    {
        timeCounter += Time.deltaTime;
        // Calculate spawn position in front of the player
        System.Random rand = new System.Random();
        int randomPosition = rand.Next(0, 3); // Randomly select one of the three positions
                                              
        Vector3 spawnPosition = Vector3.zero;

        if (randomPosition == 0)
        {
            spawnPosition = new Vector3(-7.12f, 0.72f, spawnDistance); // Left lane
        }
        else if (randomPosition == 1)
        {
            spawnPosition = new Vector3(-8.12f, 0.72f, spawnDistance); // Middle lane
        }
        else if (randomPosition == 2)
        {
            spawnPosition = new Vector3(-6.12f, 0.72f, spawnDistance); // Right lane
        }
        //spawnPosition.y = 0.5f; // Adjust height if necessary
        // Instantiate the barrel obstacle
        GameObject barrel = Instantiate(barrelPrefab, spawnPosition, Quaternion.identity);
        barrel.GetComponent<BarrelLogic>().player = player;

    }
    void SpawnCapsule()
    {
        timeCounter += Time.deltaTime;
        // Calculate spawn position in front of the player
        System.Random rand = new System.Random();
        int randomPosition = rand.Next(0, 3); // Randomly select one of the three positions
                                              // ✅ Declared outside so Instantiate can see it
        Vector3 spawnPosition = Vector3.zero;

        if (randomPosition == 0)
        {
            spawnPosition = new Vector3(-7.12f, 0.72f, spawnDistance); // Left lane
        }
        else if (randomPosition == 1)
        {
            spawnPosition = new Vector3(-8.12f, 0.72f, spawnDistance); // Middle lane
        }
        else if (randomPosition == 2)
        {
            spawnPosition = new Vector3(-6.12f, 0.72f, spawnDistance); // Right lane
        }
        //spawnPosition.y = 0.5f; // Adjust height if necessary
        // Instantiate the barrel obstacle
        GameObject capsule = Instantiate(capsuleObstacle, spawnPosition, Quaternion.identity);
        capsule.GetComponent<BarrelLogic>().player = player;
    }

    void spawnCylinder()
    {
        timeCounter += Time.deltaTime;
        // Calculate spawn position in front of the player
        System.Random rand = new System.Random();
        int randomPosition = rand.Next(0, 3); // Randomly select one of the three positions
                                              // ✅ Declared outside so Instantiate can see it
        Vector3 spawnPosition = Vector3.zero;

        if (randomPosition == 0)
        {
            spawnPosition = new Vector3(-7.12f, 0.72f, spawnDistance); // Left lane
        }
        else if (randomPosition == 1)
        {
            spawnPosition = new Vector3(-8.12f, 0.72f, spawnDistance); // Middle lane
        }
        else if (randomPosition == 2)
        {
            spawnPosition = new Vector3(-6.12f, 0.72f, spawnDistance); // Right lane
        }
        //spawnPosition.y = 0.5f; // Adjust height if necessary
        // Instantiate the barrel obstacle
        GameObject cylinder = Instantiate(cylinderObstacle, spawnPosition, Quaternion.identity);
        cylinder.GetComponent<BarrelLogic>().player = player;
    }



}

