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

    // [Header("-----SettingData-----")]

}
public class GameData
{
    [Header("-----GameData-----")]
    public GameObject scanObject;
    public ObjectData triggerObjectData;
    public bool isAction = false;
    public bool isRunningCutScene = false;
    public bool isTrigger = false;
    public bool isDoorOpen = false;
    
    public int ChapterIdx;
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
    public bool isChoice = false;
}

//FadeManager에 사용할 변수들
[Serializable]
public class FadeData
{
    [Header("-----FadeData-----")]
    public Image fadeImage;
    public Image guideBackground;
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
    public CapsuleCollider2D hitBox;
    
}

public class MovementData
{
    public float hOffset;
    public float vOffset;
}
#endregion
// 일시정지했을 떄 나타나는 설정 데이터
#region Setting Data
public class MenuData
{
    public bool isSetting = false;
}
#endregion
