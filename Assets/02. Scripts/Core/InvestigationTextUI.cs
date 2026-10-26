using System.Collections;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class InvestigationTextUI : Singleton<InvestigationTextUI>
{
    [Header("기존 대화창 TextAnim")]
    [SerializeField] private TextAnim dialogueTextAnim;
    [SerializeField] private TextMeshProUGUI investigationText;

    private Coroutine typingCoroutine;

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

        if (investigationText != null)
        {
            investigationText.text = string.Empty;
            investigationText.maxVisibleCharacters = 0;
        }
        else if (investigationText != null)
            investigationText.gameObject.SetActive(false);
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
