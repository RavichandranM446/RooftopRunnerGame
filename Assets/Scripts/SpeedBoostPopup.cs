using UnityEngine;
using TMPro;
using System.Collections;

public class SpeedBoostPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float fadeInTime = 0.3f;
    public float stayTime = 1.2f;
    public float fadeOutTime = 0.5f;

    Coroutine popupRoutine;

    void Start()
    {
        if (popupText == null)
            popupText = GetComponent<TextMeshProUGUI>();

        SetAlpha(0f);
    }

    public void ShowPopup()
    {
        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine = StartCoroutine(PopupSequence());
    }

    IEnumerator PopupSequence()
    {
        yield return Fade(0f, 1f, fadeInTime);
        yield return new WaitForSeconds(stayTime);
        yield return Fade(1f, 0f, fadeOutTime);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(to);
    }

    void SetAlpha(float a)
    {
        Color c = popupText.color;
        c.a = a;
        popupText.color = c;
    }
}
