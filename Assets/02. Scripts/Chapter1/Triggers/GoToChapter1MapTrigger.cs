using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GoToChapter1MapTrigger : MonoBehaviour
{
    private bool isTriggered;
    private ObjectData triggerObjectData;
    [SerializeField] private PlayableDirector director;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if(isTriggered && DialogueManager.Instance.GetPresentChapter() != 4) return;

        isTriggered = true;
        triggerObjectData = GetComponent<ObjectData>();


        GameManager.Instance.gameData.triggerObjectData = triggerObjectData;
        GameManager.Instance.TriggerAction();

        if(DialogueManager.Instance.GetPresentChapter() == 4)
        {
            GameManager.Instance.SetDialogueFinishedCallback(() =>
            {
                GameManager.Instance.CutSceneAction(director);
            });
        }


    }
}
