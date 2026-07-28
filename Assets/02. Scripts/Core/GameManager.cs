using System.Collections;
using DialogueSystem;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData gameData = new GameData();

    // 현재 진행 중인 대사 인덱스
    public int currentLineIdx = 0;


    public void TriggerAction()
    {
        if(gameData.triggerObjectData == null) return;

        gameData.isTrigger = true;

        Talk(gameData.triggerObjectData);

        UIManager.Instance.SetDialogueBoxActive(gameData.isTrigger);
        
    }
    public void Action()
    {
        if (gameData.scanObject == null)
        {
            Debug.LogWarning("상호작용할 오브젝트가 없습니다.");
            return;
        }

        ObjectData objData =
            gameData.scanObject.GetComponent<ObjectData>();

        if (objData == null)
        {
            Debug.LogWarning("상호작용 오브젝트에 ObjectData가 없습니다.");
            return;
        }

        Talk(objData);

        UIManager.Instance.SetDialogueBoxActive(
            gameData.isAction
        );
    }

    public void Talk(ObjectData objData)
    {
        if (TextAnim.Instance.isAnim)
        {
            TextAnim.Instance.SetText("");
            return;
        }

        DialogueLine? lineNullable =
            DialogueManager.Instance.GetLine(
                objData,
                currentLineIdx
            );

        // 더 이상 출력할 대사가 없으면 대화 종료
        if (lineNullable == null)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lineNullable.Value;

        string nameData =
            DialogueManager.Instance.GetName(objData, currentLineIdx);

        gameData.isAction = true;

        // 현재 대사 표시
        UIManager.Instance.UpdateDialogueUI(objData, nameData, line);

        if (line.hasChoices)
        {
            UIManager.Instance.ShowChoices(
                line,
                (choiceIndex, nextIdx) =>
                {
                    UIManager.Instance.HideChoices();

                    // ID가 1인 저장 책상에서
                    // 첫 번째 선택지인 "예"를 눌렀을 때만 저장
                    if (objData.id == 0 &&choiceIndex == 0)
                    {
                        SaveLoadManager.Instance.SaveGame();
                    }

                    currentLineIdx = nextIdx;

                    StartCoroutine(TalkNextFrame(objData));
                }
            );
        }
        else
        {
            // 일반 대사는 다음 인덱스 저장
            currentLineIdx = line.nextLineIdx;
        }
    }

    private IEnumerator TalkNextFrame(ObjectData objData)
    {
        yield return null;

        Talk(objData);

        UIManager.Instance.SetDialogueBoxActive(gameData.isAction);
    }

    private void EndDialogue()
    {
        gameData.isAction = false;
        gameData.isTrigger = false;
        gameData.triggerObjectData = null;
        currentLineIdx = 0;

        UIManager.Instance.HideChoices();
        UIManager.Instance.SetDialogueBoxActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}