using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerPuzzle : MonoBehaviour
{
    [Header("Computer Sprite")]
    [SerializeField] private Sprite originalComputer;
    [SerializeField] private Sprite[] computers;
    [SerializeField] private Sprite correctComputer;
    [SerializeField] private Sprite wrongComputer;
    [SerializeField] private GameObject Pherka;

    [Header("Input")]
    [SerializeField] private TMP_InputField inputField;

    private List<Sprite> spriteSequence = new List<Sprite>();
    private int[] answers = new int[4];
    private SpriteRenderer spriteRenderer;
    private bool isShowing;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        inputField.onSubmit.AddListener(OnSubmitInput);

        inputField.gameObject.SetActive(false);

        StartPuzzle();
    }

    // 제출된 문자열을 받아 코루틴에 전달
    private void OnSubmitInput(string input)
    {
        StartCoroutine(AlertPuzzle(input));
    }

    public IEnumerator AlertPuzzle(string input)
    {
        if (!GameManager.Instance.gameData.IsInputting) yield break;

        // 중복 제출 방지
        GameManager.Instance.gameData.IsInputting = false;

        inputField.gameObject.SetActive(false);
        UIManager.Instance.SetPuzzleInputActive(false);
        string correctAnswer = string.Empty;

        for (int i = 0; i < answers.Length; i++)
        {
            correctAnswer += answers[i].ToString();
        }

        bool isCorrect = input.Trim() == correctAnswer;

        spriteRenderer.sprite = isCorrect
            ? correctComputer
            : wrongComputer;

        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = originalComputer;

        if(isCorrect)
        {
            GameManager.Instance.StartMonologue(22001);
            GetComponent<ObjectData>().SetDialogueCondition(true);
            Pherka.GetComponent<ObjectData>().SetDialogueCondition(true);
            Chapter3Manager.Instance.puzzle2 = true;
        }
        else
        {
            GameManager.Instance.StartMonologue(22002);
        }
    }

    private void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onSubmit.RemoveListener(OnSubmitInput);
        }
    }

    public void StartPuzzle()
    {
        spriteSequence.Clear();

        int sum = 0;

        for (int i = 0; i < answers.Length; i++)
        {
            answers[i] = Random.Range(2, 6);
            sum += answers[i];
        }

        // 정답은 유지하고, 남은 등장 횟수만 별도로 관리
        int[] remainingCounts = (int[])answers.Clone();

        while (sum > 0)
        {
            int num = Random.Range(0, answers.Length);

            if (remainingCounts[num] > 0)
            {
                spriteSequence.Add(computers[num]);

                remainingCounts[num]--;
                sum--;
            }
        }
    }

    public IEnumerator ShowPuzzle()
    {
        // 동시에 여러 번 실행되는 것 방지
        if (isShowing) yield break;

        isShowing = true;
        GameManager.Instance.gameData.IsInputting = false;

        inputField.gameObject.SetActive(false);

        foreach (Sprite sprite in spriteSequence)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.3f);

            spriteRenderer.sprite = originalComputer;
            yield return new WaitForSeconds(0.2f);
        }

        isShowing = false;

    }

    // 선택지에서 호출할 함수
    public void OpenInputAfterDialogue()
    {
        if (isShowing) return;

        // 대화가 종료되더라도 패널 유지
        UIManager.Instance.SetPuzzleInputActive(true);

        // 선택지와 대화 정리가 끝난 뒤 입력 시작
        GameManager.Instance.SetDialogueFinishedCallback(OpenInput);
    }

    public void OpenInput()
    {
        if (isShowing) return;

        GameManager.Instance.gameData.IsInputting = true;

        UIManager.Instance.SetPuzzleInputActive(true);

        inputField.gameObject.SetActive(true);
        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

}