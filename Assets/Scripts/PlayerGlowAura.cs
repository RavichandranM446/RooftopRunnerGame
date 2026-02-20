using UnityEngine;

public class PlayerGlowAura : MonoBehaviour
{
    public GameObject auraObject;   // glow mesh or particle
    public GameSpeedManager speedManager;

    void Start()
    {
        if (speedManager == null)
            speedManager = FindObjectOfType<GameSpeedManager>();

        if (auraObject != null)
            auraObject.SetActive(false);   // start hidden
    }

    void Update()
    {
        if (speedManager == null || auraObject == null) return;

        if (speedManager.isBoostActive)
        {
            if (!auraObject.activeSelf)
                auraObject.SetActive(true);
        }
        else
        {
            if (auraObject.activeSelf)
                auraObject.SetActive(false);
        }
    }

}
