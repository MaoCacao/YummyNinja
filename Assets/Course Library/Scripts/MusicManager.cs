using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;

    public bool isOn = true;

    private const string PrefKey = "MusicOn";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null) musicSource = GetComponent<AudioSource>();
        isOn = PlayerPrefs.GetInt(PrefKey, 1) == 1;
        Apply();
    }

    public void Toggle()
    {
        isOn = !isOn;
        PlayerPrefs.SetInt(PrefKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
        Apply();
    }

    public void Apply()
    {
        if (musicSource == null) return;

        musicSource.mute = !isOn;

        // To pause, not to mute:
        // if (isOn && !musicSource.isPlaying) musicSource.Play();
        // if (!isOn && musicSource.isPlaying) musicSource.Pause();
    }
}