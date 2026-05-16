using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialogueManager : Singleton<CutsceneDialogueManager>
{
    [System.Serializable]
    public class CutsceneDialogue
    {
        public int id;
        public string[] names;
        public string[] sentences;
    }

    [System.Serializable]
    public class CutsceneDialogueContainer
    {
        public List<CutsceneDialogue> dialogues;
    }
    //               triggerid, dialogue
    private Dictionary<int, CutsceneDialogue> dialogueDict = new Dictionary<int, CutsceneDialogue>();

    void Awake()
    {
        LoadDialogueData();
    }

    void LoadDialogueData()
    {
        // Resources/CutsceneDialogueData.json 파일을 읽어옴
        TextAsset targetJson = Resources.Load<TextAsset>("CutsceneDialogueData");

        // JSON 문자열을 객체로 변환
        CutsceneDialogueContainer container = JsonUtility.FromJson<CutsceneDialogueContainer>(targetJson.text);

        // 빠른 검색을 위해 Dictionary에 담기
        foreach (CutsceneDialogue d in container.dialogues)
        {
            dialogueDict.Add(d.id, d);
        }
    }

    public string GetTalk(int id, int talkIdx)
    {   //talkIdx 는 밖에서 증가
        if (!dialogueDict.ContainsKey(id)) return null;

        if (talkIdx < dialogueDict[id].sentences.Length)
        {
            return dialogueDict[id].sentences[talkIdx];
        }

        return null; // 대사가 끝남
    }

    public string GetName(int id, int talkIdx)
    {   //GetTalk 와 구조가 같음
        if(!dialogueDict.ContainsKey(id)) return null;

        if(talkIdx < dialogueDict[id].names.Length)
        {
            return dialogueDict[id].names[talkIdx];
        }

        return null;
    }

}
