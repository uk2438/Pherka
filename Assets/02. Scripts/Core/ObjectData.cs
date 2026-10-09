using UnityEngine;

public class ObjectData : MonoBehaviour
{
    [Header("조건 미충족 시 사용할 대화 ID")]
    public int[] defaultDialogueIds;

    [Header("조건 충족 시 사용할 대화 ID")]
    public int[] satisfyDialogueIds;

    [Header("조건 충족 여부")]
    [SerializeField] public bool isConditionSatisfied;

    [Header("저장용 오브젝트 고유 ID")]
    [SerializeField] private int saveId;

    [Header("현재 대화 순서")]
    [SerializeField] private int currentDialogueIndex;
    public int SaveId => saveId;

    // 현재 조건에 맞는 대화 배열 반환
    private int[] GetActiveDialogueIds()
    {
        return isConditionSatisfied ? satisfyDialogueIds : defaultDialogueIds;
    }

    public int GetCurrentDialogueId(bool? satisfied = null)
    {
        bool useSatisfied = satisfied ?? isConditionSatisfied;
        int[] dialogueIds = useSatisfied
            ? satisfyDialogueIds
            : defaultDialogueIds;

        if (dialogueIds == null || dialogueIds.Length == 0)
            return -1;

        int index = Mathf.Clamp(
            currentDialogueIndex,
            0,
            dialogueIds.Length - 1
        );

        return dialogueIds[index];
    }

    public void AdvanceDialogue()
    {
        int[] activeDialogueIds = GetActiveDialogueIds();

        if (activeDialogueIds == null ||
            activeDialogueIds.Length == 0)
        {
            return;
        }

        if (currentDialogueIndex < activeDialogueIds.Length - 1)
        {
            currentDialogueIndex++;
        }
    }

    public void SetDialogueIndex(int index)
    {
        int[] activeDialogueIds = GetActiveDialogueIds();

        if (activeDialogueIds == null || activeDialogueIds.Length == 0)
            return;

        currentDialogueIndex = Mathf.Clamp(
            index,
            0,
            activeDialogueIds.Length - 1
        );
    }


    public int GetDialogueIndex()
    {
        return currentDialogueIndex;
    }

    public void ResetDialogue()
    {
        currentDialogueIndex = 0;
    }

    public void SetDialogueCondition(bool value)
    {
        if (isConditionSatisfied == value)
            return;

        isConditionSatisfied = value;

        // 조건 변경 후 새 대화 배열의 첫 대사부터 시작
        currentDialogueIndex = 0;
    }

    public bool IsDialogueConditionSatisfied()
    {
        return isConditionSatisfied;
    }
}