using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public GameObject targetRoom;
    public GameObject curruntRoom;

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player"))
        {
            StartCoroutine(TeleportSequence(other.transform));
        }    
    }

    IEnumerator TeleportSequence(Transform playerTransform)
    {
            yield return StartCoroutine(FadeManager.Instance.FadeOut(1f));
            targetRoom.SetActive(true);
            playerTransform.transform.position = targetRoom.transform.position;


            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(FadeManager.Instance.FadeIn(1f));

            if(curruntRoom != null)
            {
                curruntRoom.SetActive(false);

            }

    }
}
