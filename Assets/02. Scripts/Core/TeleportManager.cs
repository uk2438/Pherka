using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private PlayerFollower follower;

    [Header("사운드 설정")]
    public bool sound = false;

    [ShowIf("sound")]
    public List<AudioClip> soundClips;

    public TeleportData teleportData = new TeleportData();

    // 모든 문이 공유하는 텔레포트 잠금
    private static bool isTeleportLocked;

    private readonly List<Collider2D> overlappingColliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (isTeleportLocked) return;

        if (teleportData.targetRoom == null)
        {
            Debug.LogWarning("텔레포트 도착 위치가 연결되지 않았습니다.", this);
            return;
        }

        isTeleportLocked = true;
        StartCoroutine(TeleportSequence(other));
    }

    private IEnumerator TeleportSequence(Collider2D playerCollider)
    {
        // 코루틴이 종료될 때 잠금도 정리
        try
        {
            Transform playerTransform = playerCollider.transform;

            if (sound && soundClips != null && soundClips.Count > 0)
            {
                SoundManager.Instance.PlaySFX(soundClips[0]);
            }

            yield return StartCoroutine(FadeManager.Instance.FadeOut(1f));

            Vector3 destination =
                teleportData.targetRoom.transform.position - teleportData.offsetPosition;

            playerTransform.position = destination;

            if (follower == null)
            {
                follower = FindObjectOfType<PlayerFollower>(true);
            }

            if (follower != null && follower.isActiveAndEnabled)
            {
                follower.TeleportWithPlayer(destination);
            }

            Animator playerAnim = playerTransform.GetComponent<Animator>();
            PlayerController playerController = playerTransform.GetComponent<PlayerController>();

            bool isGrabbing = playerController != null && playerController.IsGrabbing;

            if (playerAnim != null && !isGrabbing)
            {
                if (teleportData.offsetPosition.x > 0)
                {
                    playerAnim.Play("PlayerLeftIdle");
                }
                else if (teleportData.offsetPosition.x < 0)
                {
                    playerAnim.Play("PlayerRightIdle");
                }
                else if (teleportData.offsetPosition.y > 0)
                {
                    playerAnim.Play("PlayerDownIdle");
                }
                else if (teleportData.offsetPosition.y < 0)
                {
                    playerAnim.Play("PlayerUpIdle");
                }
            }

            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(FadeManager.Instance.FadeIn(1f));

            // 이동한 위치가 물리 시스템에 반영될 때까지 대기
            yield return new WaitForFixedUpdate();

            // 문 Trigger에서 완전히 벗어날 때까지 잠금 유지
            while (playerCollider != null &&
                   playerCollider.enabled &&
                   playerCollider.gameObject.activeInHierarchy &&
                   IsOverlappingTeleportTrigger(playerCollider))
            {
                yield return new WaitForFixedUpdate();
            }
        }
        finally
        {
            isTeleportLocked = false;
        }
    }

    private bool IsOverlappingTeleportTrigger(Collider2D playerCollider)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true
        };

        overlappingColliders.Clear();
        playerCollider.OverlapCollider(filter, overlappingColliders);

        foreach (Collider2D col in overlappingColliders)
        {
            if (!col.isTrigger) continue;

            TeleportManager teleport = col.GetComponentInParent<TeleportManager>();

            if (teleport != null && teleport.isActiveAndEnabled)
            {
                return true;
            }
        }

        return false;
    }
}