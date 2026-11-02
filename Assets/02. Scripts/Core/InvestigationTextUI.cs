using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class InvestigationTextUI : Singleton<InvestigationTextUI>
{
    [Header("기존 대화창 TextAnim")]
    [SerializeField] private TextAnim dialogueTextAnim;
    [SerializeField] private TextMeshProUGUI investigationText;

    private Coroutine typingCoroutine;
    public bool IsTyping => typingCoroutine != null;

    public void ShowText(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            HideText();
            return;
        }

        if (dialogueTextAnim == null || investigationText == null)
        {
            Debug.LogError(
                "InvestigationTextUI의 Dialogue Text Anim과 Investigation Text를 연결해주세요.",
                this
            );
            return;
        }

        if (!isActiveAndEnabled)
        {
            Debug.LogError(
                "InvestigationTextUI는 항상 활성화된 관리 오브젝트에 배치해주세요.",
                this
            );
            return;
        }

        StopTyping();

        investigationText.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(PlayText(message));
    }

    public void HideText()
    {
        StopTyping();

        if (investigationText == null)
            return;

        investigationText.text = string.Empty;
        investigationText.maxVisibleCharacters = 0;
        investigationText.gameObject.SetActive(false);
    }

    public async void ShowTextAndHide(string message)
    {
        ShowText(message);

        // 글자 출력이 끝날 때까지 대기
        while (this != null && IsTyping)
        {
            await Task.Yield();
        }

        // 출력 완료 후 1초 대기
        await Task.Delay(1000);

        if (this != null)
            HideText();
    }

    private IEnumerator PlayText(string message)
    {
        yield return dialogueTextAnim.TypeInvestigationText(investigationText, message);

        typingCoroutine = null;
    }

    private void StopTyping()
    {
        if (typingCoroutine == null)
            return;

        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
    }
}
