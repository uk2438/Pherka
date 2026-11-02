using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChange : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float alpha;
    private bool isTriggered;

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;

        if(isTriggered) return;

        isTriggered = true;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }
}
