using UnityEngine;
using UnityEngine.Playables;

public class PastCutSceneTrigger : MonoBehaviour
{
    private PlayableDirector director;
    private bool isTriggered;

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
