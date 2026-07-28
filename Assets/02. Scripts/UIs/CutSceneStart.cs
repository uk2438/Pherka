using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneStart : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    void OnTriggerEnter2D(Collider2D other) {
        
        if(director == null) return;

        GameManager.Instance.gameData.isRunningCutScene = true;
        director.Play();
    }
}
