using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject instructionsPopup;

    public void OpenInstructions()
    {
        instructionsPopup.SetActive(true);
    }

    public void CloseInstructions()
    {
        instructionsPopup.SetActive(false);
    }
}
