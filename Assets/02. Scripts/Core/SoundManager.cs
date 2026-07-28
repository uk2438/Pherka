using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Fade")]
    [SerializeField] private float defaultFadeDuration = 1f;

    private Coroutine bgmFadeCoroutine;

    private float originalBGMVolume = 1f;
    private bool isVolumeCached;
    private AudioClip previousClip;

    private void CacheBGMVolume()
    {
        if (isVolumeCached || bgmSource == null)
            return;

        originalBGMVolume = bgmSource.volume;
        isVolumeCached = true;
    }

    private void StopBGMFade()
    {
        if (bgmFadeCoroutine == null)
            return;

        StopCoroutine(bgmFadeCoroutine);
        bgmFadeCoroutine = null;
    }

    // 즉시 BGM 재생
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null)
            return;

        CacheBGMVolume();
        StopBGMFade();

        // 같은 BGM이 재생 중이라면 다시 시작하지 않음
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            bgmSource.volume = originalBGMVolume;
            return;
        }

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = originalBGMVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // 기본 시간으로 현재 BGM을 줄인 뒤 다음 BGM 재생
    public void FadeToBGM(AudioClip nextClip)
    {
        FadeToBGM(nextClip, defaultFadeDuration);
    }

    // 지정한 시간으로 현재 BGM을 줄인 뒤 다음 BGM 재생
    public void FadeToBGM(AudioClip nextClip, float duration)
    {
        if (nextClip == null || bgmSource == null)
            return;

        CacheBGMVolume();
        StopBGMFade();

        // 같은 BGM이면 전환하지 않고 볼륨만 복구
        if (bgmSource.clip == nextClip && bgmSource.isPlaying)
        {
            bgmSource.volume = originalBGMVolume;
            return;
        }

        if (duration <= 0f)
        {
            PlayBGM(nextClip);
            return;
        }

        bgmFadeCoroutine = StartCoroutine(
            FadeToBGMRoutine(nextClip, duration)
        );
    }

    private IEnumerator FadeToBGMRoutine(
        AudioClip nextClip,
        float duration
    )
    {
        // 현재 BGM이 재생 중일 때만 페이드아웃
        if (bgmSource.isPlaying)
        {
            float startVolume = bgmSource.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;

                float ratio = Mathf.Clamp01(
                    elapsed / duration
                );

                bgmSource.volume = Mathf.Lerp(
                    startVolume,
                    0f,
                    ratio
                );

                yield return null;
            }
        }

        bgmSource.Stop();
        bgmSource.clip = nextClip;
        bgmSource.volume = originalBGMVolume;
        bgmSource.loop = true;
        bgmSource.Play();

        bgmFadeCoroutine = null;
    }

    public void PlayCutSceneBGM(AudioClip cutSceneClip)
    {
        if(cutSceneClip == null || bgmSource == null) return;

        previousClip = bgmSource.clip;

        FadeToBGM(cutSceneClip);
        
    }

    public void RestorePreviousBGM()
    {
        RestorePreviousBGM(defaultFadeDuration);
    }

    public void RestorePreviousBGM(float fadeDuration)
    {
        if(bgmSource == null) return;
        if(previousClip == null) StopBGM();

        AudioClip restoreClip = previousClip;
        previousClip = null;

        FadeToBGM(restoreClip, fadeDuration);
        
    }

    // 기본 시간으로 서서히 정지
    public void StopBGM()
    {
        FadeOutBGM(defaultFadeDuration);
    }

    // 지정한 시간으로 서서히 정지
    public void FadeOutBGM(float duration)
    {
        if (bgmSource == null)
            return;

        CacheBGMVolume();
        StopBGMFade();

        if (!bgmSource.isPlaying || duration <= 0f)
        {
            StopBGMImmediate();
            return;
        }

        bgmFadeCoroutine = StartCoroutine(
            FadeOutBGMRoutine(duration)
        );
    }

    private IEnumerator FadeOutBGMRoutine(float duration)
    {
        float startVolume = bgmSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float ratio = Mathf.Clamp01(
                elapsed / duration
            );

            bgmSource.volume = Mathf.Lerp(
                startVolume,
                0f,
                ratio
            );

            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();
        bgmSource.clip = null;

        // 다음 BGM 재생을 위해 원래 볼륨으로 복구
        bgmSource.volume = originalBGMVolume;

        bgmFadeCoroutine = null;
    }

    // 즉시 정지
    public void StopBGMImmediate()
    {
        if (bgmSource == null)
            return;

        CacheBGMVolume();
        StopBGMFade();

        bgmSource.Stop();
        bgmSource.clip = null;
        bgmSource.volume = originalBGMVolume;
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        if (sfxSource == null)
            return;

        sfxSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        SetMixerVolume("BGM_Volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetMixerVolume("SFX_Volume", volume);
    }

    private void SetMixerVolume(string parameterName, float volume)
    {
        if (audioMixer == null)
            return;

        if (volume <= 0.0001f)
        {
            audioMixer.SetFloat(parameterName, -80f);
            return;
        }

        float decibel =
            Mathf.Log10(
                Mathf.Clamp(volume, 0.0001f, 1f)
            ) * 20f;

        audioMixer.SetFloat(parameterName, decibel);
    }
}