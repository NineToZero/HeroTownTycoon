using System.Collections.Generic;
using UnityEngine;

public enum MusicSource { LetTheAdventureBegin, LikeAnAnt }
public enum SFXSource { Pop, SceneChange, MeleeAttack, Craft, BattleHorn, Coin, Hit, Eating, Build, InteractionFarmland }

public class SoundManager : MonoBehaviour
{
    private OptionData _optionData;
    private AudioSource _musicAudio;
    private List<AudioSource> _sfxAudio;
    private const int SFXCHANNEL = 8;

    private void Awake()
    {
        _sfxAudio = new();
        CreateAudioSource(SFXCHANNEL);
        _optionData = Managers.Instance.OptionManager.OptionData;

        _optionData.OnChangeMasterToggle += RefreshMasterMute;
        _optionData.OnChangeMasterVolume += RefreshMusicVolume;
        _optionData.OnChangeMusicVolume += RefreshMusicVolume;
        _optionData.OnChangeMusicToggle += RefreshMusicMute;
        _optionData.OnChangeMasterVolume += RefreshSFXVolume;
        _optionData.OnChangeSFXVolume += RefreshSFXVolume;
        _optionData.OnChangeSFXToggle += RefreshSFXMute;
    }

    private void CreateAudioSource(int num)
    {
        var rootObj = new GameObject("Audio").transform;
        rootObj.SetParent(gameObject.transform);

        _musicAudio = new GameObject($"Music").AddComponent<AudioSource>();
        _musicAudio.transform.SetParent(rootObj);
        _musicAudio.loop = true;
        _musicAudio.playOnAwake = false;

        for (int i = 0; i < num; i++)
        {
            var obj = new GameObject($"Audio{i}").AddComponent<AudioSource>();
            obj.transform.SetParent(rootObj);
            obj.playOnAwake = false;
            _sfxAudio.Add(obj);
        }
    }

    public void PlaySFX(SFXSource source)
    {
        foreach (var audio in _sfxAudio)
        {
            if (audio.isPlaying == true)
                continue;

            audio.clip = Resources.Load<AudioClip>($"{Const.Sound_SFX}/{source}");
            audio.Play();
            return;
        }
    }

    public void PlayMusic(MusicSource source)
    {
        _musicAudio.clip = Resources.Load<AudioClip>($"{Const.Sound_Music}/{source}");
        _musicAudio.Play();
    }

    private void RefreshSFXVolume()
    {
        float volume = _optionData.MasterVolume * _optionData.SFXVolume;
        foreach (var audio in _sfxAudio)
        {
            audio.volume = volume;
        }
    }

    private void RefreshMusicVolume()
    {
        float volume = _optionData.MasterVolume * _optionData.MusicVolume;
        _musicAudio.volume = volume;
    }

    private void RefreshMasterMute()
    {
        bool isOn = _optionData.MasterToggle;

        if (isOn == false)
        {
            _musicAudio.mute = !isOn;
            foreach (var audio in _sfxAudio)
            {
                audio.mute = !isOn;
            }
        }
        else
        {
            _musicAudio.mute = !_optionData.MusicToggle;
            foreach (var audio in _sfxAudio)
            {
                audio.mute = !_optionData.SFXToggle;
            }
        }
    }

    private void RefreshMusicMute()
    {
        if (!_optionData.MasterToggle)
            return;

        bool isOn = _optionData.MusicToggle;
        _musicAudio.mute = !isOn;
    }

    private void RefreshSFXMute()
    {
        if (!_optionData.MasterToggle)
            return;

        bool isOn = _optionData.SFXToggle;
        foreach (var audio in _sfxAudio)
        {
            audio.mute = !isOn;
        }
    }

}
