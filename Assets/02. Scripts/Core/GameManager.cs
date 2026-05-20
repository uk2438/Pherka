using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData gameData = new GameData();
    
    // UI 분리로 인해 내부적으로 관리할 대화 인덱스
    public int talkIdx = 0; 

    public void Action()
    {
        ObjectData objData = gameData.scanObject.GetComponent<ObjectData>();
        Talk(objData);
        
        // UI 연출은 UIManager에게 위임
        UIManager.Instance.SetDialogueBoxActive(gameData.isAction);
    }

    public void Talk(ObjectData objData)
    {
        // 디버그 로그
        Debug.Log($"[디버그 완료] objData 상태: {objData != null}");
        Debug.Log($"[디버그 완료] DialogueManager 인스턴스 상태: {DialogueManager.Instance != null}");
        
        if (TextAnim.Instance.isAnim)
        {   
            TextAnim.Instance.SetText("");
            return;
        }

        string talkData = DialogueManager.Instance.GetTalk(objData, talkIdx);
        string nameData = DialogueManager.Instance.GetName(objData);

        // 대사가 끝났는지(null인지) 확인
        if (talkData == null)
        {
            if (objData.id == 100)
            {   
                // 저장 오브젝트면 UI 매니저에게 패널 오픈 요청
                UIManager.Instance.OpenCheckPanel();
                return;
            }
            else
            {
                gameData.isAction = false;
                talkIdx = 0;
                return;
            }
        }

        // 대사가 존재하므로 UI 업데이트 위임
        gameData.isAction = true;
        UIManager.Instance.UpdateDialogueUI(objData, nameData, talkData, talkIdx);
        
        talkIdx++;
    }

    // 버튼 기능들은 이제 흐름 제어만 하거나 시스템 명령만 수행
    public void TogglePause()
    {
        UIManager.Instance.TogglePause();
    }

    public void OpenQuit()
    {
        UIManager.Instance.SetQuitPanelActive(true);
    }

    public void CancelQuit()
    {
        UIManager.Instance.SetQuitPanelActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSave()
    {
        Debug.Log("save panel open");
    }

    public void CloseSave()
    {
        gameData.isAction = false;
        talkIdx = 0;
        
        UIManager.Instance.SetDialogueBoxActive(gameData.isAction);
        UIManager.Instance.CloseCheckPanel();
    }
}