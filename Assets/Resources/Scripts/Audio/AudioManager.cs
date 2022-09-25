using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    //オーディオリソースのルートオブジェクト
    public GameObject audioRootObject;
    public GameObject musicRootObject;
    public GameObject effectRootObject;
    //BGMを格納する辞書
    [SerializeField]
    public Dictionary<string, AudioSource> musicDic = new Dictionary<string, AudioSource>();
    //エフェクトを格納する辞書
    public Dictionary<string, AudioSource> effectDic = new Dictionary<string, AudioSource>();

    public bool isMusicMute = false;


    public override void Init()
    {
        //ルートオブジェクトを初期化する。
        if (!GameObject.Find("audioRootObject"))
        { audioRootObject = new GameObject("audioRootObject"); }
        //DontDestroyOnLoad(audioRootObject);
    }
    public void MusicPlay(string path,bool isLoop)
    {
        AudioSource audio = null;
        if(musicDic.ContainsKey(path))
        {
            audio = musicDic[path];
        }
        else
        {
            Debug.Log("bug");
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
    public void MusicPause(string path)
    {

    }
    public void MusicStop(string path)
    {
        AudioSource audio = null;
        if (!musicDic.ContainsKey(path))
        {
            return;
        }
        audio = musicDic[path];
        audio.Stop();
    }
    public void MusicMute(string path,bool isMute)
    {

    }
    public void MusicVolume(string path,float volume)
    {
        AudioSource audio = null;
        if(!musicDic.ContainsKey(path))
        {
            return;
        }
        audio = musicDic[path];
        audio.volume = volume;
    }

    public void MusicClear()
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

    public void EffectPlay(string path,bool isLoop)
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
    public void EffectPause(string path)
    {

    }
    public void EffectStop(string path)
    {

    }
    public void EffectMute(string path, bool isMute)
    {

    }
    public void EffectVolume(string path, int volume)
    {

    }
    public void EffectClear()
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
