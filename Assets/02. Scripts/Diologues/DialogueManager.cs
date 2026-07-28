using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueManager : Singleton<DialogueManager>
{
    // Key: 오브젝트 ID, Value: 대화 데이터
    private readonly Dictionary<int, DialogueData> dialogueDict
        = new Dictionary<int, DialogueData>();

    private void Awake()
    {
        LoadDialogueData();
    }

    private void LoadDialogueData()
    {
        dialogueDict.Clear();

        foreach (DialogueData data in DialogueStaticData.Dialogues)
        {
            if (!dialogueDict.ContainsKey(data.id))
            {
                dialogueDict.Add(data.id, data);
            }
            else
            {
                Debug.LogWarning(
                    $"중복된 Dialogue ID 발견: {data.id}"
                );
            }
        }
    }

    public DialogueLine? GetLine(
        ObjectData objectData,
        int lineIdx
    )
    {
        if (objectData == null)
            return null;

        if (!dialogueDict.TryGetValue(
            objectData.id,
            out DialogueData data
        ))
        {
            return null;
        }

        if (data.lines == null ||
            lineIdx < 0 ||
            lineIdx >= data.lines.Length)
        {
            return null;
        }

        return data.lines[lineIdx];
    }

    public Sprite GetPotrait(
        ObjectData objectData,
        DialogueLine line
    )
    {
        if (objectData == null ||
            objectData.imgs == null ||
            line.portraitIdx < 0 ||
            line.portraitIdx >= objectData.imgs.Length)
        {
            return null;
        }

        return objectData.imgs[line.portraitIdx];
    }

    // 이미 가져온 DialogueLine에서 이름 반환
    public string GetName(DialogueLine line)
    {
        return line.defaultname;
    }

    // ObjectData와 줄 번호로 이름 반환
    public string GetName(
        ObjectData objectData,
        int lineIdx
    )
    {
        DialogueLine? line = GetLine(
            objectData,
            lineIdx
        );

        if (!line.HasValue)
            return string.Empty;

        return line.Value.defaultname;
    }
}