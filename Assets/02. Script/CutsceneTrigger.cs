using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public int id;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CutsceneManager.Instance.cutsceneId = this.id;
            CutsceneManager.Instance.StartCutScene();
        }
    }
}