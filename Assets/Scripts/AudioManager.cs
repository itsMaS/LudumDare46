using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    struct AudioEvent
    {
        public AudioSource source;
        public Action onComplete;
        public AudioEvent(AudioSource source, Action onComplete)
        {
            this.source = source;
            this.onComplete = onComplete;
        }
    }

    static AudioManager instance;
    static List<AudioSource> Sources = new List<AudioSource>();
    static List<AudioEvent> TrackedSources = new List<AudioEvent>();

    public static AudioSource PlayWithPitchDeviation(string clipName, float volume = 1, bool looping = false, float pitchDeviation = 0, GameObject source = null, Action onComplete = null)
    {
        AudioClip clip = GetAudioClip(clipName);
        return PlayWithPitchDeviation(clip, volume, looping, pitchDeviation, source, onComplete);
    }

    public static AudioSource PlayWithPitchDeviation(AudioClip clip, float volume = 1, bool looping = false, float pitchDeviation = 0,GameObject source = null, Action onComplete = null)
    {
        AudioSource audio = Play(clip, volume, looping, source, onComplete);
        System.Random rand = new System.Random();
        audio.pitch += UnityEngine.Random.Range(-pitchDeviation,pitchDeviation);
        return audio;
    }

    public static AudioSource Play(string clipName, float volume = 1, bool looping = false, GameObject source = null, Action onComplete = null)
    {
        AudioClip clip = GetAudioClip(clipName);
        return Play(clip,volume, looping, source,onComplete);
    }

    public static AudioSource Play(AudioClip clip, float volume = 1, bool looping = false, GameObject source = null , Action onComplete = null)
    {
        if(clip == null)
        {
            Debug.LogError("Audio clip not found!");
            return null;
        }

        if (!instance)
        {
            CreateObject();
        }

        AudioSource audio; 
        if(source)
        {
            audio = source.AddComponent<AudioSource>();
            audio.spatialBlend = 1;
        }
        else
        {
            audio = instance.gameObject.AddComponent<AudioSource>();
            audio.spatialBlend = 0;
        }
        audio.clip = clip;
        audio.volume = volume;
        audio.loop = looping;
        audio.spatialize = true;
        audio.Play();
        if(onComplete != null)
        {
            TrackedSources.Add(new AudioEvent(audio, onComplete));
        }
        else
        {
            Sources.Add(audio);
        }
        return audio;
    }

    public static AudioClip GetAudioClip(string filePath)
    {
        return Resources.Load<AudioClip>("Audio/" + filePath);
    }

    static void CreateObject()
    {
        GameObject AudioManager = new GameObject("Audio Manager");
        instance = AudioManager.AddComponent<AudioManager>();
        
        DontDestroyOnLoad(AudioManager);

    }

    private void Start()
    {
        StartCoroutine(ClearNotPlaying());
    }

    private void Update()
    {
        TrackedSources.RemoveAll(x => x.source == null);
        foreach (var item in TrackedSources)
        {
            if(!item.source.isPlaying)
            {
                item.onComplete();
                Destroy(item.source);
            }
        }
        TrackedSources.RemoveAll(x => !x.source.isPlaying);
    }

    IEnumerator ClearNotPlaying()
    {
        while(instance)
        {
            Sources.RemoveAll(x => x == null);
            foreach (var item in Sources)
            {
                if(item && !item.isPlaying)
                {
                    Destroy(item);
                }
            }
            Sources.RemoveAll(x => !x.isPlaying);
            yield return new WaitForSeconds(5);
        }
    }

    public static void FadeOut(AudioSource source, float time)
    {
        instance.StartFadeOutCor(source, time);
    }
    void StartFadeOutCor(AudioSource source, float time)
    {
        StartCoroutine(FadeOut(time, source));
    }
    IEnumerator FadeOut(float time, AudioSource source)
    {
        float startVolume = source.volume;
        float elapsed = 0;
        while(source && source.volume > 0)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume,0,Mathf.InverseLerp(0, time, elapsed));
            yield return null;
        }
    }
}
