using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;
    public GameObject videoMenu;
    public GameObject audioMenu;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToogle;
    public int[] screenWidths;
    int activeScreenResolutionIndex;

    void Start()
    {
        activeScreenResolutionIndex = PlayerPrefs.GetInt("Screen Resolution Index");
        bool isFullscreen = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;

        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[2].value = AudioManager.instance.SFXVolumePercent;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResolutionIndex;
        }

        fullscreenToogle.isOn = isFullscreen;
    }

    public void Play()
    {
        SceneManager.LoadScene("Main Level");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void SettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        videoMenu.SetActive(false);
        audioMenu.SetActive(false);
    }

    public void CreditsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ControlsMenu()
    {
        controlsMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void VideoMenu()
    {
        videoMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void AudioMenu()
    {
        audioMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResolutionIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("Screen Resolution Index", activeScreenResolutionIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }
        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResolutionIndex);
        }
        PlayerPrefs.SetInt("Fullscreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.SFX);
    }
}
