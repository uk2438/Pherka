using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    private bool isTriggered;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Carried")) return;
        if(isTriggered) return;

        isTriggered = true;
        Chapter1Manager.Instance.isPherka3 = true;
        GameManager.Instance.StartMonologue(21002 + Chapter1Manager.Instance.completeIndex);
        Chapter1Manager.Instance.completeIndex++;
    }

}
