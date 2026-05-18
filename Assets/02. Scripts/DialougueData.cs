using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem
{
    // 대사 데이터를 담는 구조체
[Serializable]
public struct DialogueData
{
    public int id;
    public string name;
    public string[] sentences;
    public int[] sequences;

    public DialogueData(int id, string name, string[] sentences, int[] sequences)
    {
        this.id = id;
        this.name = name;
        this.sentences = sentences;
        this.sequences = sequences;
    }
}

// 스크립트에서 데이터를 직접 들고 있는 static 클래스
    public static class DialogueStaticData
    {
        public static readonly List<DialogueData> Dialogues = new List<DialogueData>()
        {
            new DialogueData(
                100, 
                "책상", 
                new string[] { "평범한 책상이다.", "저장하시겠습니까?" }, 
                new int[] { 0, 1 } // 각 대사별 초상화 인덱스
            ),

            new DialogueData(
                200, 
                "상자", 
                new string[] { "평범한 상자이다.", "옮길 수 있을거 같다." }, 
                new int[] { 0,0 }
            ),
            new DialogueData(
                1000,
                "Ludo", 
                new string[] { "안녕 반가워.", "오늘 날씨가 참 좋아.", "옆에 상자 좀 옮겨줄래?" }, 
                new int[] { 1, 1, 1 } // 각 대사별 초상화 인덱스
            ),

            new DialogueData(
                1001, 
                "Ludo", 
                new string[] { "정말고마워!" }, 
                new int[] { 2 }
            ),

            
            
            new DialogueData(
                1100, 
                "Luna", 
                new string[] { "항..", "졸업작품이 너무 힘들어..." }, 
                new int[] { 3, 3 }
            ),
            
            // 필요할 때마다 여기에 줄줄이 추가하면 됩니다.
        };
    }
}