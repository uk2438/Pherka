using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TeleportManager : MonoBehaviour
{
    [Header("사운드 설정")]
    public bool sound = false;
    
    [ShowIf("sound")]
    public List<AudioClip> soundClips;
    public TeleportData teleportData = new TeleportData();
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportSequence(other.transform));
        }
    }

    IEnumerator TeleportSequence(Transform playerTransform)
    {
        if(sound) SoundManager.Instance.PlaySFX(soundClips[0]);
        yield return StartCoroutine(FadeManager.Instance.FadeOut(1f));
        playerTransform.transform.position = teleportData.targetRoom.transform.position - teleportData.offsetPosition;

        Animator playerAnim = playerTransform.GetComponent<Animator>();
        if (playerAnim != null)
        {
            if(teleportData.offsetPosition.x > 0)
            {
                playerAnim.Play("PlayerLeftIdle");
            }
            else if(teleportData.offsetPosition.x < 0)
            {
                playerAnim.Play("PlayerRightIdle");
            }
             else if(teleportData.offsetPosition.y > 0)
            {
                playerAnim.Play("PlayerDownIdle");
            }
            else if(teleportData.offsetPosition.y < 0)
            {
                playerAnim.Play("PlayerUpIdle");
            }
        }

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeManager.Instance.FadeIn(1f));


    }
}
