using DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneDialogueTimelineController : MonoBehaviour
{
    [Header("Timeline")]
    [SerializeField] private PlayableDirector director;

    [Header("컷신 대화 데이터")]
    [SerializeField] private ObjectData dialogueObjectData;

    private DialogueLine currentLine;
    private bool isDialogueRunning;
    private bool isChoice;

    private void Awake()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }

        isDialogueRunning = false;
        isChoice = false;
    }

    private void Update()
    {
        if (!isDialogueRunning)
            return;

        if (isChoice)
            return;

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        // 글자가 출력 중이면 현재 문장을 즉시 완성
        if (TextAnim.Instance.isAnim)
        {
            TextAnim.Instance.SetText(currentLine.sentence);
            return;
        }

        // 출력이 끝났으면 다음 대사로 이동
        GoToNextLine(currentLine.nextLineIdx);
    }

    // Timeline Signal Receiver에서 호출
    public void StartDialogue(int startIndex)
    {
        if (director == null)
        {
            Debug.LogError("PlayableDirector가 연결되지 않았습니다.", this);
            return;
        }

        if (dialogueObjectData == null)
        {
            Debug.LogError("컷신 ObjectData가 연결되지 않았습니다.", this);
            return;
        }

        if (isDialogueRunning)
            return;

        isDialogueRunning = true;
        isChoice = false;

        GameManager.Instance.gameData.isAction = true;

        director.Pause();

        UIManager.Instance.SetDialogueBoxActive(true);

        ShowLine(startIndex);
    }

    private void ShowLine(int lineIndex)
    {
        DialogueLine? line =
            DialogueManager.Instance.GetLine(dialogueObjectData, lineIndex);

        if (!line.HasValue)
        {
            FinishDialogue();
            return;
        }

        currentLine = line.Value;

        string speakerName =
            DialogueManager.Instance.GetName(dialogueObjectData, lineIndex);

        UIManager.Instance.UpdateDialogueUI(
            dialogueObjectData,
            speakerName,
            currentLine
        );

        if (currentLine.hasChoices)
        {
            isChoice = true;

            UIManager.Instance.ShowChoices(
                currentLine,
                OnChoiceSelected
            );
        }
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

    private void OnChoiceSelected(
       int choiceIndex,
       int nextLineIndex
   )
    {
        UIManager.Instance.HideChoices();

        isChoice = false;

        GoToNextLine(nextLineIndex);
    }

    private void FinishDialogue()
    {
        if (!isDialogueRunning)
            return;

        isDialogueRunning = false;
        isChoice = false;

        GameManager.Instance.gameData.isAction = false;

        UIManager.Instance.HideChoices();
        UIManager.Instance.SetDialogueBoxActive(false);

        if (director != null)
        {
            director.Resume();
        }
    }

    // Timeline 종료용 Signal에서도 호출 가능
    public void FinishDialogueSignal()
    {
        GameManager.Instance.gameData.isRunningCutScene = false;
        FinishDialogue();
    }
}