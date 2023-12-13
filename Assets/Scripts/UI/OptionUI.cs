using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    [Header("Common")]
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _tutorialBtn;
    [SerializeField] private Button _creditBtn;
    [SerializeField] private Slider _mouseSensitivity;
    [SerializeField] private TMP_Text _mouseSensitivityText;

    [Header("Screen")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    [Header("Sound")]
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;

    [SerializeField] private Toggle _masterToggle;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _sfxToggle;

    [SerializeField] private TMP_Text _masterValueText;
    [SerializeField] private TMP_Text _musicValueText;
    [SerializeField] private TMP_Text _sfxValueText;

    [Header("Save")]
    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _turnOffBtn;
    [SerializeField] private TMP_Text _saveText;

    private OptionData _optionData;

    private void Awake()
    {
        _optionData = Managers.Instance.OptionManager.OptionData;
        List<Resolution> resolutions = _optionData.Resolutions;
        for (int i = 0; i < resolutions.Count; i++)
        {
            var option = new TMP_Dropdown.OptionData();
            option.text = $"{resolutions[i].width} x {resolutions[i].height} x {resolutions[i].refreshRateRatio.value:N2}hz";
            _resolutionDropdown.options.Add(option);
        }

        // Event
        _closeBtn.onClick.AddListener(() => { Managers.Instance.UIManager.CloseUI<OptionUI>(); });
        _creditBtn.onClick.AddListener(OpenCreditUI);

        _resolutionDropdown.onValueChanged.AddListener(OnChangeResolution);
        _fullscreenToggle.onValueChanged.AddListener(OnChangeFullScreen);
        _mouseSensitivity.onValueChanged.AddListener(OnChangeMouseSensitivity);

        _masterVolume.onValueChanged.AddListener(OnChangeMasterText);
        _musicVolume.onValueChanged.AddListener(OnChangeMusicText);
        _sfxVolume.onValueChanged.AddListener(OnChangeSFXText);

        _masterToggle.onValueChanged.AddListener(OnMasterToggle);
        _musicToggle.onValueChanged.AddListener(OnMusicToggle);
        _sfxToggle.onValueChanged.AddListener(OnSFXToggle);

        int currntScene = SceneManager.GetActiveScene().buildIndex;

        if (currntScene == (int)SceneType.Title)
        {
            _tutorialBtn.gameObject.SetActive(false);

            _saveText.text = "저장 초기화";
            _saveBtn.onClick.AddListener(ResetSaveData);
        }
        else
        {
            _tutorialBtn.gameObject.SetActive(true);
            _tutorialBtn.onClick.AddListener(() => { Managers.Instance.UIManager.OpenUI<KeyHelperUI>(); });

            _saveText.text = "타이틀로 돌아가기";
            _saveBtn.onClick.AddListener(QuitToMenu);
            _turnOffBtn.gameObject.SetActive(true);
            _turnOffBtn.onClick.AddListener(() =>
            {
                Managers.Instance.UIManager.ShowPopupUI
                (Util.TurnOffGame, "게임 종료", "정말 종료하시겠습니까?\n저장되지 않은 진행 사항은 사라집니다.", null, "종료", "취소");
            });
        }
    }

    public override void On()
    {
        base.On();
        ScreenRefresh();
        VolumeRefresh();
        MouseRefresh();

        Managers.Instance.SoundManager.PlaySFX(SFXSource.Pop);
    }

    public void Init(Action closeMethod = null)
    {
        _closeBtn.onClick.AddListener(() => { closeMethod(); });
    }

    public void ScreenRefresh()
    {
        _fullscreenToggle.isOn = Screen.fullScreen;
        _resolutionDropdown.value = _optionData.ResolutionIndex;
    }

    public void MouseRefresh()
    {
        float value = _optionData.MouseSensitivity;
        _mouseSensitivity.value = value;
        _mouseSensitivityText.text = $"{value:N1}";
    }

    public void VolumeRefresh()
    {
        float value;

        value = _optionData.MasterVolume;
        _masterVolume.value = value;
        OnChangeMasterText(value);
        _masterToggle.isOn = _optionData.MasterToggle;


        value = _optionData.MusicVolume;
        _musicVolume.value = value;
        OnChangeMusicText(value);
        _musicToggle.isOn = _optionData.MusicToggle;


        value = _optionData.SFXVolume;
        _sfxVolume.value = value;
        OnChangeSFXText(value);
        _sfxToggle.isOn = _optionData.SFXToggle;
    }

    private void OpenCreditUI()
    {
        Managers.Instance.UIManager.OpenUI<CreditUI>();
    }

    #region Event
    private void ResetSaveData()
    {
        Action action = () =>
        {
            ES3.DeleteFile("SaveFile.es3");
        };

        Managers.Instance.UIManager.ShowPopupUI(action, "경고", "저장된 모든 데이터를 삭제하시겠습니까?");
    }

    private void QuitToMenu()
    {
        Action quit = () => { SceneManager.LoadScene((int)SceneType.Title); };
        Managers.Instance.UIManager.ShowPopupUI(quit, "경고", "저장되지 않은 진행 사항은 사라집니다.");
    }

    private void OnChangeResolution(int num)
    {
        _optionData.ResolutionIndex = num;
    }

    private void OnChangeFullScreen(bool isFullScreen)
    {
        _optionData.IsFullScreen = isFullScreen;
    }

    private void OnChangeMasterText(float value)
    {
        _masterValueText.text = $"{(int)(value * 100f)}%";
        _optionData.MasterVolume = value;
    }

    private void OnChangeMusicText(float value)
    {
        _musicValueText.text = $"{(int)(value * 100f)}%";
        _optionData.MusicVolume = value;
    }

    private void OnChangeSFXText(float value)
    {
        _sfxValueText.text = $"{(int)(value * 100f)}%";
        _optionData.SFXVolume = value;
    }

    private void OnMasterToggle(bool isON)
    {
        _optionData.MasterToggle = isON;
    }

    private void OnMusicToggle(bool isON)
    {
        _optionData.MusicToggle = isON;
    }

    private void OnSFXToggle(bool isON)
    {
        _optionData.SFXToggle = isON;
    }

    private void OnChangeMouseSensitivity(float value)
    {
        _mouseSensitivityText.text = $"{value:N1}";
        _optionData.MouseSensitivity= value;
    }
    #endregion
}
