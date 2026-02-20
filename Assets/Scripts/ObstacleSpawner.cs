//using UnityEngine;
//using System.Collections.Generic;

//public class ObstacleSpawner : MonoBehaviour
//{
//    [Header("References")]
//    public Transform player;
//    public GameObject jumpObstaclePrefab;     // Cube
//    public GameObject slideObstaclePrefab;    // Bar
//    public GameObject energyOrbPrefab;

//    [Header("Spawn Settings")]
//    public float spawnAheadDistance = 35f;
//    public float spawnInterval = 20f;

//    [Header("Lane Settings")]
//    public float laneDistance = 2f;
//    public float cubeY = 11f;
//    public float cylinderY = 15.8f;     // ?? lowered so it sits on roof
//    public float orbYOffset = 1.2f;

//    [Header("Cleanup")]
//    public float destroyBehindDistance = 20f;

//    private float lastSpawnZ;
//    private List<GameObject> activeObstacles = new List<GameObject>();

//    void Start()
//    {
//        if (player == null)
//        {
//            GameObject p = GameObject.FindGameObjectWithTag("Player");
//            if (p != null)
//                player = p.transform;
//        }

//        lastSpawnZ = player.position.z;
//    }

//    void Update()
//    {
//        if (!player) return;

//        if (player.position.z > lastSpawnZ + spawnInterval)
//        {
//            TrySpawnObstacle();
//            lastSpawnZ = player.position.z;
//        }

//        CleanupObstacles();
//    }

//    // ---------------- MAIN SPAWN LOGIC ----------------
//    void TrySpawnObstacle()
//    {
//        float spawnZ = player.position.z + spawnAheadDistance;

//        RaycastHit hit;
//        Vector3 rayOrigin = new Vector3(0f, 25f, spawnZ);

//        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 40f))
//        {
//            if (hit.collider.CompareTag("Glass"))
//                return;
//        }

//        int roll = Random.Range(0, 100);

//        if (roll < 25)
//            SpawnJumpObstacle(spawnZ);
//        else if (roll < 40)
//            SpawnSlideObstacle(spawnZ);
//    }

//    // ---------------- JUMP OBSTACLE ----------------
//    void SpawnJumpObstacle(float spawnZ)
//    {
//        int laneCount = Random.Range(1, 3);
//        List<int> lanes = new List<int> { 0, 1, 2 };

//        for (int i = 0; i < laneCount; i++)
//        {
//            if (lanes.Count == 0) break;

//            int index = Random.Range(0, lanes.Count);
//            int lane = lanes[index];
//            lanes.RemoveAt(index);

//            float laneX = (lane - 1) * laneDistance;
//            Vector3 pos = new Vector3(laneX, cubeY, spawnZ);

//            GameObject cube = Instantiate(jumpObstaclePrefab, pos, Quaternion.identity);

//            // ?? ensure correct tag
//            cube.tag = "JumpObstacle";

//            activeObstacles.Add(cube);

//            SpawnOrbsOnCube(cube.transform.position);
//        }
//    }

//    // ---------------- SLIDE OBSTACLE (BAR) ----------------
//    void SpawnSlideObstacle(float spawnZ)
//    {
//        int lane = Random.Range(0, 3);

//        float laneX = (lane - 1) * laneDistance;
//        Vector3 pos = new Vector3(laneX, cylinderY, spawnZ);

//        // ?? FORCE BAR ROTATION (HORIZONTAL)
//        Quaternion rot = Quaternion.Euler(0f, 0f, -90f);

//        GameObject bar = Instantiate(slideObstaclePrefab, pos, rot);

//        // ?? ensure correct tag for hit detection
//        bar.tag = "SlideObstacle";

//        activeObstacles.Add(bar);
//    }

//    // ---------------- ORBS ----------------
//    void SpawnOrbsOnCube(Vector3 cubePos)
//    {
//        if (energyOrbPrefab == null) return;

//        int orbCount = Random.Range(2, 5);

//        for (int i = 0; i < orbCount; i++)
//        {
//            Vector3 orbPos = cubePos;
//            orbPos.y += orbYOffset;
//            orbPos.z += i * 1.2f;

//            GameObject orb = Instantiate(energyOrbPrefab, orbPos, Quaternion.identity);
//            activeObstacles.Add(orb);
//        }
//    }

//    // ---------------- CLEANUP ----------------
//    void CleanupObstacles()
//    {
//        for (int i = activeObstacles.Count - 1; i >= 0; i--)
//        {
//            if (activeObstacles[i] == null)
//            {
//                activeObstacles.RemoveAt(i);
//                continue;
//            }

//            if (activeObstacles[i].transform.position.z < player.position.z - destroyBehindDistance)
//            {
//                Destroy(activeObstacles[i]);
//                activeObstacles.RemoveAt(i);
//            }
//        }
//    }
//}
using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject jumpObstaclePrefab;
    public GameObject slideObstaclePrefab;
    public GameObject energyOrbPrefab;

    [Header("Spawn Settings")]
    public float spawnAheadDistance = 35f;

    [Header("Boost Spawn Settings")]
    public float normalSpawnInterval = 20f;
    public float boostSpawnInterval = 12f;

    [Header("Lane Settings")]
    public float laneDistance = 2f;
    public float cubeY = 11f;
    public float cylinderY = 15.8f;
    public float orbYOffset = 1.2f;

    [Header("Cleanup")]
    public float destroyBehindDistance = 20f;

    private float lastSpawnZ;
    private List<GameObject> activeObstacles = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        lastSpawnZ = player.position.z;
    }

    void Update()
    {
        if (!player) return;

        float currentInterval = normalSpawnInterval;

        if (GameSpeedManager.instance != null &&
            GameSpeedManager.instance.IsBoostActive())
        {
            currentInterval = boostSpawnInterval;
        }

        if (player.position.z > lastSpawnZ + currentInterval)
        {
            TrySpawnObstacle();
            lastSpawnZ = player.position.z;
        }

        CleanupObstacles();
    }

    void TrySpawnObstacle()
    {
        float spawnZ = player.position.z + spawnAheadDistance;

        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(0f, 25f, spawnZ);

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 40f))
        {
            if (hit.collider.CompareTag("Glass"))
                return;
        }

        int roll = Random.Range(0, 100);

        if (roll < 25)
            SpawnJumpObstacle(spawnZ);
        else if (roll < 40)
            SpawnSlideObstacle(spawnZ);
    }

    void SpawnJumpObstacle(float spawnZ)
    {
        int laneCount = Random.Range(1, 3);
        List<int> lanes = new List<int> { 0, 1, 2 };

        for (int i = 0; i < laneCount; i++)
        {
            if (lanes.Count == 0) break;

            int index = Random.Range(0, lanes.Count);
            int lane = lanes[index];
            lanes.RemoveAt(index);

            float laneX = (lane - 1) * laneDistance;
            Vector3 pos = new Vector3(laneX, cubeY, spawnZ);

            GameObject cube = Instantiate(jumpObstaclePrefab, pos, Quaternion.identity);
            cube.tag = "JumpObstacle";

            activeObstacles.Add(cube);
            SpawnOrbsOnCube(cube.transform.position);
        }
    }

    void SpawnSlideObstacle(float spawnZ)
    {
        int lane = Random.Range(0, 3);

        float laneX = (lane - 1) * laneDistance;
        Vector3 pos = new Vector3(laneX, cylinderY, spawnZ);

        // ?? FORCE BAR ROTATION
        Quaternion rot = Quaternion.Euler(0f, 0f, -90f);

        GameObject bar = Instantiate(slideObstaclePrefab, pos, rot);
        bar.tag = "SlideObstacle";

        activeObstacles.Add(bar);
    }

    void SpawnOrbsOnCube(Vector3 cubePos)
    {
        if (energyOrbPrefab == null) return;

        int orbCount = Random.Range(2, 5);

        for (int i = 0; i < orbCount; i++)
        {
            Vector3 orbPos = cubePos;
            orbPos.y += orbYOffset;
            orbPos.z += i * 1.2f;

            GameObject orb = Instantiate(energyOrbPrefab, orbPos, Quaternion.identity);
            activeObstacles.Add(orb);
        }
    }

    void CleanupObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }

            if (activeObstacles[i].transform.position.z <
                player.position.z - destroyBehindDistance)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }
}

