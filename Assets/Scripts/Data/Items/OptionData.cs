using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OptionData
{
    public List<Resolution> Resolutions { get; private set; }
    // Screen
    private bool isFullScreen;
    private int _resolutionIndex;
    private float _mouseSensitivity;

    // Sound
    private bool _masterToggle;
    private bool _musicToggle;
    private bool _sfxToggle;

    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;

    #region Property
    public bool IsFullScreen
    {
        get { return isFullScreen; }
        set
        {
            isFullScreen = value;
            OnChangeFullScreen?.Invoke();
        }
    }
    public int ResolutionIndex
    {
        get { return _resolutionIndex; }
        set
        {
            _resolutionIndex = value;
            OnChangeResolution?.Invoke();
        }
    }

    public float MouseSensitivity 
    { 
        get { return _mouseSensitivity; } 
        set
        {
            _mouseSensitivity = value;
            OnChangeMouseSensitivity?.Invoke();
        }
    }

    public bool MasterToggle
    {
        get { return _masterToggle; }
        set
        {
            _masterToggle = value;
            OnChangeMasterToggle?.Invoke();
        }
    }
    public bool MusicToggle
    {
        get { return _musicToggle; }
        set
        {
            _musicToggle = value;
            OnChangeMusicToggle?.Invoke();
        }
    }
    public bool SFXToggle
    {
        get { return _sfxToggle; }
        set
        {
            _sfxToggle = value;
            OnChangeSFXToggle?.Invoke();
        }
    }

    public float MasterVolume
    {
        get { return _masterVolume; }
        set
        {
            _masterVolume = value;
            OnChangeMasterVolume?.Invoke();
        }
    }
    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            _musicVolume = value;
            OnChangeMusicVolume?.Invoke();
        }
    }
    public float SFXVolume
    {
        get { return _sfxVolume; }
        set
        {
            _sfxVolume = value;
            OnChangeSFXVolume?.Invoke();
        }
    }
    #endregion

    // Event
    public event Action OnChangeFullScreen;
    public event Action OnChangeResolution;
    public event Action OnChangeMouseSensitivity;

    public event Action OnChangeMasterToggle;
    public event Action OnChangeMusicToggle;
    public event Action OnChangeSFXToggle;

    public event Action OnChangeMasterVolume;
    public event Action OnChangeMusicVolume;
    public event Action OnChangeSFXVolume;

    public void Init()
    {
        SetScreen();
        SetMouse();
        SetVolume();

        OnChangeMouseSensitivity += () => PlayerPrefs.SetFloat(Const.PlayerPrefs_MouseSensitivity, MouseSensitivity);
        OnChangeMasterToggle     += () => PlayerPrefs.SetInt(Const.PlayerPrefs_MasterToggle, Util.BooleanToInt(MasterToggle));
        OnChangeMusicToggle      += () => PlayerPrefs.SetInt(Const.PlayerPrefs_MusicToggle, Util.BooleanToInt(MusicToggle));
        OnChangeSFXToggle        += () => PlayerPrefs.SetInt(Const.PlayerPrefs_SFXToggle, Util.BooleanToInt(SFXToggle));
        OnChangeMasterVolume     += () => PlayerPrefs.SetFloat(Const.PlayerPrefs_MasterVolume, MasterVolume);
        OnChangeMusicVolume      += () => PlayerPrefs.SetFloat(Const.PlayerPrefs_MusicVolume, MusicVolume);
        OnChangeSFXVolume        += () => PlayerPrefs.SetFloat(Const.PlayerPrefs_SFXVolume, SFXVolume);
    }

    public void SetScreen()
    {
        Resolution[] temp = Screen.resolutions;
        Resolutions = new();

        bool isFind = false;
        int index = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].refreshRateRatio.value != Screen.currentResolution.refreshRateRatio.value)
                continue;

            Resolutions.Add(temp[i]);

            int currenHeight = Screen.height;
            int currenWidth = Screen.width;

            if (isFind == false
                 && temp[i].height == currenHeight
                 && temp[i].width == currenWidth)
            {
                isFind = true;
                ResolutionIndex = index;
            }
            index++;
        }

        isFullScreen = Screen.fullScreen;
    }

    public void SetVolume()
    {
        if (PlayerPrefs.HasKey(Const.PlayerPrefs_NewGame))
        {
            LoadVolume();
            return;
        }

        MasterToggle = true;
        MusicToggle = true;
        SFXToggle = true;
        
        MouseSensitivity = 1;
        MasterVolume = 1;
        MusicVolume = 0.5f;
        SFXVolume = 1;

        PlayerPrefs.SetInt(Const.PlayerPrefs_NewGame, 1);
        PlayerPrefs.SetInt(Const.PlayerPrefs_MasterToggle, Util.BooleanToInt(MasterToggle));
        PlayerPrefs.SetInt(Const.PlayerPrefs_MusicToggle, Util.BooleanToInt(MusicToggle));
        PlayerPrefs.SetFloat(Const.PlayerPrefs_MouseSensitivity, MouseSensitivity);
        PlayerPrefs.SetInt(Const.PlayerPrefs_SFXToggle, Util.BooleanToInt(SFXToggle));
        PlayerPrefs.SetFloat(Const.PlayerPrefs_MasterVolume, MasterVolume);
        PlayerPrefs.SetFloat(Const.PlayerPrefs_MusicVolume, MusicVolume);
        PlayerPrefs.SetFloat(Const.PlayerPrefs_SFXVolume, SFXVolume);
    }

    private void LoadVolume()
    {
        MasterToggle = Convert.ToBoolean(PlayerPrefs.GetInt(Const.PlayerPrefs_MasterToggle));
        MusicToggle = Convert.ToBoolean(PlayerPrefs.GetInt(Const.PlayerPrefs_MusicToggle)); ;
        SFXToggle = Convert.ToBoolean(PlayerPrefs.GetInt(Const.PlayerPrefs_SFXToggle));
        MasterVolume = PlayerPrefs.GetFloat(Const.PlayerPrefs_MasterVolume);
        MusicVolume = PlayerPrefs.GetFloat(Const.PlayerPrefs_MusicVolume);
        SFXVolume = PlayerPrefs.GetFloat(Const.PlayerPrefs_SFXVolume);
    }

    private void SetMouse()
    {
        float number = PlayerPrefs.GetFloat(Const.PlayerPrefs_MouseSensitivity);
        if (number == 0) 
            number = 1.0f;

        MouseSensitivity = number;
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetInt(Const.PlayerPrefs_MasterToggle, Util.BooleanToInt(MasterToggle));
        PlayerPrefs.SetInt(Const.PlayerPrefs_MusicToggle, Util.BooleanToInt(MusicToggle));
        PlayerPrefs.SetInt(Const.PlayerPrefs_SFXToggle, Util.BooleanToInt(SFXToggle));
        PlayerPrefs.SetFloat(Const.PlayerPrefs_MasterVolume, MasterVolume);
        PlayerPrefs.SetFloat(Const.PlayerPrefs_MusicVolume, MusicVolume);
        PlayerPrefs.SetFloat(Const.PlayerPrefs_SFXVolume, SFXVolume);
    }

    public void ExecuteAllEvent()
    {
        OnChangeFullScreen?.Invoke();
        OnChangeResolution?.Invoke();

        OnChangeMasterToggle?.Invoke();
        OnChangeMusicToggle?.Invoke();
        OnChangeMasterVolume?.Invoke();
        OnChangeMusicVolume?.Invoke();
        OnChangeSFXVolume?.Invoke();
        OnChangeMasterVolume?.Invoke();
    }
}
