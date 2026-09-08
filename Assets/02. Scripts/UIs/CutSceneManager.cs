using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    private Image cutSceneImage;

    void Awake()
    {
        cutSceneImage = GetComponent<Image>();
    }

    public void ActiveCutSceneImage(Sprite img)
    {
        if (img == null) return;

        if (!GameManager.Instance.gameData.isRunningCutScene)
        {
            GameManager.Instance.gameData.isRunningCutScene = true;
        }
        cutSceneImage.sprite = img;
        cutSceneImage.enabled = true;
    }

    public void DeactiveCutSceneImage()
    {
        if (cutSceneImage == null) return;
        cutSceneImage.enabled = false;

        cutSceneImage.sprite = null;
    }
    public void FinishImageSignal()
    {
        GameManager.Instance.gameData.isRunningCutScene = false;
        DeactiveCutSceneImage();
    }

    

}
