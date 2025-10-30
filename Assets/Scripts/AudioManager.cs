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

        // ��ʼ����Ч�ֵ�
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in soundEffects)
        {
            soundDictionary[clip.name] = clip;
        }

        // ����BGM
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
            Debug.LogWarning("��Чδ�ҵ�: " + clipName);
        }
    }

    // ��ָ��λ�ò���3D��Ч
    public void Play3DSoundEffect(string clipName, Vector3 position)
    {
        if (soundDictionary.ContainsKey(clipName))
        {
            AudioSource.PlayClipAtPoint(soundDictionary[clipName], position);
        }
    }

    // ֹͣBGM
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // ����BGM����
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    // ������Ч����
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }
}