using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer audioMixer;
    
    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start(){
        resolutions = Screen.resolutions; //store all avalible resolutions on this hardware.
        resolutionDropdown.ClearOptions();

        //convert resolutions to options string list;
        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
       
        for (int i = 0; i < resolutions.Length; i++) {
            
            string option = resolutions[i].width + "x" + resolutions[i].height;
            
            if (options.Contains(option) == false) { options.Add(option); }

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) { currentResolutionIndex = i ; }
                }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterVolume(float volume) { audioMixer.SetFloat("MasterVolume", volume); }
    public void SetDialogueVolume(float volume) { audioMixer.SetFloat("DialogueVolume", volume); }
    public void SetSFXVolume(float volume) { audioMixer.SetFloat("SFXVolume", volume); }
    public void SetMusicVolume(float volume) { audioMixer.SetFloat("MusicVolume", volume); }

    public void SetGraphicsQuality(int qualityIndex) { QualitySettings.SetQualityLevel(qualityIndex); }

    public void SetResolution(int resolutionIndex) {
        //Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);  
    }

    public void SetFullscreen(bool isFullscreen) { Screen.fullScreen = isFullscreen; }


}
