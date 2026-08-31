using System.Collections;
using DialogueSystem;
using UnityEngine;

public class DialogueEventManager :
    Singleton<DialogueEventManager>
{
    [Header("Player Object")]
    [SerializeField] private GameObject player;
    private bool isRunningEvent;

    public bool IsRunningEvent
    {
        get { return isRunningEvent; }
    }

    public IEnumerator ExecuteEvent(int dialogueId, int lineIndex, DialogueEventTiming timing)
    {
        DialogueEventData[] events =
            DialogueEventStaticData.Events;

        foreach (DialogueEventData eventData in events)
        {
            if (eventData.dialogueId != dialogueId ||
                eventData.lineIndex != lineIndex ||
                eventData.timing != timing)
            {
                continue;
            }

            yield return StartCoroutine(
                RunEvent(eventData)
            );
        }
    }

    private IEnumerator RunEvent(DialogueEventData eventData)
    {
        isRunningEvent = true;

        float duration =eventData.duration;
        
        switch (eventData.eventType)
        {
            case DialogueEventType.FadeOut:
                yield return StartCoroutine(
                    FadeManager.Instance.FadeOut(duration)
                );
                break;

            case DialogueEventType.FadeIn:
                yield return StartCoroutine(
                    FadeManager.Instance.FadeIn(duration)
                );
                break;

            case DialogueEventType.FadeOutIn:
                yield return StartCoroutine(
                    FadeManager.Instance.FadeOut(duration)
                );

                yield return StartCoroutine(
                    FadeManager.Instance.FadeIn(duration)
                );
                break;
            case DialogueEventType.SetMartActive:
                yield return StartCoroutine(PrologueManager.Instance.SetMartNPCActive(true));
                break;
            case DialogueEventType.ShowGuide0:
                yield return StartCoroutine(GuideManager.Instance.ShowGuide(0));
                break;
            case DialogueEventType.ShowGuide1:
                yield return StartCoroutine(GuideManager.Instance.ShowGuide(1));
                break;
            case DialogueEventType.TeleportPlayer:
                yield return StartCoroutine(TeleportPlayer(eventData.teleportTarget, duration));
                break;
        }

        isRunningEvent = false;
    }

    private IEnumerator TeleportPlayer(DialogueTeleportTarget teleportTarget, float duration)
    {
        if (player == null)
        {
            Debug.LogError(
                "DialogueEventManager의 Player가 지정되지 않았습니다."
            );

            yield break;
        }

        if (FadeManager.Instance == null)
        {
            Debug.LogError(
                "FadeManager.Instance가 존재하지 않습니다."
            );

            yield break;
        }

        Vector3 targetPosition;

        switch (teleportTarget)
        {
            case DialogueTeleportTarget.FirstGoToWork:
                if (PrologueManager.Instance == null)
                {
                    yield break;
                }

                targetPosition =
                    PrologueManager.Instance.GetFirstGoToWork();
                break;
            case DialogueTeleportTarget.SecondGoToWork:
                if (PrologueManager.Instance == null)
                {
                    yield break;
                }

                targetPosition =
                    PrologueManager.Instance.GetSecondGoToWork();
                    
                break;

            case DialogueTeleportTarget.GoToHome:
                if (PrologueManager.Instance == null)
                {
                    yield break;
                }

                targetPosition =
                    PrologueManager.Instance.GetGoToHome();

                break;

            default:
                Debug.LogWarning(
                    "순간이동 목적지가 지정되지 않았습니다."
                );

                yield break;
        }

        FadeManager.Instance.fadeData.isFading = true;

        yield return StartCoroutine(
            FadeManager.Instance.FadeOut(
                duration
            )
        );

        SetPlayerPosition(
            targetPosition
        );

        yield return StartCoroutine(
            FadeManager.Instance.FadeIn(
                duration
            )
        );

        FadeManager.Instance.fadeData.isFading = false;
    }

    private void SetPlayerPosition(Vector3 position)
    {
        if (player == null)
            return;

        Rigidbody2D playerRb =
            player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.position = position;
            playerRb.velocity = Vector2.zero;
        }
        else
        {
            player.transform.position = position;
        }

        Physics2D.SyncTransforms();
    }
}