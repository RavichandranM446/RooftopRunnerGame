using UnityEngine;

public class CameraCurveController : MonoBehaviour
{
    [Header("Curve Settings")]
    public float maxYaw = 10f;        // curve strength (degrees)
    public float curveSpeed = 2f;     // how fast camera turns
    public float curveDuration = 2f;  // how long curve stays

    private float targetYaw = 0f;
    private float timer = 0f;

    void Update()
    {
        // Smooth rotation only on Y (yaw)
        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * curveSpeed
        );

        // Reset after duration
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                targetYaw = 0f; // back to straight
            }
        }
    }

    // ?? Call this to trigger curve
    public void TriggerCurve(bool left)
    {
        targetYaw = left ? -maxYaw : maxYaw;
        timer = curveDuration;
    }
}
