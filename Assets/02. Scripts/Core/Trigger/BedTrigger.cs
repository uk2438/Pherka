using UnityEngine;
using UnityEngine.Playables;

public class BedTrigger : MonoBehaviour
{
    private bool isTrigger = false;
    [SerializeField] private PlayableDirector director;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if(isTrigger) return;

        isTrigger = true;

        GameManager.Instance.StartMonologue(20002, FirstOnMonologueFinished);

    }

    private void FirstOnMonologueFinished()
    {
        director.stopped += OnTimelineFinished;
        director.Play();
        PrologueManager.Instance.StartChapterOne();
    }
        private void OnTimelineFinished(PlayableDirector finishedDirector)
    {
        // 다른 PlayableDirector의 종료 이벤트는 무시
        if (finishedDirector != director) return;

        director.stopped -= OnTimelineFinished;

        GameManager.Instance.StartMonologue(20004);
    }

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnTimelineFinished;
        }
    }
}
