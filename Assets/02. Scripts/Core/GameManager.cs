using System.Collections;
using DialogueSystem;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData gameData = new GameData();

    // 현재 대화 묶음 안에서 진행 중인 줄 인덱스
    public int currentLineIdx = 0;

    // Raycast에서 오브젝트가 사라져도 대화를 유지하기 위한 필드
    private ObjectData currentDialogueObject;

    // 대화 이벤트 또는 대사 출력 준비가 진행 중인지 확인
    private bool isProcessingDialogueLine;

    //monologue params
    private bool isMonologue;
    private int currentMonologueId;
    private int currentMonologueLineIdx;
    private System.Action onMonologueFinished, onDialogueFinished;


    public void TriggerAction()
    {
        if (isProcessingDialogueLine ||
            FadeManager.Instance.fadeData.isFading)
        {
            return;
        }

        if (gameData.triggerObjectData == null)
            return;

        gameData.isTrigger = true;

        currentDialogueObject =
            gameData.triggerObjectData;

        Talk(currentDialogueObject);
    }

    public void Action()
    {
        if (FadeManager.Instance.fadeData.isFading ||
            isProcessingDialogueLine)
        {
            return;
        }
        if (isMonologue)
        {
            if (TextAnim.Instance.isAnim)
            {
                TextAnim.Instance.SetText("");
                return;
            }

            NextMonologueLine();
            return;
        }

        // 이미 대화 중이면 Raycast 결과와 관계없이 기존 대화 진행
        if (gameData.isAction &&
            currentDialogueObject != null)
        {
            Talk(currentDialogueObject);
            return;
        }

        if (gameData.scanObject == null)
        {

            return;
        }

        ObjectData objData =
            gameData.scanObject.GetComponent<ObjectData>();

        if (objData == null)
        {
            Debug.LogWarning(
                "상호작용 오브젝트에 ObjectData가 없습니다."
            );

            return;
        }

        currentDialogueObject = objData;

        Talk(currentDialogueObject);
    }

    public void Talk(ObjectData objData)
    {
        if (isProcessingDialogueLine)
            return;

        if (objData == null)
        {
            EndDialogue(null);
            return;
        }

        // 타이핑 애니메이션 진행 중이면 문장 전체 출력
        if (TextAnim.Instance.isAnim)
        {
            TextAnim.Instance.SetText("");
            return;
        }

        int lineIndex =
            currentLineIdx;

        DialogueLine? lineNullable =
            DialogueManager.Instance.GetLine(
                objData,
                lineIndex
            );

        if (!lineNullable.HasValue)
        {
            EndDialogue(objData);
            return;
        }

        DialogueLine line =
            lineNullable.Value;

        string nameData =
            DialogueManager.Instance.GetName(
                objData,
                lineIndex
            );

        StartCoroutine(
            ProcessDialogueLine(
                objData,
                line,
                nameData,
                lineIndex
            )
        );
    }

    private IEnumerator ProcessDialogueLine(ObjectData objData, DialogueLine line, string nameData, int lineIndex)
    {
        isProcessingDialogueLine = true;

        int dialogueId =
            objData.GetCurrentDialogueId();

        // 현재 대사를 출력하기 전에 등록된 이벤트 실행
        if (DialogueEventManager.Instance != null)
        {
            yield return StartCoroutine(
                DialogueEventManager.Instance.ExecuteEvent(
                    dialogueId,
                    lineIndex,
                    DialogueEventTiming.BeforeLine
                )
            );
        }
        else
        {
            Debug.LogWarning(
                "DialogueEventManager.Instance가 존재하지 않습니다."
            );
        }

        // 이벤트 실행 도중 대화가 종료되었는지 확인
        if (currentDialogueObject == null)
        {
            isProcessingDialogueLine = false;
            yield break;
        }

        gameData.isAction = true;

        UIManager.Instance.SetDialogueBoxActive(true);

        UIManager.Instance.UpdateDialogueUI(
            objData,
            nameData,
            line
        );

        if (line.hasChoices)
        {
            ShowDialogueChoices(
                objData,
                line
            );
        }
        else
        {
            currentLineIdx =
                line.nextLineIdx;
        }

        isProcessingDialogueLine = false;
    }


    private void ShowDialogueChoices(ObjectData objData, DialogueLine line)
    {
        UIManager.Instance.ShowChoices(
            line,
            (choiceIndex, nextIdx) =>
            {
                UIManager.Instance.HideChoices();

                int dialogueId =
                    objData.GetCurrentDialogueId();

                // Dialogue ID 0: 저장 책상
                if (dialogueId == 0 &&
                    choiceIndex == 0)
                {
                    SaveLoadManager.Instance.SaveGame();
                }

                /*
                 * 순간이동 특수 처리는 제거했습니다.
                 * 모든 선택지는 nextIdx로 동일하게 이동합니다.
                 */
                currentLineIdx = nextIdx;

                StartCoroutine(
                    TalkNextFrame(objData)
                );
            }
        );
    }

    private IEnumerator TalkNextFrame(ObjectData objData)
    {
        yield return null;

        if (objData == null)
            yield break;

        Talk(objData);
    }

    private void EndDialogue(ObjectData objData)
    {
        if (!gameData.isTrigger && objData != null)
        {
            objData.AdvanceDialogue();
        }

        StopAllCoroutines();

        isProcessingDialogueLine = false;

        gameData.isAction = false;
        gameData.isTrigger = false;

        gameData.triggerObjectData = null;
        gameData.scanObject = null;

        currentDialogueObject = null;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.fadeData.isFading = false;
        }

        currentLineIdx = 0;

        UIManager.Instance.HideChoices();
        UIManager.Instance.SetDialogueBoxActive(false);

        System.Action callback = onDialogueFinished;
        onDialogueFinished = null;

        if (callback != null)
        {
            StartCoroutine(InvokeDialogueCallbackNextFrame(callback));
        }
    }
    private IEnumerator InvokeDialogueCallbackNextFrame(System.Action callback)
    {
        yield return null;
        callback?.Invoke();
    }

    private void ShowMonologueLine()
    {
        if (isProcessingDialogueLine)
            return;

        StartCoroutine(ProcessMonologueLine());
    }

    private IEnumerator ProcessMonologueLine()
    {
        isProcessingDialogueLine = true;

        int lineIndex = currentMonologueLineIdx;

        DialogueLine? line = DialogueManager.Instance.GetLine(
            currentMonologueId,
            lineIndex
        );

        if (!line.HasValue)
        {
            isProcessingDialogueLine = false;
            FinishMonologue();
            yield break;
        }

        if (DialogueEventManager.Instance != null)
        {
            yield return StartCoroutine(
                DialogueEventManager.Instance.ExecuteEvent(
                    currentMonologueId,
                    lineIndex,
                    DialogueEventTiming.BeforeLine
                )
            );
        }

        // 이벤트 실행 도중 독백이 종료됐는지 확인
        if (!isMonologue)
        {
            isProcessingDialogueLine = false;
            yield break;
        }

        DialogueLine currentLine = line.Value;

        string nameData = currentLine.defaultname;

        UIManager.Instance.SetDialogueBoxActive(true);
        UIManager.Instance.UpdateMonologueUI(
            nameData,
            currentLine
        );

        currentMonologueLineIdx = currentLine.nextLineIdx;
        isProcessingDialogueLine = false;
    }

    public void StartMonologue(int dialogueId, System.Action onFinished)
    {
        if (isMonologue)
            return;

        isMonologue = true;
        onMonologueFinished = onFinished;
        currentMonologueId = dialogueId;
        currentMonologueLineIdx = 0;

        gameData.isAction = true;

        UIManager.Instance.SetDialogueBoxActive(true);
        ShowMonologueLine();
    }
    public void StartMonologue(int dialogueId)
    {
        StartMonologue(dialogueId, null);
    }
    public void NextMonologueLine()
    {
        if (!isMonologue)
            return;

        ShowMonologueLine();
    }

    private void FinishMonologue()
    {
        isMonologue = false;
        isProcessingDialogueLine = false;

        currentMonologueId = -1;
        currentMonologueLineIdx = 0;

        gameData.isAction = false;
        gameData.isTrigger = false;

        UIManager.Instance.HideChoices();
        UIManager.Instance.SetDialogueBoxActive(false);

        System.Action callback = onMonologueFinished;
        onMonologueFinished = null;

        callback?.Invoke();
    }
    public void SetDialogueFinishedCallback(System.Action callback)
    {
        onDialogueFinished = callback;
    }


    public void Quit()
    {
        Application.Quit();
    }
}