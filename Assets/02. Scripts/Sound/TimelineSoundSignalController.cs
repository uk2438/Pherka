using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineSoundSignalController : MonoBehaviour
{
    [Header("Timeline에서 사용할 AudioSource")]
    [SerializeField] private AudioSource[] audioSources;

    public void Play(int index)
    {
        AudioSource source = GetSource(index);

        if(source == null || source.clip == null) return;

        source.Play();
        
    }

    public void PlayOneShot(int index)
    {
        AudioSource source = GetSource(index);

        if(source == null || source.clip == null) return;

        source.PlayOneShot(source.clip);
    }

    public void Stop(int index)
    {
        AudioSource source = GetSource(index);

        if(source == null || source.clip == null) return;

        source.Stop();        
    }

    public void Pause(int index)
    {
        AudioSource source = GetSource(index);

        if(source == null || source.clip == null) return;

        source.Pause();
    }

    public void Resume(int index)
    {
        AudioSource source = GetSource(index);

        if(source == null || source.clip == null) return;

        source.UnPause();
    }

    public void StopAll()
    {
        foreach(AudioSource source in audioSources)
        {
            if(source == null || source.clip == null) return;

            source.Stop();
        }
    }

    public AudioSource GetSource(int index)
    {
        if(audioSources == null || index < 0 || index >= audioSources.Length) return null;

        if(audioSources[index] == null) return null;

        return audioSources[index];
    }
}
