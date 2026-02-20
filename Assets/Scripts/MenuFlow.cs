using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuFlow : MonoBehaviour
{
    public GameObject splashPanel;
    public GameObject menuPanel;

    public float splashTime = 2.5f;

    void Start()
    {
        splashPanel.SetActive(true);
        menuPanel.SetActive(false);

        StartCoroutine(ShowMenu());
    }

    IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(splashTime);

        splashPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // ?? your gameplay scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
