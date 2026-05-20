using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnim : Singleton<TextAnim>
{
    public int CPS;
    TextMeshProUGUI mainText;
    public GameObject endCursor;
    string targetText;
    int idx;
    float interval;
    public bool isAnim;
    void Awake()
    {
        mainText = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        if (isAnim)
        {// 텍스트 출력중에 SetText 호출시 한번에 출력
            mainText.text = targetText;
            CancelInvoke();
            AnimEnd();
        }
        else
        {// 일반 로직
            targetText = text;
            AnimStart();
        }
    }

    public void AnimStart()
    {

        isAnim = true;
        endCursor.SetActive(false);
        mainText.text = "";
        idx = 0;
        interval = 1.0f / CPS;
        Invoke("Animating", interval);

    }

    public void Animating()
    {
        if (mainText.text == targetText)
        {// 같으면 그냥 end로
            AnimEnd();
            return;
        }
        mainText.text += targetText[idx];
        //여기에 채팅 나오는 사운드 넣으면 될듯?
        idx++;
        Invoke("Animating", interval);
    }
    public void AnimEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }

    public void StartTyping(string target)
    {
        if (mainText == null) 
        mainText = GetComponent<TextMeshProUGUI>();

        if(isAnim)
        {
            CancelInvoke();
            isAnim = false;
        }

        targetText = target;
        mainText.text = "";
        AnimStart();
    }
}
