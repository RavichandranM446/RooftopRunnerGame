using UnityEngine;

public class ACVentRotator : MonoBehaviour
{
    public float rotateSpeed = 180f;   // speed
    public float maxRotation = -80f;    // ?? rotate only 90°

    private float rotatedAmount = 0f;

    void Update()
    {
        if (rotatedAmount >= maxRotation)
            return;

        float step = rotateSpeed * Time.deltaTime;

        // prevent over-rotate
        if (rotatedAmount + step > maxRotation)
            step = maxRotation - rotatedAmount;

        transform.Rotate(0f, step, 0f);

        rotatedAmount += step;
    }
}
