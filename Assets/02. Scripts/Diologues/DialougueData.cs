using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem
{
    // 대사 데이터를 담는 구조체
    [Serializable]
    public struct DialogueLine
    {
        public string sentence;
        public int portraitIdx;
        public int nextLineIdx;
        public bool hasChoices;

        public string choice1Text;
        public int choice1NextLineIdx;

        public string choice2Text;
        public int choice2NextLineIdx;
    }
    public struct DialogueData
    {
        public int id;
        public string name;
        public DialogueLine[] lines;

        public DialogueData(int id, string name, DialogueLine[] lines)
        {
            this.id = id;
            this.name = name;
            this.lines = lines;
        }
    }

    // 스크립트에서 데이터를 직접 들고 있는 static 클래스
    public static class DialogueStaticData
    {
        public static readonly List<DialogueData> Dialogues = new List<DialogueData>()
        {
            new DialogueData( 100, "책상", new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "평범한 책상이다.", nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "저장하시겠습니까?", nextLineIdx = 2,
                },
                new DialogueLine
                {
                    hasChoices = true,
                    choice1Text = "예", choice1NextLineIdx = 3,
                    choice2Text = "아니오", choice2NextLineIdx = 4
                },

                new DialogueLine
                {
                    sentence = "저장되었습니다.", nextLineIdx = -1
                },

                new DialogueLine
                {
                    sentence = "지금 기록할 필요는 없는거 같다.", nextLineIdx = -1
                }

            }
            )

            // new DialogueData(
            //     200,
            //     "상자",
            //     new string[] { "평범한 상자이다.", "옮길 수 있을거 같다." },
            //     new int[] { 0,0 }
            // ),
            // new DialogueData(
            //     1000,
            //     "Ludo",
            //     new string[] { "안녕 반가워.", "오늘 날씨가 참 좋아.", "옆에 상자 좀 옮겨줄래?" },
            //     new int[] { 1, 1, 1 } // 각 대사별 초상화 인덱스
            // ),

            // new DialogueData(
            //     1001,
            //     "Ludo",
            //     new string[] { "정말고마워!" },
            //     new int[] { 2 }
            // ),



            // new DialogueData(
            //     1100,
            //     "Luna",
            //     new string[] { "항..", "졸업작품이 너무 힘들어..." },
            //     new int[] { 3, 3 }
            // ),
            
            // 필요할 때마다 여기에 줄줄이 추가하면 됩니다.
        };
    }
}