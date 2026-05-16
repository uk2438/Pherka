using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public PlayableDirector director;
    bool isCutsceneRunning = false;
    bool isCutsceneDialogueRunning = false;
    int talkIdx = 0;
    public TextMeshProUGUI nameText;
    public Animator dialogueBox;
    public GameObject potrait;
    public int cutsceneId;
    public CinemachineBrain brain;
    public GameObject player;
    void Update()
    {
        if (isCutsceneDialogueRunning && Input.GetKeyDown(KeyCode.Space))
        {
            Action();
        }
    }


    public void StartCutScene()
    {
        if(brain != null)
        {
            brain.enabled = true;
        }
        director.Play();
        isCutsceneRunning = true;
    }

    public void TimeLinePause()
    {
        director.Pause();
        // 타임라인의 재생 속도를 0으로 강제 고정
        if (director.playableGraph.IsValid())
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        // timeline 을 pause할 때 화면에 깜빡거림을 없애기 위함
        // 이거와 같이 cinemachine brain inspector 에서 update method를 fixed update로 바꾸면 없어짐
        director.Evaluate();

        isCutsceneDialogueRunning = true;
        Action();
    }

    public void ResumeTimeline() // 대화가 끝날 때 호출될 부분
    {
        // 다시 재생할 때는 속도를 1로 복구
        if (director.playableGraph.IsValid())
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        director.Play();
    }

    public void EndCutScene()
    {
        if(brain != null)
        {
            brain.enabled = false;
        }
        isCutsceneRunning = false;
        InitCamera();
    }

    public void InitCamera()
    {
        Camera.main.transform.localPosition = new Vector3(0,0,-10);
        Camera.main.orthographicSize = 5f;
    }
    public void Action()
    {
        Talk(cutsceneId);
        dialogueBox.SetBool("isShow", isCutsceneDialogueRunning);
    }

    public void Talk(int id)
    {
        if (!isCutsceneDialogueRunning)
        {
            isCutsceneDialogueRunning = true;
        }
        string nameData = "";
        string talkData = "";
        if (TextAnim.Instance.isAnim)
        {   // 텍스트 애니메이션 도중에 Talk함수를 부른다면, 다음 talkData를 불러오는것을 방지하기 위해,
            // 빈 string을 부름
            TextAnim.Instance.SetText("");
            return;
        }
        talkData = CutsceneDialogueManager.Instance.GetTalk(id, talkIdx);
        nameData = CutsceneDialogueManager.Instance.GetName(id, talkIdx);

        if (nameData != null)
        {
            nameText.text = nameData;
        }
        // 대사 끝남
        if (talkData == null)
        {
            isCutsceneDialogueRunning = false;
            talkIdx = 0;
            ResumeTimeline();
            Debug.Log("Play 실행");
            return;
        }

        TextAnim.Instance.SetText(talkData);
        potrait.SetActive(false);
        talkIdx++;
    }

}
