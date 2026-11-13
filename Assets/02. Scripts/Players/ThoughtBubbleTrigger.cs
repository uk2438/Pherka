using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class ThoughtBubbleTrigger : MonoBehaviour
{
    [SerializeField] private Animator thought;
    [SerializeField] private SpriteRenderer thoughtImage;
    [SerializeField] private Sprite sprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        thoughtImage.enabled = false;
        thoughtImage.sprite = sprite;

        thought.gameObject.SetActive(true);
        thought.SetTrigger("Enter");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        thought.SetTrigger("Exit");
        thought.gameObject.SetActive(false);
    }

}
