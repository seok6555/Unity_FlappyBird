using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    public AudioSource bgmSource;
    public AudioMixer mixer;
    public AudioClip[] bgmList;

    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    /// <summary>
    /// �� �ε��� ���� �޼���. ���� �̸��� ���� �̸��� ���� ����� Ŭ���� ����.
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgmList.Length; i++)
        {
            if (arg0.name == bgmList[i].name)
            {
                BGMPlay(bgmList[i]);
            }
        }
    }

    /// <summary>
    /// ����� ��� �޼���.
    /// </summary>
    /// <param name="clip">����� Ŭ��.</param>
    public void BGMPlay(AudioClip clip)
    {
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGMSound")[0];
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.volume = 1f;
        bgmSource.Play();
    }

    /// <summary>
    /// ����� ���� ���� �޼���.
    /// </summary>
    /// <param name="value">������ ������ ��</param>
    public void BGMSoundVolume(float value)
    {
        mixer.SetFloat("BGMSoundVolume", Mathf.Log10(value) * 20);
    }

    /// <summary>
    /// ȿ���� ��� �޼���.
    /// ����� �ҽ��� ������ �ִ� ������Ʈ�� ���� ��, �ش� ������Ʈ���� ���带 ����ϰ� �ı��ϴ� ���.
    /// </summary>
    /// <param name="sfxName">������ ȿ���� �̸�.</param>
    /// <param name="_clip">����� Ŭ��.</param>
    public void SFXPlay(string sfxName, AudioClip _clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = _clip;
        audioSource.Play();

        Destroy(go, _clip.length);
    }

    /// <summary>
    /// ȿ���� ��� �޼���. �ٸ� ��ũ��Ʈ���� ���带 ����ϴ� ���.
    /// </summary>
    /// <param name="_audioSource">����� �ҽ�.</param>
    /// <param name="_clip">����� Ŭ��.</param>
    public void SFXPlay(AudioSource _audioSource, AudioClip _clip)
    {
        _audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        _audioSource.clip = _clip;
        _audioSource.Play();
    }

    /// <summary>
    /// ȿ���� ���� ���� �޼���.
    /// </summary>
    /// <param name="value">������ ������ ��</param>
    public void SFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}
