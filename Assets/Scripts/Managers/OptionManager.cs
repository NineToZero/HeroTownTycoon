using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public OptionData OptionData;

    private void Awake()
    {
        SetOptionData();
    }

    private void Start()
    {
        OptionData.ExecuteAllEvent();
    }

    private void SetOptionData()
    {
        OptionData = new OptionData();
        OptionData.Init();

        OptionData.OnChangeFullScreen += RefreshFullScreen;
        OptionData.OnChangeResolution += RefreshResolution;
    }

    private void RefreshResolution()
    {
        int index = OptionData.ResolutionIndex;
        bool isFullscreen = OptionData.IsFullScreen;
        Resolution resolution = OptionData.Resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height,isFullscreen);
    }

    private void RefreshFullScreen()
    {
        bool isFullScreen = OptionData.IsFullScreen;
        Screen.fullScreen = isFullScreen;
    }
}
