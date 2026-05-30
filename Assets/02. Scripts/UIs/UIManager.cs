using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public UIData UIData = new UIData();
    public PanelData panelData = new PanelData();

    // 대화창 애니메이션 제어
    public void SetDialogueBoxActive(bool isActive)
    {
        UIData.dialogueBox.SetBool("isShow", isActive);
    }

    // 이름 및 대화 UI 업데이트
    public void UpdateDialogueUI(ObjectData objData, string nameData, string talkData, int talkIdx)
    {
        if (nameData != null)
        {
            UIData.nameText.text = nameData;
        }

        bool isNpc = objData.isNpc;

        if (isNpc)
        {
            if (UIData.potraitObj != null)
            {
                UIData.potraitObj.SetActive(true);
            }
            
            UIData.potrait.sprite = DialogueManager.Instance.GetPotrait(objData, talkIdx);
            UIData.currPotrait = DialogueManager.Instance.GetCurrentSequenceNum(objData, talkIdx);
            UIData.potrait.color = new Color(1, 1, 1, 1);
            
            if (UIData.prevPotrait != UIData.currPotrait)
            {
                UIData.potraitAnim.SetTrigger("doMove");
                UIData.prevPotrait = UIData.currPotrait;
            }
        }
        else
        {
            UIData.potrait.color = new Color(1, 1, 1, 0);
        }

        // 텍스트 애니메이션 실행
        TextAnim.Instance.SetText(talkData);
    }
    // 일시정지 토글
    public void TogglePause()
    {
        panelData.isPause = !panelData.isPause;
        panelData.pausePanel.SetActive(panelData.isPause);
    }

    // 종료 패널 제어
    public void SetQuitPanelActive(bool isActive)
    {
        if (panelData.quitPanel != null)
        {
            panelData.quitPanel.SetActive(isActive);
        }
    }

    // 체크 패널(저장 등) 열기
    public void OpenCheckPanel()
    {
        panelData.checkPanel.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(panelData.firstButton);
        
        Input.ResetInputAxes();
    }

    // 체크 패널 닫기
    public void CloseCheckPanel()
    {
        panelData.checkPanel.SetActive(false);
        Input.ResetInputAxes();
    }
}