using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("UI References")]
    public GameObject settingsPanel;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    private const string MUSIC_KEY = "MusicEnabled";
    private const string SFX_KEY = "SFXEnabled";

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadSettings();

        // Hook toggle events
        if (musicToggle != null)
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);

        if (sfxToggle != null)
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
    }

    // ---------------- OPEN / CLOSE ----------------

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        // UI click sound
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(AudioManager.instance.uiClick);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // UI click sound
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(AudioManager.instance.uiClick);
    }

    // ---------------- TOGGLES ----------------

    void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(MUSIC_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AudioManager.instance != null)
        {
            if (isOn)
                AudioManager.instance.PlayBackgroundMusic();
            else
                AudioManager.instance.StopMusic();
        }
    }

    void OnSFXToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(SFX_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AudioManager.instance != null)
            AudioManager.instance.SetSFXEnabled(isOn);
    }

    // ---------------- LOAD SETTINGS ----------------

    void LoadSettings()
    {
        bool musicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        bool sfxEnabled = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;

        if (musicToggle != null)
            musicToggle.isOn = musicEnabled;

        if (sfxToggle != null)
            sfxToggle.isOn = sfxEnabled;

        // Apply immediately
        if (AudioManager.instance != null)
        {
            if (musicEnabled)
                AudioManager.instance.PlayBackgroundMusic();
            else
                AudioManager.instance.StopMusic();

            AudioManager.instance.SetSFXEnabled(sfxEnabled);
        }
    }
}
