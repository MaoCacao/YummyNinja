using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    public Image iconImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    void Start()
    {
        RefreshIcon();
    }

    public void OnClickToggleMusic()
    {
        MusicManager.Instance.Toggle();
        RefreshIcon();
    }

    private void RefreshIcon()
    {
        if (MusicManager.Instance == null || iconImage == null) return;
        iconImage.sprite = MusicManager.Instance.isOn ? soundOnSprite : soundOffSprite;
    }
}