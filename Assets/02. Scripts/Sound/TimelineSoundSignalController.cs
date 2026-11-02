using UnityEngine;

public class TimelineSoundSignalController : MonoBehaviour
{
    [Header("BGM 믹서로 출력하는 AudioSource")]
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX 믹서로 출력하는 AudioSource")]
    [SerializeField] private AudioSource sfxSource;

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null)
            return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void StopBGM()
    {
        if (bgmSource == null)
            return;

        bgmSource.Stop();
    }

    public void StopSFX()
    {
        if (sfxSource == null)
            return;

        sfxSource.Stop();
    }

    public void PauseBGM()
    {
        if (bgmSource == null)
            return;

        bgmSource.Pause();
    }

    public void PauseSFX()
    {
        if (sfxSource == null)
            return;

        sfxSource.Pause();
    }

    public void ResumeBGM()
    {
        if (bgmSource == null)
            return;

        bgmSource.UnPause();
    }

    public void ResumeSFX()
    {
        if (sfxSource == null)
            return;

        sfxSource.UnPause();
    }

    public void StopAll()
    {
        StopBGM();
        StopSFX();
    }
}