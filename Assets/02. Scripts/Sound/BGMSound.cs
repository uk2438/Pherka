using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSound : MonoBehaviour
{
    [Header("배경음악 리스트")]
    
    [SerializeField] public List<AudioClip> BGMList = new List<AudioClip>();
    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        PlayBGM(0);
    }

    void PlayBGM(int idx) {
        if(BGMList[idx] == null) return;
        
        audioSource.clip = BGMList[idx];
        audioSource.loop = true;
        audioSource.Play();
    }

    
    void StopBGM() {
        audioSource.loop = true;
        audioSource.Stop();
    }
}
