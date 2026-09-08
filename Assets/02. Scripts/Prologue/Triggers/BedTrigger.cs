using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BedTrigger : MonoBehaviour
{
    private bool isTrigger = false;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Vector3 position;
    private Transform player;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if(isTrigger) return;

        isTrigger = true;
        player = other.transform;


        GameManager.Instance.StartMonologue(20002, FirstOnMonologueFinished);

    }

    private void FirstOnMonologueFinished()
    {
        director.Play();
        PrologueManager.Instance.StartChapterOne();

        Invoke(nameof(MovePlayer), 2f);
    }

    private void MovePlayer()
    {
        if(player == null) return;
        player.position = position;
        PrologueManager.Instance.EndPrologue();


    }
    private void DeactivePrologueMap()
    {
    }
    // private void FirstOnMonologueFinished(Transform transform)
    // {
    //     StartCoroutine(DirectorPlay(transform));

    // }

    // private IEnumerator DirectorPlay(Transform transform)
    // {
    //     director.Play();
    //     yield return new WaitForSeconds(2f);
    //     PrologueManager.Instance.StartChapterOne();
    //     transform.position = position;

    // }
}
