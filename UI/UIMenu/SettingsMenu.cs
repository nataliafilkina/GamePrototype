using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;

    private Resolution[] _resolutions;

    private void Start()
    {
        List<string> dropdownOptions = new List<string>();
        _resolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;
        int dropdownValue = 0;

        int index = 0;
        foreach (Resolution resolution in _resolutions )
        {
            dropdownOptions.Add(resolution.width + " x " + resolution.height);

            if(resolution.width == currentResolution.width && resolution.height == currentResolution.height)
                dropdownValue = index;

            index++;
        }

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(dropdownOptions);
        _resolutionDropdown.value = dropdownValue;
        _resolutionDropdown.RefreshShownValue();

    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetResolution(int  resolutionIndex) 
    {
        Resolution resolution = _resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
