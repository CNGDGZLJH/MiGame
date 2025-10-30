using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Settings")]
    public AudioClip bgmClip;
    public AudioSource bgmSource;

    [Header("Audio Effects Settings")]
    public AudioClip[] soundEffects;
    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> soundDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 初始化音效字典
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in soundEffects)
        {
            soundDictionary[clip.name] = clip;
        }

        // 播放BGM
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySoundEffect(string clipName)
    {
        if (soundDictionary.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(soundDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning("音效未找到: " + clipName);
        }
    }

    // 在指定位置播放3D音效
    public void Play3DSoundEffect(string clipName, Vector3 position)
    {
        if (soundDictionary.ContainsKey(clipName))
        {
            AudioSource.PlayClipAtPoint(soundDictionary[clipName], position);
        }
    }

    // 停止BGM
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // 设置BGM音量
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    // 设置音效音量
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }
}