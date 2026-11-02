using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue3Trigger : MonoBehaviour
{
    bool isTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (isTriggered) return;

        isTriggered = true;


        InvestigationTextUI.Instance.ShowTextAndHide("그렇게 하염없이 걷다보니 한줄기 빛이 보였다.");
    }
}
