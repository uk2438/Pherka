using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [System.Serializable]
    public class Dialogue
    {
        public int id;
        public string name;
        public string[] sentences;
        public int[] sequences;
    }

    [System.Serializable]
    public class DialogueContainer
    {
        public List<Dialogue> dialogues;
    }
    //              objid, dialogue
    private Dictionary<int, Dialogue> dialogueDict = new Dictionary<int, Dialogue>();

    void Awake()
    {
        LoadDialogueData();
    }

    void LoadDialogueData()
    {
        // Resources/DialogueData.json 파일을 읽어옴
        TextAsset targetJson = Resources.Load<TextAsset>("DialogueData");

        // JSON 문자열을 객체로 변환
        DialogueContainer container = JsonUtility.FromJson<DialogueContainer>(targetJson.text);

        // 빠른 검색을 위해 Dictionary에 담기
        foreach (Dialogue d in container.dialogues)
        {
            dialogueDict.Add(d.id, d);
        }
    }

    public string GetTalk(ObjectData objdata, int talkIdx)
    {   //talkIdx 는 밖에서 증가
        if (!dialogueDict.ContainsKey(objdata.id)) return null;

        if (talkIdx < dialogueDict[objdata.id].sentences.Length)
        {
            return dialogueDict[objdata.id].sentences[talkIdx];
        }

        return null; // 대사가 끝남
    }

    public Sprite GetPotrait(ObjectData objData, int talkIdx)
    {
        if (!dialogueDict.ContainsKey(objData.id)) return null;

        int[] talkSequence;
        if(dialogueDict[objData.id].sequences == null)
        {
            talkSequence = null;   
        }
        else
        {
            talkSequence = dialogueDict[objData.id].sequences;
        }
        // 1. NPC가 아니면 즉시 null 반환 (초상화 필요 없음)
        if (talkSequence == null)
        {
            return null;
        }

        // 2. talkSequence 배열 자체가 비어있거나, 인덱스가 범위를 벗어났는지 확인
        if (talkSequence == null || talkIdx >= talkSequence.Length)
        {
            // 인덱스가 범위를 벗어나면 마지막 초상화나 기본 초상화를 반환하거나 null 반환
            return null;
        }

        // 3. 위 조건을 다 통과했을 때만 배열에 접근
        int spriteIdx = talkSequence[talkIdx];

        // 4. imgs 배열 범위도 체크
        if (objData.imgs == null || spriteIdx >= objData.imgs.Length)
        {
            return null;
        }

        return objData.imgs[spriteIdx];
    }

    public string GetName(ObjectData objdata)
    {
        return dialogueDict[objdata.id].name;
    }
    
    public int GetCurrentSequenceNum(ObjectData objData, int talkIdx)
    {
        return dialogueDict[objData.id].sequences[talkIdx];
    }
}
