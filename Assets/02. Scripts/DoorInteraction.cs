using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{

    [Header("오디오 클립 설정")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    Collider2D doorCollider;
    Animator doorAnim;
    AudioSource doorAudio;

    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        doorAnim = GetComponent<Animator>();
        doorAudio = GetComponent<AudioSource>();
    }
    public void OpenDoor()
    {
        if (doorAnim != null) doorAnim.SetTrigger("Open");

        if (doorAudio != null)
        {
            doorAudio.clip = openSound;
            doorAudio.Play();
        }
        GameManager.Instance.gameData.isDoorOpen = true;

        doorCollider.enabled = false;
    }

    public void CloseDoor()
    {
        if (doorAnim != null)
        {
            doorAnim.SetTrigger("Close");

        }
        if (doorAudio != null)
        {
            doorAudio.clip = closeSound;
            doorAudio.Play();
        }
        GameManager.Instance.gameData.isDoorOpen = false;
        doorCollider.enabled = true;
    }

}

