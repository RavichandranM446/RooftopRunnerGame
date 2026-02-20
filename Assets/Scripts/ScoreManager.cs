//using TMPro;
//using UnityEngine;

//public class ScoreManager : MonoBehaviour
//{
//    public static ScoreManager Instance;

//    public TextMeshProUGUI scoreText;
//    private int score = 0;

//    void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    void Start()
//    {
//        UpdateUI();
//    }

//    public void AddScore(int amount)
//    {
//        score += amount;
//        UpdateUI();
//    }

//    void UpdateUI()
//    {
//        scoreText.text = "SCORE : " + score;
//    }
//}
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;   // lowercase match

    public TextMeshProUGUI scoreText;
    public int score = 0;

    private bool boostTriggered = false;   // one-time trigger

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        score = 0;
        boostTriggered = false;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();

        // ?? Trigger speed boost at 100 orbs (only once)
        if (score >= 100 && !boostTriggered)
        {
            boostTriggered = true;

            if (GameSpeedManager.instance != null)
                GameSpeedManager.instance.TriggerSpeedBoost();
        }
    }

    // ?? HIGH SCORE LOGIC
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public bool TrySetHighScore()
    {
        int high = GetHighScore();

        if (score > high)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            return true; // new high score
        }

        return false;
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE : " + score;
    }
}
