//using UnityEngine;
//using System.Collections.Generic;

//public class TileSpawner : MonoBehaviour
//{
//    [Header("Prefabs")]
//    public GameObject roofTilePrefab;
//    public GameObject glassTilePrefab;
//    public Transform player;

//    [Header("Settings")]
//    public int startTiles = 7;
//    public float tileY = 10f;

//    [Header("Glass Settings")]
//    public float glassSpawnDistance = 25f;

//    private float spawnZ = 0f;
//    private float nextGlassZ = 25f;
//    private bool glassSpawnedLast = false;

//    private List<GameObject> activeTiles = new List<GameObject>();

//    void Start()
//    {
//        for (int i = 0; i < startTiles; i++)
//        {
//            SpawnTile();
//        }
//    }

//    void Update()
//    {
//        if (player.position.z + 30f > spawnZ)
//        {
//            SpawnTile();
//            RemoveOldTile();
//        }
//    }

//    void SpawnTile()
//    {
//        GameObject prefabToSpawn;
//        bool spawnedGlass = false;

//        // ?? GLASS TILE LOGIC
//        if (!glassSpawnedLast && spawnZ >= nextGlassZ)
//        {
//            prefabToSpawn = glassTilePrefab;
//            glassSpawnedLast = true;
//            spawnedGlass = true;

//            nextGlassZ = spawnZ + glassSpawnDistance;
//        }
//        else
//        {
//            prefabToSpawn = roofTilePrefab;
//            glassSpawnedLast = false;
//        }

//        GameObject tile = Instantiate(prefabToSpawn);

//        float length = tile.GetComponent<Renderer>().bounds.size.z;

//        tile.transform.position = new Vector3(
//            0f,
//            tileY,
//            spawnZ + length / 2f
//        );

//        activeTiles.Add(tile);
//        spawnZ += length;

//        // ?? RARE CAMERA CURVE (ONLY ON ROOF TILE)
//        if (!spawnedGlass && Random.Range(0, 6) == 0)
//        {
//            CameraCurveController camCurve =
//                FindObjectOfType<CameraCurveController>();

//            if (camCurve != null)
//            {
//                bool left = Random.value > 0.5f;
//                camCurve.TriggerCurve(left);
//            }
//        }
//    }

//    void RemoveOldTile()
//    {
//        if (activeTiles.Count == 0) return;

//        Destroy(activeTiles[0]);
//        activeTiles.RemoveAt(0);
//    }
//}
using UnityEngine;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject roofTilePrefab;
    public GameObject glassTilePrefab;
    public Transform player;

    [Header("Settings")]
    public int startTiles = 7;
    public float tileY = 10f;

    [Header("Glass Settings")]
    public float glassSpawnDistance = 25f;

    [Header("Spawn Distance Control")]
    public float normalCheckDistance = 45f;   // ?? increased (was 30)
    public float boostCheckDistance = 75f;    // ?? much earlier spawn during boost

    private float spawnZ = 0f;
    private float nextGlassZ = 25f;
    private bool glassSpawnedLast = false;

    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < startTiles; i++)
            SpawnTile();
    }

    void Update()
    {
        if (!player) return;

        float checkDist = normalCheckDistance;

        // ?? Boost = spawn even earlier
        if (GameSpeedManager.instance != null &&
            GameSpeedManager.instance.IsBoostActive())
        {
            checkDist = boostCheckDistance;
        }

        if (player.position.z + checkDist > spawnZ)
        {
            SpawnTile();
            RemoveOldTile();
        }
    }

    void SpawnTile()
    {
        GameObject prefabToSpawn;
        bool spawnedGlass = false;

        if (!glassSpawnedLast && spawnZ >= nextGlassZ)
        {
            prefabToSpawn = glassTilePrefab;
            glassSpawnedLast = true;
            spawnedGlass = true;
            nextGlassZ = spawnZ + glassSpawnDistance;
        }
        else
        {
            prefabToSpawn = roofTilePrefab;
            glassSpawnedLast = false;
        }

        GameObject tile = Instantiate(prefabToSpawn);

        float length = tile.GetComponent<Renderer>().bounds.size.z;

        tile.transform.position = new Vector3(
            0f,
            tileY,
            spawnZ + length / 2f
        );

        activeTiles.Add(tile);
        spawnZ += length;

        // camera curve logic unchanged
        if (!spawnedGlass && Random.Range(0, 6) == 0)
        {
            CameraCurveController camCurve =
                FindObjectOfType<CameraCurveController>();

            if (camCurve != null)
            {
                bool left = Random.value > 0.5f;
                camCurve.TriggerCurve(left);
            }
        }
    }

    void RemoveOldTile()
    {
        if (activeTiles.Count == 0) return;

        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}

