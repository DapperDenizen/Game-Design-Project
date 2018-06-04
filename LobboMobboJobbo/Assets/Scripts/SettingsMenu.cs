using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;


public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions; //get computer resolutions

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>(); //list of resolutions

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //string that displays res
            options.Add(option); //add to list

            if(resolutions[i].width == Screen.currentResolution.width && //apparently can't compare resolution types
                resolutions[i].height == Screen.currentResolution.height) //which is why we're comparing width and height instead
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); //add list strings to dropdown
        resolutionDropdown.value = currentResolutionIndex; //get the current scren res and make default choice
        resolutionDropdown.RefreshShownValue(); //display the new default res
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex]; //use index to find the right amounts from array
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Debug.Log("on off");
        Screen.fullScreen = isFullscreen;
    }

}
