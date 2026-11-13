using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GymCutSceneTrigger : MonoBehaviour
{
    private PlayableDirector director;
    private bool isTriggered;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(!other.CompareTag("Player")) return;

        if(isTriggered) return;
        
        isTriggered = true;

        GameManager.Instance.CutSceneAction(director, () =>
        {
            Chapter4Manager.Instance.GymCutSceneEnd(7021);
        });
    }

}
