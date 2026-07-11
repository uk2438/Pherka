using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiableObject : MonoBehaviour
{
    [Header("사운드 설정")]
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip activateSound;
    [SerializeField] private AudioClip deactivateSound;

    private AudioSource audioSource;
    protected virtual void Awake() {

        audioSource = GetComponent<AudioSource>();
        
    }


    protected void PlayInteractSound() => PlayClip(interactSound);
    protected void PlayactivateSound() => PlayClip(activateSound);
    protected void PlaydeactivateSound() => PlayClip(deactivateSound);

    private void PlayClip(AudioClip clip)
    {
        if(audioSource == null || clip == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public abstract void Activate();
    public abstract void Deactivate();
}
