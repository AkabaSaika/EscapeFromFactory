using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public static GameObject audioRootObject;
    public static Dictionary<string, AudioSource> musicDic = new Dictionary<string, AudioSource>();
    public static Dictionary<string, AudioSource> effectDic = new Dictionary<string, AudioSource>();

    public static bool isMusicMute = false;

    private void Start()
    {
        audioRootObject = new GameObject("audioRootObject");
    }
    public static void MusicPlay(string path,bool isLoop)
    {
        AudioSource audio = null;
        if(musicDic.ContainsKey(path))
        {
            audio = musicDic[path];
        }
        else
        {
            GameObject music = new GameObject(path);
            music.transform.parent = audioRootObject.transform;
            AudioClip clip = Resources.Load<AudioClip>(path);
            audio = music.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.loop = isLoop;
            audio.playOnAwake = true;
            audio.spatialBlend = 0.0f;

            musicDic.Add(path, audio);
        }
        audio.mute = isMusicMute;
        audio.enabled = true;
        audio.Play();
    }
    public static void MusicPause(string path)
    {

    }
    public static void MusicStop(string path)
    {
        AudioSource audio = null;
        if (!musicDic.ContainsKey(path))
        {
            return;
        }
        audio = musicDic[path];
        audio.Stop();
    }
    public static void MusicMute(string path,bool isMute)
    {

    }
    public static void MusicVolume(string path,float volume)
    {
        AudioSource audio = null;
        if(!musicDic.ContainsKey(path))
        {
            return;
        }
        audio = musicDic[path];
        audio.volume = volume;
    }

    public static void MusicClear()
    {
        if(musicDic.Count==0)
        {
            return;
        }
        else
        {
            musicDic.Clear();
        }
    }

    public static void EffectPlay(string path,bool isLoop)
    {
        AudioSource audio = null;
        if (effectDic.ContainsKey(path))
        {
            audio = effectDic[path];
        }
        else
        {
            GameObject effect = new GameObject(path);
            effect.transform.parent = audioRootObject.transform;
            AudioClip clip = Resources.Load<AudioClip>(path);
            audio = effect.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.loop = isLoop;
            audio.playOnAwake = true;
            audio.spatialBlend = 0.0f;

            effectDic.Add(path, audio);
        }
        audio.mute = isMusicMute;
        audio.enabled = true;
        audio.Play();
    }
    public static void EffectPause(string path)
    {

    }
    public static void EffectStop(string path)
    {

    }
    public static void EffectMute(string path, bool isMute)
    {

    }
    public static void EffectVolume(string path, int volume)
    {

    }
    public static void EffectClear()
    {
        if(effectDic.Count==0)
        {
            return;
        }
        else
        {
            effectDic.Clear();
        }
    }
}
