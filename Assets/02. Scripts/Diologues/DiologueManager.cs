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

    public DialogueLine? GetLine(ObjectData objectData, int lineIdx)
    {
        if(!dialogueDict.ContainsKey(objectData.id)) return null;

        DialogueData data = dialogueDict[objectData.id];

        if(data.lines == null || lineIdx < 0 || lineIdx >= data.lines.Length) 
            return null; //대화 종료

        return data.lines[lineIdx];

    }

    public Sprite GetPotrait(ObjectData objData, DialogueLine line)
    {
        if(objData.imgs == null || line.portraitIdx < 0 || line.portraitIdx >= objData.imgs.Length)
            return null;

        return objData.imgs[line.portraitIdx];
    }

    public string GetName(ObjectData objdata)
    {
        if (!dialogueDict.ContainsKey(objdata.id)) return "Unknown";
        return dialogueDict[objdata.id].name;
    }
}