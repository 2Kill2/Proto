using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;

    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public Toggle fullscreenToggle;

    [Header("Audio Settings")]
    public AudioMixer mainMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;


    // Switches to the specified level
    public void SwitchLevel(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Opens the settings menu and closes the main menu
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    // Closes the settings menu and returns to the main menu
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Opens the credits menu and closes the main menu
    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    // Closes the credits menu and returns to the main menu
    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    // RESOLUTION AND AUDIO SETTINGS

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        int savedIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedIndex);

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = PlayerPrefs.GetInt("isFullscreen", 1) == 1;

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // AUDIO SETTINGS
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.75f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("isFullscreen", isFullscreen ? 1 : 0);
    }

    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
