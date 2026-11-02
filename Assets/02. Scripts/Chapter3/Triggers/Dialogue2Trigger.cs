using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue2Trigger : MonoBehaviour
{
    bool isTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (isTriggered) return;

        isTriggered = true;


        InvestigationTextUI.Instance.ShowTextAndHide("칠흑같은 어둠속에 내가 할 수 있는것은 나아가는 것 뿐이였다.");


    }
}
