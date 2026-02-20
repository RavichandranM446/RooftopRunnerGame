using UnityEngine;

public class EnergyOrb : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public float floatSpeed = 2f;
    public float floatHeight = 0.3f;

    public int orbValue = 1;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // Float up & down
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + Vector3.up * yOffset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.instance != null)
                ScoreManager.instance.AddScore(orbValue);

            // ?? ORB SOUND + DUCK MUSIC
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.orbPickup);
                AudioManager.instance.DuckForOrb();
            }

            Destroy(gameObject);
        }
    }
}
