using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem;

public class DialogueManager : Singleton<DialogueManager>
{

    // 빠른 검색을 위한 딕셔너리 (Key: objId, Value: DialogueData 구조체)
    private Dictionary<int, DialogueData> dialogueDict = new Dictionary<int, DialogueData>();

    void Awake()
    {
        LoadDialogueData();
    }

    void LoadDialogueData()
    {
        dialogueDict.Clear();

        // 💡 인스펙터 에셋이 아닌, 스크립트 내부 static 데이터를 바로 가져옵니다.
        foreach (DialogueData d in DialogueStaticData.Dialogues)
        {
            if (!dialogueDict.ContainsKey(d.id))
            {
                dialogueDict.Add(d.id, d);
            }
            else
            {
                Debug.LogWarning($"중복된 Dialogue ID 발견: {d.id}");
            }
        }
    }

    public string GetTalk(ObjectData objdata, int talkIdx)
    {
        if (!dialogueDict.ContainsKey(objdata.id)) return null;

        DialogueData data = dialogueDict[objdata.id];

        if (data.sentences != null && talkIdx < data.sentences.Length)
        {
            return data.sentences[talkIdx];
        }

        return null; // 대사가 끝남
    }

    public Sprite GetPotrait(ObjectData objData, int talkIdx)
    {
        if (!dialogueDict.ContainsKey(objData.id)) return null;

        DialogueData data = dialogueDict[objData.id];
        int[] talkSequence = data.sequences;

        // 1. 초상화 시퀀스가 없으면 null 반환
        if (talkSequence == null || talkSequence.Length == 0)
        {
            return null;
        }

        // 2. 인덱스 범위 체크
        if (talkIdx >= talkSequence.Length)
        {
            return null;
        }

        // 3. 위 조건을 다 통과했을 때만 배열에 접근
        int spriteIdx = talkSequence[talkIdx];

        // 4. imgs 배열 범위 체크
        if (objData.imgs == null || spriteIdx >= objData.imgs.Length)
        {
            return null;
        }

        return objData.imgs[spriteIdx];
    }

    public string GetName(ObjectData objdata)
    {
        if (!dialogueDict.ContainsKey(objdata.id)) return "Unknown";
        return dialogueDict[objdata.id].name;
    }

    public int GetCurrentSequenceNum(ObjectData objData, int talkIdx)
    {
        if (!dialogueDict.ContainsKey(objData.id)) return -1;

        DialogueData data = dialogueDict[objData.id];
        if (data.sequences != null && talkIdx < data.sequences.Length)
        {
            return data.sequences[talkIdx];
        }
        return -1;
    }
}