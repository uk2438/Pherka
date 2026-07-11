using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [Header("오디오 믹서 설정")]
    [SerializeField] private AudioMixer audioMixer;

    // 설정 창의 볼륨 슬라이더(0.0001 ~ 1 사이의 값으로 세팅 추천)와 연결할 함수
    public void SetSFXVolume(float volume)
    {
        // 오디오 믹서는 데시벨(-80dB ~ 20dB)을 쓰기 때문에 로그 연산이 필요합니다.
        // volume이 0일 때 -80dB(무음), 1일 때 0dB(원래 소리)가 됩니다.
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0000f, 1f)) * 20f;
        
        // 아까 노출시킨 Exposed Parameter 이름인 "SFX_Volume"을 제어합니다.
        audioMixer.SetFloat("SFX_Volume", dB);
    }

    // 설정 창의 볼륨 슬라이더(0.0001 ~ 1 사이의 값으로 세팅 추천)와 연결할 함수
    public void SetBGMVolume(float volume)
    {
        // 오디오 믹서는 데시벨(-80dB ~ 20dB)을 쓰기 때문에 로그 연산이 필요합니다.
        // volume이 0일 때 -80dB(무음), 1일 때 0dB(원래 소리)가 됩니다.
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0000f, 1f)) * 20f;
        
        // 아까 노출시킨 Exposed Parameter 이름인 "SFX_Volume"을 제어합니다.
        audioMixer.SetFloat("BGM_Volume", dB);
    }    
}