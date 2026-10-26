using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// 기존 TextAnim의 CPS를 사용해 별도의 TMP 텍스트를 출력합니다.
/// 대화창의 targetText, isAnim, endCursor, Invoke에는 접근하지 않습니다.
/// </summary>
public static class InvestigationTextAnimExtensions
{
    public static IEnumerator TypeInvestigationText(this TextAnim source,TMP_Text target,string message)
    {
        if (source == null || target == null)
            yield break;

        float charactersPerSecond = Mathf.Max(1, source.CPS);

        // 전체 문장을 먼저 넣어 줄바꿈 위치를 고정하고,
        // 실제로 보이는 글자 수만 하나씩 늘립니다.
        target.text = message ?? string.Empty;
        target.maxVisibleCharacters = 0;
        target.ForceMeshUpdate();

        int totalCharacters = target.textInfo.characterCount;
        float characterProgress = 0f;
        int visibleCharacters = 0;

        while (visibleCharacters < totalCharacters)
        {
            yield return null;

            if (target == null)
                yield break;

            characterProgress += Time.deltaTime * charactersPerSecond;
            visibleCharacters = Mathf.Min(
                totalCharacters,
                Mathf.FloorToInt(characterProgress)
            );

            target.maxVisibleCharacters = visibleCharacters;
        }

        if (target != null)
            target.maxVisibleCharacters = int.MaxValue;
    }
}
