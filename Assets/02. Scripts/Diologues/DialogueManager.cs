using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueManager : Singleton<DialogueManager>
{
    // Key: Dialogue ID, Value: 대화 데이터
    private readonly Dictionary<int, DialogueData> dialogueDict
        = new Dictionary<int, DialogueData>();

    private int presentChpater = 1;

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

        int dialogueId = objectData.GetCurrentDialogueId();

        if (dialogueId < 0)
            return null;

        if (!dialogueDict.TryGetValue(
            dialogueId,
            out DialogueData data
        ))
        {
            Debug.LogWarning(
                $"Dialogue ID {dialogueId}에 해당하는 대화가 없습니다.",
                objectData
            );

            return null;
        }

        if (data.lines == null ||
            lineIdx < 0 ||
            lineIdx >= data.lines.Length)
        {
            CheckWasAction(objectData);
            return null;
        }

        return data.lines[lineIdx];
    }

    public DialogueLine? GetLine(int dialogueId, int lineIdx)
    {
        if (!dialogueDict.TryGetValue(dialogueId, out DialogueData data))
            return null;

        if (data.lines == null ||
            lineIdx < 0 ||
            lineIdx >= data.lines.Length)
        {
            return null;
        }

        return data.lines[lineIdx];
    }

    public string GetName(DialogueLine line)
    {
        return line.defaultname;
    }

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

    public void CheckWasAction(ObjectData objectData)
    {
        if (objectData == null) return;

        switch (presentChpater)
        {
            case 0:
                PrologueManager.Instance.CheckWasAction(objectData);
                break;
            case 1:
                Chapter1Manager.Instance.CheckWasAction(objectData);
                break;
            case 2:
                // PrologueManager.Instance.CheckWasAction(objectData);
                break;
            case 3:
                // PrologueManager.Instance.CheckWasAction(objectData);
                break;
            case 4:
                // PrologueManager.Instance.CheckWasAction(objectData);
                break;
        }
    }

    public void SetPresentChapter(int index)
    {
        presentChpater = index;
    }
}