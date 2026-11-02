using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue1Trigger : MonoBehaviour
{
    bool isTriggered;

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;

        if(isTriggered) return;

        isTriggered = true;

        
        InvestigationTextUI.Instance.ShowTextAndHide("아무것도 보이지 않는다.");
    }
}
