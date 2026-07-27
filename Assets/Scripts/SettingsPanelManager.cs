using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        SaveData data = SaveManager.Instance?.LoadGame() ?? new SaveData();
        musicSlider.value = data.musicVolume;
        sfxSlider.value = data.sfxVolume;

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
        closeButton.onClick.RemoveListener(Close);
    }

    private void OnMusicChanged(float value)
    {
        SaveData data = SaveManager.Instance?.LoadGame() ?? new SaveData();
        data.musicVolume = value;
        SaveManager.Instance?.SaveGame(data);
        AudioListener.volume = value; // simple global hookup; refine once real audio mixing exists
    }

    private void OnSfxChanged(float value)
    {
        SaveData data = SaveManager.Instance?.LoadGame() ?? new SaveData();
        data.sfxVolume = value;
        SaveManager.Instance?.SaveGame(data);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}