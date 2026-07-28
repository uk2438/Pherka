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
        public string name;
        public string defaultname
        {
            get
            {
                return name ?? "";
            }
        }
        public int portraitIdx;
        public int nextLineIdx;
        public bool hasChoices;
        public bool isCutSceneEnd;


        public string choice1Text;
        public int choice1NextLineIdx;

        public string choice2Text;
        public int choice2NextLineIdx;
    }
    public struct DialogueData
    {
        public int id;
        public DialogueLine[] lines;

        public DialogueData(int id, DialogueLine[] lines)
        {
            this.id = id;
            this.lines = lines;
        }
    }

    // 스크립트에서 데이터를 직접 들고 있는 static 클래스
    public static class DialogueStaticData
    {
        public static readonly List<DialogueData> Dialogues = new List<DialogueData>()
        {
            new DialogueData(
                0,
                new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "평범한 책상이다.", name = "책상", nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "저장하시겠습니까?", name = "책상", nextLineIdx = 2
                },
                new DialogueLine
                {
                    hasChoices = true,
                    choice1Text = "예", choice1NextLineIdx = 3,
                    choice2Text = "아니오", choice2NextLineIdx = 4
                },

                new DialogueLine
                {
                    sentence = "저장되었습니다.", name = "책상", nextLineIdx = -1
                },

                new DialogueLine
                {
                    sentence = "지금 기록할 필요는 없는거 같다.", name = "책상", nextLineIdx = -1
                }

            }
        ),

        new DialogueData (
            1,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "앞에는 집과 평야가 무한히 펼쳐져있다.", nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            9000, // Opening
            new DialogueLine[]
            {
                // start cutscene
                new DialogueLine
                {
                    sentence = "아득한 하늘 위 영혼들이 머무는 세계가 있었다.", nextLineIdx = 1
                },

                new DialogueLine
                {
                    sentence = "하늘은 죄를 지은 영혼들을 수용하는 지옥, 선을 배푼 영혼들의 안식처 천국, 그런 선과 악을 구분 짓는 황혼이 있었다.", nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "모든 영혼은 황혼을 거쳐 판결을 받았고, 각자의 자리에서 시간을 보낸 뒤에야 비로소 다음 생으로 윤회할 수 있었다.", nextLineIdx = -1
                },

                // 1st cutscene
                new DialogueLine
                {
                    sentence = "하지만, 세월이 흐를수록 하늘을 찾는 영혼들의 수는 많아지고, 황혼에서 판결이 지연됐으며, 지옥과 천국에는 수용할 수 있는 공간이 부족해졌다.", nextLineIdx = -1
                },
                
                // 2nd cutscene
                new DialogueLine
                {
                    sentence = "각 영역의 관리자들은 이 현상을 해결하기 위해 한자리에 모였다", nextLineIdx = -1
                },
                
                // 3rd cutscene
                new DialogueLine
                {
                    sentence = "여러가지 많은 의견들이 오갔지만", nextLineIdx = -1
                },
                
                //4th cutscene
                new DialogueLine
                {
                    sentence = "명쾌한 답은 쉽게 나오지 않았다.", nextLineIdx = -1
                },
                
                //5th cutscene
                new DialogueLine
                {
                    sentence = "그렇게 아무런 결론 없이 토론이 끝날 때 즈음, 황혼 관리자가 입을 열었다.", nextLineIdx = 8
                },
                
                new DialogueLine
                {
                    sentence = "“선과 악을 구분 짓는 것이 진정 중요한 일입니까? 현세의 고난을 견뎌온 영혼들에게, 이곳 에서만이라도 온전한 휴식을 주는 것은 어떻겠습니까?”", nextLineIdx = -1
                },

                //6th cutscene
                new DialogueLine
                {
                    sentence = "이 제안을 기점으로 지옥, 천국, 황혼의 경계가 허물어졌다.", nextLineIdx = 10
                },

                new DialogueLine
                {
                    sentence = "경계가 사라진 하늘은 이제 영혼들의 거대한 쉼터가 되었고, 충분한 안식을 취한 영혼들은 비로소 스스로의 의지에 따라 윤회의 길을 걷게 되었다.", nextLineIdx = -1
                },

                new DialogueLine
                {
                    sentence = "...", name = "하달", nextLineIdx = 12
                },

                new DialogueLine
                {
                    sentence = "출근해야지...", name = "하달", nextLineIdx = -1, isCutSceneEnd = true
                }

            }
        ),

        new DialogueData
        {
            
        }
            
            // 필요할 때마다 여기에 줄줄이 추가하면 됩니다.
        };
    }
}