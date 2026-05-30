using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region Manager Variables

//GameManager에 사용할 변수들
[Serializable]
public class UIData
{
    [Header("-----DialogueData-----")]
    public Animator dialogueBox;

    [Header("-----TextData-----")]
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI nameText;
    [HideInInspector]
    public int talkIdx;

    [Header("-----PotraitData-----")]

    public Image potrait;
    public GameObject potraitObj;
    public Animator potraitAnim;
    [HideInInspector]
    public int prevPotrait, currPotrait;
}
public class GameData
{
    [Header("-----GameData-----")]
    public GameObject scanObject;
    public bool isAction = false;

    public bool isDoorOpen = false;
}

[Serializable]
public class PanelData
{
    [Header("-----PanelData-----")]
    public GameObject pausePanel;
    public GameObject quitPanel;
    public GameObject checkPanel;
    public GameObject firstButton;
    [HideInInspector]
    public bool isPause = false;
}

//FadeManager에 사용할 변수들
[Serializable]
public class FadeData
{
    [Header("-----FadeData-----")]
    public Image fadeImage;
    [HideInInspector]
    public bool isFading = false;
}

//TeleportManager에 사용할 변수들
[Serializable]
public class TeleportData
{
    [Header("-----TeleportData-----")]
    public GameObject targetRoom;
    public Vector3 offsetPosition;
}

[Serializable]

public class SoundData
{
    [Header("-----SoundData-----")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;
}

#endregion

#region Player and Object Variables
[Serializable]
public class PlayerData
{
    public Animator anim;
    public float grabDelay;
    public BoxCollider2D hitBox;
    
}

public class MovementData
{
    public float hOffset;
    public float vOffset;
}
#endregion