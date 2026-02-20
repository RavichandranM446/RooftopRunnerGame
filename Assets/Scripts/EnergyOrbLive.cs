using UnityEngine;

public class EnergyOrbAlive : MonoBehaviour
{
    [Header("Rotation")]
    public float rotateSpeed = 90f;

    [Header("Floating")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // ?? Rotate
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        // ?? Float
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + Vector3.up * yOffset;
    }
}
