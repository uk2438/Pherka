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


    protected void PlayInteractSound() => SoundManager.Instance.PlaySFX(interactSound);
    protected void PlayactivateSound() => SoundManager.Instance.PlaySFX(activateSound);
    protected void PlaydeactivateSound() =>SoundManager.Instance.PlaySFX(deactivateSound);

    public abstract void Activate();
    public abstract void Deactivate();
}
