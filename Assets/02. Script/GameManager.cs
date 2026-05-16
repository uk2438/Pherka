using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI nameText;
    public GameObject scanObject;
    public Animator dialogueBox;
    public bool isAction = false;
    public Image potrait;
    public GameObject potraitObj;
    public Animator potraitAnim;

    public int talkIdx;
    public int prevPotrait;
    public int currPotrait;
    public GameObject pausePanel;
    public GameObject quitPanel;
    public GameObject checkPanel;
    public GameObject firstButton;
    public bool isPause = false;

    public void Action()
    {
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData);
        //대화창 애니메이션 (실전에선 안쓸듯?)
        dialogueBox.SetBool("isShow", isAction);
    }

    public void Talk(ObjectData objData)
    {
        string nameData = "";
        string talkData = "";
        // 1. 대사 데이터를 먼저 가져옵니다.
        if (TextAnim.Instance.isAnim)
        {   // 텍스트 애니메이션 도중에 Talk함수를 부른다면, 다음 talkData를 불러오는것을 방지하기 위해,
            // 빈 string을 부름
            TextAnim.Instance.SetText("");
            return;
        }
        talkData = DialogueManager.Instance.GetTalk(objData, talkIdx);
        nameData = DialogueManager.Instance.GetName(objData);

        if (nameData != null)
        {
            nameText.text = nameData;
        }
        // 2. 대사가 끝났는지(null인지) 먼저 확인합니다.
        if (talkData == null)
        {
            if (objData.id == 100)
            {   //저장 obj면 저장 UI 호출
                OpenCheckPanel();
                return;
            }
            else
            {
                isAction = false;
                talkIdx = 0;
                return;
            }
        }

        // 3. 대사가 존재할 때만 아래 로직을 실행합니다.
        bool isNpc = objData.isNpc;

        if (isNpc)
        {
            if (potraitObj != null)
            {
                potraitObj.SetActive(true);
            }
            // NPC일 때만 초상화 데이터를 가져오고 색상을 조절합니다.
            potrait.sprite = DialogueManager.Instance.GetPotrait(objData, talkIdx);
            currPotrait = DialogueManager.Instance.GetCurrentSequenceNum(objData, talkIdx);
            potrait.color = new Color(1, 1, 1, 1);
            if (prevPotrait != currPotrait)
            {
                // 초상화가 바뀔 때 애니메이션
                potraitAnim.SetTrigger("doMove");
                prevPotrait = currPotrait;
            }
        }
        else
        {
            // NPC가 아니면 초상화를 투명하게 만듭니다.
            potrait.color = new Color(1, 1, 1, 0);
        }
        // 텍스트 애니메이션
        TextAnim.Instance.SetText(talkData);
        isAction = true;
        talkIdx++;
    }


    // 메뉴 버튼 클릭 함수들
    // gameManager 보다 다른 스크립트 만들어서 관리하는게 나을듯
    public void TogglePause()
    {
        //isPause라는 변수 대신 pausePanel.activeSelf라는 변수도 사용가능
        if (!isPause)
        {
            isPause = true;
            pausePanel.SetActive(isPause);
        }
        else
        {
            isPause = false;
            pausePanel.SetActive(isPause);
        }
    }

    public void OpenQuit()
    {
        //이거 작동 안됨 고쳐야 함
        if (!quitPanel.activeSelf)
        {
            quitPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        if (quitPanel.activeSelf)
        {
            quitPanel.SetActive(false);
        }
    }

    public void OpenSave()
    {
        //SAVE PANEL OPEN
        Debug.Log("save panel open");
    }

    public void OpenCheckPanel()
    {
        checkPanel.SetActive(true);
        //키보드로 버튼 선택 가능하게 만드는 함수
        // inspector창에서 button - navigation을 automatic이여야만 사용가능
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        // 중요: 이번 프레임의 모든 입력을 리셋하여 
        // 다음 버튼 클릭 판정이 스페이스바에 의해 즉시 발생하는 것을 방지
        Input.ResetInputAxes();
    }
    public void CloseSave()
    {
        isAction = false;
        talkIdx = 0;
        dialogueBox.SetBool("isShow", isAction);
        checkPanel.SetActive(false);

        // 중요: 이번 프레임의 모든 입력을 리셋하여 
        // 다음 버튼 클릭 판정이 스페이스바에 의해 즉시 발생하는 것을 방지
        Input.ResetInputAxes();
    }
}
