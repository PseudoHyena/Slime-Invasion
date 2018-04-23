using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour {

    [SerializeField] TMP_InputField seedInputField;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer effectsMixer;

    public static int Seed { get; private set; }

    void Start() {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float currentMusicVolume;
        musicMixer.GetFloat("MusicVolume", out currentMusicVolume);
        musicVolumeSlider.value = currentMusicVolume;

        float currentEffectsVolume;
        effectsMixer.GetFloat("EffectsVolume", out currentEffectsVolume);
        effectsVolumeSlider.value = currentEffectsVolume;
    }

    public void SetSeedFromInput() {
        Seed = seedInputField.text.GetHashCode();
    }

    public void StartLevel() {
        SceneManager.LoadScene("GeneratedLevel");
    }

    public void SetMusicVolume(float volume) {
        musicMixer.SetFloat("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume) {
        effectsMixer.SetFloat("EffectsVolume", volume);
    }

    public void Quit() {
        Application.Quit();
    }
}
