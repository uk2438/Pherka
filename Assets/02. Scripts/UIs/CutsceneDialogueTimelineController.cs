using System.Collections;
using DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneDialogueTimelineController : MonoBehaviour
{
    [Header("Timeline")]
    [SerializeField] private PlayableDirector director;

    [Header("컷신 대화 데이터")]
    [SerializeField] private ObjectData dialogueObjectData;

    [Header("컷신 등장인물 데이터")]
    [SerializeField] private CharacterData characterData;

    private DialogueLine currentLine;
    private bool isDialogueRunning;
    private bool isChoice;
    private bool isProcessingLine;
    private bool isFinishingDialogue;

    private Coroutine lineCoroutine;

    // 실제 마지막으로 표시한 대사의 정보
    private int lastDisplayedDialogueId = -1;
    private int lastDisplayedLineIndex = -1;

    private void Awake()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }
    }

    private void Update()
    {
        if (!isDialogueRunning ||
            isChoice ||
            isProcessingLine ||
            isFinishingDialogue)
        {
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (TextAnim.Instance.isAnim)
        {
            TextAnim.Instance.SetText(currentLine.sentence);
            return;
        }

        GoToNextLine(currentLine.nextLineIdx);
    }

    public void StartDialogue(int startIndex)
    {
        if (isDialogueRunning || isFinishingDialogue)
            return;

        if (director == null)
        {
            Debug.LogError(
                "PlayableDirector가 연결되지 않았습니다.",
                this
            );
            return;
        }

        if (dialogueObjectData == null)
        {
            Debug.LogError(
                "컷신 대화용 ObjectData가 연결되지 않았습니다.",
                this
            );
            return;
        }

        isDialogueRunning = true;
        isChoice = false;
        isProcessingLine = false;

        lastDisplayedDialogueId = -1;
        lastDisplayedLineIndex = -1;

        GameManager.Instance.gameData.isAction = true;

        SetTimelineSpeed(0);
        UIManager.Instance.SetDialogueBoxActive(true);

        ShowLine(startIndex);
    }

    private void ShowLine(int lineIndex)
    {
        if (!isDialogueRunning ||
            isProcessingLine ||
            isFinishingDialogue)
        {
            return;
        }

        isProcessingLine = true;
        lineCoroutine = StartCoroutine(ProcessLine(lineIndex));
    }

    private IEnumerator ProcessLine(int lineIndex)
    {
        DialogueLine? line = DialogueManager.Instance.GetLine(
            dialogueObjectData,
            lineIndex
        );

        if (!line.HasValue)
        {
            isProcessingLine = false;
            FinishDialogue();
            yield break;
        }

        int dialogueId = dialogueObjectData.GetCurrentDialogueId();

        string speakerName = DialogueManager.Instance.GetName(
            dialogueObjectData,
            lineIndex
        );

        // 대사 출력 전에 이벤트 실행
        DialogueEventManager eventManager =
            DialogueEventManager.Instance;

        if (eventManager != null)
        {
            yield return eventManager.ExecuteEvent(
                dialogueId,
                lineIndex,
                DialogueEventTiming.BeforeLine
            );
        }
        else
        {
            Debug.LogWarning(
                "DialogueEventManager.Instance가 존재하지 않습니다.",
                this
            );
        }

        // 이벤트 도중 대화가 종료됐다면 출력하지 않음
        if (!isDialogueRunning || isFinishingDialogue)
        {
            isProcessingLine = false;
            lineCoroutine = null;
            yield break;
        }

        currentLine = line.Value;

        UIManager.Instance.UpdateCutSceneDialogueUI(
            speakerName,
            currentLine
        );

        lastDisplayedDialogueId = dialogueId;
        lastDisplayedLineIndex = lineIndex;

        isChoice = currentLine.hasChoices;

        if (isChoice)
        {
            UIManager.Instance.ShowChoices(
                currentLine,
                OnChoiceSelected
            );
        }

        isProcessingLine = false;
        lineCoroutine = null;
    }

    private void GoToNextLine(int nextLineIndex)
    {
        if (nextLineIndex < 0)
        {
            FinishDialogue();
            return;
        }

        ShowLine(nextLineIndex);
    }

    private void OnChoiceSelected(int choiceIndex, int nextLineIndex)
    {
        if (!isDialogueRunning || isProcessingLine || !isChoice)
            return;

        UIManager.Instance.HideChoices();
        isChoice = false;

        GoToNextLine(nextLineIndex);
    }

    private void FinishDialogue()
    {
        if (!isDialogueRunning || isFinishingDialogue)
            return;

        isDialogueRunning = false;
        isFinishingDialogue = true;
        isChoice = false;

        // 종료 후 진행 중이던 대사가 다시 표시되는 것을 방지
        if (lineCoroutine != null)
        {
            StopCoroutine(lineCoroutine);
            lineCoroutine = null;
        }

        isProcessingLine = false;

        int finishedDialogueId = lastDisplayedDialogueId;
        int finishedLineIndex = lastDisplayedLineIndex;

        lastDisplayedDialogueId = -1;
        lastDisplayedLineIndex = -1;

        GameManager.Instance.gameData.isAction = false;

        UIManager.Instance.HideChoices();
        UIManager.Instance.SetDialogueBoxActive(false);

        StartCoroutine(
            ProcessDialogueEnd(
                finishedDialogueId,
                finishedLineIndex
            )
        );
    }

    private IEnumerator ProcessDialogueEnd(int dialogueId, int lineIndex)
    {
        // 대화창을 닫은 다음 프레임에 종료 이벤트 실행
        yield return null;

        DialogueEventManager eventManager =
            DialogueEventManager.Instance;

        if (dialogueId >= 0 &&
            lineIndex >= 0 &&
            eventManager != null)
        {
            yield return eventManager.ExecuteEvent(
                dialogueId,
                lineIndex,
                DialogueEventTiming.EndDialogue
            );
        }

        isFinishingDialogue = false;

        // 종료 이벤트가 끝난 뒤 Timeline 재개
        SetTimelineSpeed(1);
    }

    public void FinishDialogueSignal()
    {
        GameManager.Instance.gameData.isRunningCutScene = false;
        FinishDialogue();
    }

    private void SetTimelineSpeed(double speed)
    {
        if (director == null)
            return;

        if (!director.playableGraph.IsValid())
            return;

        int rootCount = director.playableGraph.GetRootPlayableCount();

        for (int i = 0; i < rootCount; i++)
        {
            director.playableGraph
                .GetRootPlayable(i)
                .SetSpeed(speed);
        }
    }
}