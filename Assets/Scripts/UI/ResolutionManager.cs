using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{



    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fsToggle;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private Resolution currentResolution;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    public bool fullscreen = true;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }
        GetCurrentResPreference();
        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " * " + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (filteredResolutions[i].Equals(Screen.currentResolution))
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        fsToggle.SetIsOnWithoutNotify(fullscreen);
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void GetCurrentResPreference()
    {
        currentResolution = new Resolution();
        currentResolution.width = GamePreference.Width;
        currentResolution.height = GamePreference.Height;
        fullscreen = GamePreference.FullScreen;
        if (filteredResolutions.Contains(currentResolution))
        {
            SetResolutionDirectly(currentResolution);
        }
    }



    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        currentResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
        GamePreference.Width = resolution.width;
        GamePreference.Height = resolution.height;
        GamePreference.FullScreen = fullscreen;
    }

    public void SetFullscreen(bool fs)
    {
        fullscreen = fs;
        SetResolution(currentResolutionIndex);
    }

    public void SetResolutionDirectly(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
    }
}
