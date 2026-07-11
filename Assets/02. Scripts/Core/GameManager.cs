using System.Collections;
using DialogueSystem;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData gameData = new GameData();

    // UI 분리로 인해 내부적으로 관리할 대화 인덱스
    public int currentLineIdx = 0;

    public void Action()
    {
        ObjectData objData = gameData.scanObject.GetComponent<ObjectData>();
        Talk(objData);

        // UI 연출은 UIManager에게 위임
        UIManager.Instance.SetDialogueBoxActive(gameData.isAction);
    }

    public void Talk(ObjectData objData)
    {
        if (TextAnim.Instance.isAnim)
        {
            TextAnim.Instance.SetText("");
            return;
        }

        DialogueLine? lineNullable = DialogueManager.Instance.GetLine(objData, currentLineIdx);

        if (lineNullable == null)
        {
            gameData.isAction = false;
            currentLineIdx = 0;
            return;
        }

        DialogueLine line = lineNullable.Value;
        string nameData = DialogueManager.Instance.GetName(objData);

        gameData.isAction = true;

        // 대사는 선택지 유무와 상관없이 항상 먼저 표시
        UIManager.Instance.UpdateDialogueUI(objData, nameData, line);

        if (line.hasChoices)
        {
            // 선택지가 있으면 버튼 띄우고 자동 진행 멈춤
            UIManager.Instance.ShowChoices(line, (nextIdx) =>
            {
                currentLineIdx = nextIdx;
                UIManager.Instance.HideChoices();
                StartCoroutine(TalkNextFrame(objData));
            });
        }
        else
        {
            // 선택지 없으면 기존처럼 자동으로 다음 라인 인덱스 저장
            currentLineIdx = line.nextLineIdx;
        }
    }

    private IEnumerator TalkNextFrame(ObjectData objData)
    {
        yield return null;
        Talk(objData);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSave()
    {
        Debug.Log("save panel open");
    }

    // public void CloseSave()
    // {
    //     gameData.isAction = false;
    //     talkIdx = 0;

    //     UIManager.Instance.SetDialogueBoxActive(gameData.isAction);
    //     UIManager.Instance.CloseCheckPanel();
    // }
}