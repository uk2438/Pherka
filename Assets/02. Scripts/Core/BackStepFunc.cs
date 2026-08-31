using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStepFunc : MonoBehaviour
{
    ObjectData objectData;
    [Header("백스텝 설정")]
    public float stepDistance = 1f;
    public float moveDuration = 0.3f;
    public enum BackStepDirection {Up, Down, Left, Right};
    public BackStepDirection backStepDir;
    private Coroutine backStepCoroutine;


    public void BackStep(Transform player, float distance)
    {
        if (backStepCoroutine != null)
            StopCoroutine(backStepCoroutine);

        // backstep 방향 선택
        Vector3 dir = GetDirection(backStepDir);

        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + dir * distance;

        backStepCoroutine = StartCoroutine(MoveBackCoroutine(player, startPos, targetPos));
    }

    private Vector3 GetDirection(BackStepDirection dir)
    {
        switch (dir)
        {
            case BackStepDirection.Up: return Vector3.up;
            case BackStepDirection.Down: return Vector3.down;
            case BackStepDirection.Left: return Vector3.left;
            case BackStepDirection.Right: return Vector3.right;
            default: return Vector3.up;
            
        }
    }

    private IEnumerator MoveBackCoroutine(Transform player, Vector3 startPos, Vector3 targetPos)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            player.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        player.position = targetPos;
        backStepCoroutine = null;
    }
}
