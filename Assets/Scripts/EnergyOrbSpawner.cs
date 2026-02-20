using UnityEngine;

public class EnergyOrbSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject energyOrbPrefab;

    [Header("Spawn Control")]
    public float spawnAheadDistance = 30f;   // always in front of player
    public float spawnInterval = 20f;        // distance between spawns
    public int minOrbs = 3;
    public int maxOrbs = 5;

    [Header("Lane Settings")]
    public float laneDistance = 2f;
    public float orbHeight = 11.5f;
    public float orbGap = 1.5f;

    private float lastSpawnZ;

    void Start()
    {
        if (player != null)
            lastSpawnZ = player.position.z;
    }

    void Update()
    {
        if (!player) return;

        if (player.position.z > lastSpawnZ + spawnInterval)
        {
            TrySpawn();
            lastSpawnZ = player.position.z;
        }
    }

    void TrySpawn()
    {
        float spawnZ = player.position.z + spawnAheadDistance;

        // 🔥 CHOOSE LANE FIRST
        int laneIndex = Random.Range(0, 3); // 0,1,2
        float laneX = (laneIndex - 1) * laneDistance;

        // 🔍 RAYCAST UNDER THE SAME LANE
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(laneX, 20f, spawnZ);

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 40f))
        {
            // ❌ DO NOT SPAWN ON GLASS
            if (hit.collider.CompareTag("Glass"))
                return;
        }

        SpawnOrbLine(laneX, spawnZ);
    }

    void SpawnOrbLine(float laneX, float spawnZ)
    {
        int orbCount = Random.Range(minOrbs, maxOrbs + 1);

        for (int i = 0; i < orbCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                laneX,
                orbHeight,
                spawnZ + i * orbGap
            );

            Instantiate(energyOrbPrefab, spawnPos, Quaternion.identity);
        }
    }
}
