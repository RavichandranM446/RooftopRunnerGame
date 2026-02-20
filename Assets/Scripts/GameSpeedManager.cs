using UnityEngine;
using System.Collections;

public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager instance;

    [Header("Speed Boost Settings")]
    public float boostMultiplier = 1.2f;
    public float boostDuration = 120f;   // ?? 2 minutes
    public SpeedBoostPopup popupUI;

    [Header("References")]
    public PlayerMovement player;
    public GameObject playerGlow;

    public bool isBoostActive = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        if (playerGlow != null)
            playerGlow.SetActive(false);
    }

    public bool IsBoostActive()
    {
        return isBoostActive;
    }

    public void TriggerSpeedBoost()
    {
        if (isBoostActive) return;

        if (popupUI != null)
            popupUI.ShowPopup();

        StartCoroutine(SpeedBoostRoutine());
    }

    IEnumerator SpeedBoostRoutine()
    {
        isBoostActive = true;

        float originalSpeed = player.forwardSpeed;

        // ?? speed up
        player.forwardSpeed = originalSpeed * boostMultiplier;

        // ?? glow ON
        if (playerGlow != null)
            playerGlow.SetActive(true);

        yield return new WaitForSeconds(boostDuration);

        // ?? reset speed
        player.forwardSpeed = originalSpeed;

        // ?? glow OFF
        if (playerGlow != null)
            playerGlow.SetActive(false);

        isBoostActive = false;
    }
}
