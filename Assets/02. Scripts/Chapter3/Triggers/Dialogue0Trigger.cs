using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue0Trigger : MonoBehaviour
{
    ObjectData data;
    bool isTriggered;

    private void Awake() {
        data = GetComponent<ObjectData>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;

        if(isTriggered) return;

        isTriggered = true;

        FadeManager.Instance.StartDialogueAfterFade(data);
    }
}
