using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;       // ?? High score
    public TextMeshProUGUI newHighScoreText;    // ?? "NEW HIGH SCORE!"

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(false);
            newHighScoreText.transform.localScale = Vector3.one;
        }
    }

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (scoreText != null)
            scoreText.text = "SCORE : " + finalScore;

        bool isNewHigh = false;

        if (ScoreManager.instance != null)
        {
            isNewHigh = ScoreManager.instance.TrySetHighScore();
            int high = ScoreManager.instance.GetHighScore();

            if (highScoreText != null)
                highScoreText.text = "HIGH SCORE : " + high;
        }

        // ?? Bounce HIGH SCORE
        if (highScoreText != null)
            StartCoroutine(BounceText(highScoreText.transform));

        // ?? Show + Glow + Bounce NEW HIGH SCORE
        if (newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(isNewHigh);

            if (isNewHigh)
            {
                StartCoroutine(BounceText(newHighScoreText.transform));
                StartCoroutine(GlowText(newHighScoreText));
            }
        }

        Time.timeScale = 0f;   // Pause game
    }

    // ---------------- BUTTONS ----------------

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ---------------- BOUNCE ANIMATION ----------------

    IEnumerator BounceText(Transform textTransform)
    {
        Vector3 startScale = Vector3.one * 0.7f;
        Vector3 overshootScale = Vector3.one * 1.1f;
        Vector3 normalScale = Vector3.one;

        float t = 0f;

        // Scale up (0.7 -> 1.1)
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 4f;
            textTransform.localScale = Vector3.Lerp(startScale, overshootScale, t);
            yield return null;
        }

        t = 0f;

        // Settle (1.1 -> 1.0)
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 4f;
            textTransform.localScale = Vector3.Lerp(overshootScale, normalScale, t);
            yield return null;
        }

        textTransform.localScale = normalScale;
    }

    // ---------------- GLOW EFFECT ----------------

    IEnumerator GlowText(TextMeshProUGUI text)
    {
        if (text == null) yield break;

        Color baseColor = text.color;
        Color glowColor = new Color(1f, 0.9f, 0.2f, 1f); // gold glow

        float t = 0f;

        // Glow in
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            text.color = Color.Lerp(baseColor, glowColor, t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.6f);

        t = 0f;

        // Glow out
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            text.color = Color.Lerp(glowColor, baseColor, t);
            yield return null;
        }

        text.color = baseColor;
    }
}
