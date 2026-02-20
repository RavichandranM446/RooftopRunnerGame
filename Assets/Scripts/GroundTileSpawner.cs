//using UnityEngine;
//using System.Collections.Generic;

//public class GroundTileSpawner : MonoBehaviour
//{
//    [Header("References")]
//    public Transform player;
//    public GameObject groundPrefab;

//    [Header("Settings")]
//    public int startTiles = 3;
//    public float tileLength = 100f;
//    public float groundY = 0f;

//    [Header("Material Change")]
//    public Material[] groundMaterials;

//    private float spawnZ = 0f;
//    private List<GameObject> activeTiles = new List<GameObject>();

//    void Start()
//    {
//        if (player == null)
//        {
//            GameObject p = GameObject.FindGameObjectWithTag("Player");
//            if (p != null)
//                player = p.transform;
//        }

//        for (int i = 0; i < startTiles; i++)
//        {
//            SpawnTile();
//        }
//    }

//    void Update()
//    {
//        if (!player) return;

//        if (player.position.z + tileLength > spawnZ)
//        {
//            SpawnTile();
//            RemoveOldTile();
//        }
//    }

//    void SpawnTile()
//    {
//        GameObject tile = Instantiate(groundPrefab);

//        tile.transform.position = new Vector3(0f, groundY, spawnZ);

//        // ?? Assign random material
//        if (groundMaterials.Length > 0)
//        {
//            Renderer r = tile.GetComponent<Renderer>();
//            if (r != null)
//            {
//                int index = Random.Range(0, groundMaterials.Length);
//                r.material = groundMaterials[index];
//            }
//        }

//        activeTiles.Add(tile);
//        spawnZ += tileLength;
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

public class GroundTileSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject groundPrefab;

    [Header("Settings")]
    public int startTiles = 3;
    public float tileLength = 100f;
    public float groundY = 0f;

    [Header("Material Change")]
    public Material[] groundMaterials;

    [Header("Boost Settings")]
    public float normalCheckDistance = 100f;
    public float boostCheckDistance = 70f;

    private float spawnZ = 0f;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        for (int i = 0; i < startTiles; i++)
            SpawnTile();
    }

    void Update()
    {
        if (!player) return;

        float checkDist = normalCheckDistance;

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
        GameObject tile = Instantiate(groundPrefab);
        tile.transform.position = new Vector3(0f, groundY, spawnZ);

        if (groundMaterials.Length > 0)
        {
            Renderer r = tile.GetComponent<Renderer>();
            if (r != null)
            {
                int index = Random.Range(0, groundMaterials.Length);
                r.material = groundMaterials[index];
            }
        }

        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    void RemoveOldTile()
    {
        if (activeTiles.Count == 0) return;

        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}

