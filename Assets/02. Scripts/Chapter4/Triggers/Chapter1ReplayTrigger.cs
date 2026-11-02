using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Chapter1ReplayTrigger : MonoBehaviour
{
    private bool isTriggered;
    private PlayableDirector director;

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(isTriggered) return;

        isTriggered = true;

        director = GetComponent<PlayableDirector>();

        GameManager.Instance.CutSceneAction(director);
        Chapter4Manager.Instance.SetChapter1ClearCheck(true);


    }
}
