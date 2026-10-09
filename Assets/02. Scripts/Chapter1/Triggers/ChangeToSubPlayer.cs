using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeToSubPlayer : MonoBehaviour
{
    [Header("SubPlayer Transform")]
    [SerializeField] private Transform subPlayer;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if(isTriggered) return;

        isTriggered = true;

        SetSubPlayer();

    }

    private void SetSubPlayer()
    {
        PlayerFollower follower = subPlayer.GetComponent<PlayerFollower>();
        BoxCollider2D collider2D = subPlayer.GetComponent<BoxCollider2D>();
        subPlayer.tag = "SubPlayer";

        follower.enabled = true;
        collider2D.enabled = false;
    }
}
