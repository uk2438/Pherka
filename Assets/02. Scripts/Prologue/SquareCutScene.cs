using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SquareCutScene : MonoBehaviour
{
    // bool isWatched = false;
    [Header("컷신 director")]
    [SerializeField] public PlayableDirector director;

    void OnTriggerEnter2D(Collider2D other)
    {

        director.Play();

    }
}
