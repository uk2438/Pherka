using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UnconsciousCutSceneTrigger : MonoBehaviour
{
    private bool isTriggered;
    private PlayableDirector director;

    private void Awake() {
        director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(isTriggered) return;

        isTriggered = true;

        GameManager.Instance.CutSceneAction(director);
    }
}
