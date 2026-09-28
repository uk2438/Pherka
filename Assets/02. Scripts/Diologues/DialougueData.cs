using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor.Callbacks;
using UnityEngine.Experimental.AI;
using Unity.VisualScripting;

namespace DialogueSystem
{
    [Serializable]
    public struct DialogueChoice
    {
        public string text;
        public int nextLineIdx;
        public int eventId;
    }
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

        public int potraitIdx;
        public int nextLineIdx;
        public bool isCutSceneEnd;

        public DialogueChoice[] choices;

        // 배열에 선택지가 있으면 자동으로 true
        public bool hasChoices => ChoiceCount > 0;

        public int ChoiceCount => choices?.Length ?? 0;
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

    public enum DialogueEventType
    {
        None,
        FadeOut,
        FadeIn,
        FadeOutIn,
        SetMartActive,
        ShowGuide0,
        ShowGuide1,
        TeleportPlayer
    }

    public enum DialogueEventTiming
    {
        None,
        BeforeLine
    }
    public enum DialogueTeleportTarget
    {
        None,
        FirstGoToWork,
        SecondGoToWork,
        GoToHome,
        GoChapter1Map,
        GoToMainStreet
    }


    [System.Serializable]
    public class DialogueEventData
    {
        public int dialogueId;
        public int lineIndex;

        public DialogueEventType eventType;
        public DialogueEventTiming timing;

        public float duration;
        public DialogueTeleportTarget teleportTarget;
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
                    sentence = "평범한 책상이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "저장하시겠습니까?", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "", potraitIdx = -1,
                    choices = new DialogueChoice[]
                    {
                        new DialogueChoice
                        {
                            text = "네", nextLineIdx = 3
                        },
                        new DialogueChoice
                        {
                            text = "아니오", nextLineIdx = 4
                        }
                    }

                },
                new DialogueLine
                {
                    sentence = "저장되었습니다.", name = "시스템", potraitIdx = -1, nextLineIdx = -1
                },

                new DialogueLine
                {
                    sentence = "지금 기록할 필요는 없는거 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),

        // 1~999 trigger dialogue
        
        //빌딩 트리거
        new DialogueData (
            1,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "앞에는 집과 평야가 무한히 펼쳐져있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //1F - 2F 트리거
        new DialogueData(
            2,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "데스크에서 업무를 받고 올라가자.",name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        // 1F - B1F 트리거
        new DialogueData(
            3,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "출근 시간이니 농떙이 피울 시간은 없다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        // 2F - 3F 트리거
        new DialogueData(
            4,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "놀 시간은 없다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //광장 큰 문 트리거
        new DialogueData(
            5,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여긴 영혼들이 들어오는 입구입니다!!", name = "근위병", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "들어가시면 안돼요!!", name = "근위병", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            6,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "영혼의 수가 너무 많아서 들어갈 수가 없다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            7,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여긴 들어가면 안될거같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            8,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "절차대로 한다면, 이 사람도 안정될 수 있을 것이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        // 100 ~ 199 chapter1 triggers
        new DialogueData(
            100,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "어떻게 업무가 진행되는지는 어느정도 알고계시죠?", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "네! 기본적인 것들은 알고있어요!", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "그럼 설명하지않고 출발할게요.", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "....", name = "모사", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "긴장되세요?", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "... 네 조금요.", name = "모사", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "....", name = "모사", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "별거 없어요. 잘 하실 수 있을거에요.", name = "하달", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "네!!", name = "모사", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "그럼 출발할게요.", name = "하달", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "평범한 집이네요.", name = "하달", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence = "기본적인건 알고계신다 했으니... 그럼 여기서 할일은 뭐죠?", name = "하달", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence = "영혼을 찾는다!", name = "모사", potraitIdx = -1, nextLineIdx = 13
                },
                new DialogueLine
                {
                    sentence = "맞아요.", name = "하달", potraitIdx = -1, nextLineIdx = 14
                },
                new DialogueLine
                {
                    sentence = "관리자님 말씀대로 모든 기억을 재구성 한다고 하셨으니, 아마 이 영혼의 시간 순서대로 재구성 할 것 같아요.", name = "하달", potraitIdx = -1, nextLineIdx = 15
                },
                new DialogueLine
                {
                    sentence = "그럼 어린 영혼을 찾으면 되겠네요!", name = "모사", potraitIdx = -1, nextLineIdx = 16
                },
                new DialogueLine
                {
                    sentence = "제법이신데요?", name = "하달", potraitIdx = -1, nextLineIdx = 17
                },
                new DialogueLine
                {
                    sentence = "그..그런가요? 헤헤...", name = "모사", potraitIdx = -1, nextLineIdx = 18
                },
                new DialogueLine
                {
                    sentence = "그럼 모사씨 말대로 어린 영혼을 찾으러 가볼까요?", name = "하달", potraitIdx = -1, nextLineIdx = 19
                },
                new DialogueLine
                {
                    sentence = "네!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //101 MainMap Backstep Trigger
        new DialogueData(
            101,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "영혼을 먼저 찾아야된다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
            ),
        //102 TimeMachine Explain Trigger
        new DialogueData(
            102,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여긴.. 아까 공간과는 다르게 이질적이네요....", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "엇! 저기 엄청 큰 시계가 있어요!", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = " 한번 조사해볼까요?", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //103 Check Trigger
        new DialogueData(
            103,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "조사를 충분히 해보자.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //104 chapter1 clear trigger
        new DialogueData(
            104,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "아직 여기에 볼일이 남았다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            105,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "다음 기억으로 가죠.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "다음 기억은 오른쪽인거 같네요.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "저기.. 하달씨..", name = "모사", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence  = "네?", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "이렇게 해도 될까요..?", name = "모사", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "그게 무슨 의미죠?", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "사실... 업무를 시작하기 전 부터 생각하고 있었는데...", name = "모사", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "이렇게 기억을 조작하는게 영혼을 위한 일이 맞을까요?", name = "모사", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "....", name = "하달", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "이런 방법 말고 더 좋게 영혼을 안정시키는 방법이 있지 않을까요?", name = "모사", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "모사 씨", name = "하달", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence = "이 일은 총 관리자가 직접 생각하신 업무에요.", name = "하달", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence = "이 방법이 정답이라고 할 순 없지만,", name = "하달", potraitIdx = -1, nextLineIdx = 13
                },
                new DialogueLine
                {
                    sentence = "정답에 가까운 방법이라고 생각해요.", name = "하달", potraitIdx = -1, nextLineIdx = 14
                },
                new DialogueLine
                {
                    sentence = "그렇게 믿어왔기에 저도 계속 이 업무를 주 업무로 하고있고요.", name = "하달", potraitIdx = -1, nextLineIdx = 15
                },
                new DialogueLine
                {
                    sentence = "그렇지만..!", name = "모사", potraitIdx = -1, nextLineIdx = 16
                },
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx = 17
                },
                new DialogueLine
                {
                    sentence = "네... 다음 기억으로 가죠..", name = "모사", potraitIdx = -1, nextLineIdx = 18
                },
                new DialogueLine
                {
                    sentence = "...네", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        // 1000~4999 structure and object dialogue

        // 하달 집 표지판
        new DialogueData
        (
            1000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "하달의 집", name = "표지판", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        // 다른 집 표지판
        new DialogueData
        (
            1001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 집에 살고있는 관리자의 이름인 것 같다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        // 다른 집
        new DialogueData
        (
            1002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "다른 관리자의 집인 것 같다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        // 관리자 거주 공간
        new DialogueData
        (
            1003,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여기는 관리자 거주공간 입니다.", name = "표지판", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "정숙해주시길 바랍니다.", name = "표지판", potraitIdx = -1, nextLineIdx = -1
                }
            }

        ),

        // 분수대
        new DialogueData
        (
            1004,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "움직이지 않는 분수대다.", potraitIdx = -1, nextLineIdx = -1
                },
            }
        ),

        // <-관리자 거주 공간 / 가디언 빌딩->
        new DialogueData
        (
            1005,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "<- 관리자 거주공간 \t 가디언 빌딩 ->", potraitIdx = -1, nextLineIdx = -1
                }
            }

        ),

        new DialogueData(
            1006,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "1006", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1007,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "1007", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1008,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 1008", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1009,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 1009", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1010,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 1010", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1011,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 1011", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1012,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 1012", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1013,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 1013", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1014,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테스트 테스트", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1015,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "흥미로운 책들이 많지만, 다읽을 시간은 없을 것 같다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //1016 ~ 1019 2F 문
        new DialogueData(
            1016,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "첫번째 문이다.",potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1017,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "두번째 문이다.",potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "들어가시겠습니까",potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "" ,potraitIdx = -1,
                    choices = new DialogueChoice[]
                    {
                        new DialogueChoice
                        {
                            text = "예", nextLineIdx = 4
                        },
                        new DialogueChoice
                        {
                            text = "아니오", nextLineIdx = 3
                        }
                    }
                },
                new DialogueLine
                {
                    sentence = "아 출근 하기 싫다...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                },
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },

                new DialogueLine
                {
                    sentence = "시작해볼까...", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "일단 영혼을 찾아보자.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1018,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "세번째 문이다.",potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1019,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "네번째 문이다.",potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 큰 곰돌이 인형
        new DialogueData(
            1020,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "엄청 큰 곰돌이 인형이다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        
        //마트 주류선반
        new DialogueData(
            1021,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이름모를 와인과 주류가 선반에 나열되어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 계산기
        new DialogueData(
            1022,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "계산대이다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 장바구니
        new DialogueData(
            1023,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "장바구니가 진열되어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 왼측 선반들
        new DialogueData(
            1024,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "여기 선반에 있는 물건들은 분명 다 다른데 똑같이 보여...",  name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        
        //마트 곰돌이 아랫 선반들
        new DialogueData(
            1025,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "의약품이 진열되어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 곰돌이 윗 선반들
        new DialogueData(
            1026,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "무언가가 진열되어있지만 무엇인지 알 수 없다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        
        //마트 우측 아랫 선반들
        new DialogueData(
            1027,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "주방용품, 청소용품 등의 가정의 쓰이는 용품들이 진열되어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 곰돌이 우측 냉장고
        new DialogueData(
            1028,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "냉동 식품들이 들어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //마트 음료수 냉장고
        new DialogueData(
            1029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "음료수가 들어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //마트 아이스크림 냉동고
        new DialogueData(
            1030,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "아이스크림이 들어있다.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //마트 테티베어
        new DialogueData(
            1031,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "테디베어.", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //주류 shinyeffect
        new DialogueData(
            1032,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "주류 진열대에 곰돌이 인형이 있을리가 없지.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //전자기기 shinyeffect
        new DialogueData(
            1033,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 진열대는 다른 진열대와 달리 선명하게 보이네.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                                new DialogueLine
                {
                    sentence = "이 진열대는 전자기기 용품을 팔고있는거 같아.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //장난감 shinyeffect
        new DialogueData(
            1034,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 진열대는 다른 진열대와 달리 선명하게 보이네.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이 진열대는 장난감을 팔고있는거 같아.", name="하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "... 하지만 인형은 팔고있지 않아...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //화장품 shinyeffect
        new DialogueData(
            1035,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 진열대는 다른 진열대와 달리 선명하게 보이네.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                                new DialogueLine
                {
                    sentence = "이 진열대는 화장품을 팔고있는거 같아.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //인형 shinyeffect
        new DialogueData(
            1036,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 진열대는 다른 진열대와 달리 선명하게 보이네.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                                new DialogueLine
                {
                    sentence = "이 진열대는 인형을 팔고있는거 같아.", name="하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "곰돌이 인형은 안보이네...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //과자 shinyeffect
        new DialogueData(
            1037,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 진열대는 다른 진열대와 달리 선명하게 보이네.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이 진열대는 과자를 팔고있는거 같아.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //의약품 shinyeffect
        new DialogueData(
            1038,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "의약품과 영양제를 팔고있어.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //욕실용품 shinyeffect
        new DialogueData(
            1039,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "욕실 청소용품을 팔고있어.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //냉동식품 shinyeffect
        new DialogueData(
            1040,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "냉동식품을 팔고있어.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //아이스크림 냉동고 shinyeffect
        new DialogueData(
            1041,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "아이스크림 냉동고야.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                               new DialogueLine
                {
                    sentence = "내가 어릴때는 장난감이나 인형보다는 먹는거를 좋아했었는데...", name="하달", potraitIdx = -1, nextLineIdx = 2
                },
                               new DialogueLine
                {
                    sentence = "특히 구슬 아이스크림을 좋아했어서 맨날 사달라고 했는데 비싸다고 안사주셨지.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
        // 음료수 냉장고 shinyeffect
        new DialogueData(
            1042,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "음료수를 팔고있어.", name="하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "인형을 찾아야되는데 왜 냉장고를 보고있지?", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        
        //Home

        //의자
        new DialogueData(
            1043,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "낡은 의자다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //방안 책상
        new DialogueData(
            1044,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="책상이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //거실 티비
        new DialogueData(
            1045,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="큰 티비다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //쇼파
        new DialogueData(
            1046,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="쇼파다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //티비 옆 꽃
        new DialogueData(
            1047,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="이름 모를 꽃이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence="이상하게 꽃향기는 안난다.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "... 가만보니 조화다...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //거실 책상
        new DialogueData(
            1048,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="혼자쓰기엔 너무 큰 책상이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //가스레인지
        new DialogueData(
            1049,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="가스레인지다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "오랜시간동안 사용이 안된거같다.", name="하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
        //싱크대
        new DialogueData(
            1050,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="싱크대이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //도마
        new DialogueData(
            1051,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="오래된 도마 위에 칼이 놓여져있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //작은 박스
        new DialogueData(
            1052,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="작은 박스이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //큰 박스
        new DialogueData(
            1053,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="큰 박스이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "크기에 비해 무겁지는 않다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
        //큰 꽃
        new DialogueData(
            1054,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="이름모를 식물이 화분에 심어져있다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "메모가 있다.", name = "메모", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "'한달에 한번씩 물주기'", name = "메모", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "이래서 살아있는건가...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //사료
        new DialogueData(
            1055,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="강아지 사료이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //화분 두개
        new DialogueData(
            1056,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="책상 위에 두개의 꽃들이 놓여져있다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence="이상하게 꽃향기는 안난다.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "... 가만보니 조화다...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),


        //퇴근 후 문 id
        new DialogueData(
            1057,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "출근을 두 번 하는건 미친짓이지.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            1058,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "벤치다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1059,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "휴지통이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1060,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "가로등이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "불은 안들어와있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1061,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "향기로운 꽃이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1062,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "관목이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1063,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "영혼들을 위한 쇼파이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1064,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "유리책상이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1065,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "관목이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1066,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "쉼터에 대한 안내서가 들어가있다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="영혼들을 위한것이니 나는 읽을 필요가 없다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1067,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "정수기다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "진짜 정수기는 아니다.... 여기있는 영혼들과 관리자는 목이 안마르기 때문이다.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "장식용으로 설치한것 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                },
            }
        ),
        new DialogueData(
            1068,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "책장이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "내가 즐겨읽은 책들이 가득하다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            1069,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "서랍이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "서랍 안에는 자주입는 옷들이 있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //2000~ chapter1

        new DialogueData(
            2000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "두번째 문이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        // Timemachine Clock Explain
        new DialogueData(
            2001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "시계가 엄청 커요!", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이걸로 뭔가 하는걸까요?", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "시계의 시침을 바꿀수 있는거 같네요.", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "바꾸면 뭐가 달라지지 않을까요??", name = "모사", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "음...", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "달리 할수있는거도 없는데 한번 해보죠.", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "좋아요!!", name = "모사", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "(시간을 바꾸고싶으면 한번 더 시계를 조사해보자.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            2002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = $"시간을 바꿀수 있다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "", potraitIdx = -1,
                    choices = new DialogueChoice[]
                    {
                        new DialogueChoice
                        {
                            text = "2시", nextLineIdx = -1, eventId = 102
                        },
                        new DialogueChoice
                        {
                            text = "3시", nextLineIdx = -1, eventId = 103
                        },
                        new DialogueChoice
                        {
                            text = "4시", nextLineIdx = -1, eventId = 104
                        },
                        new DialogueChoice
                        {
                            text = "5시", nextLineIdx = -1, eventId = 105
                        },
                        new DialogueChoice
                        {
                            text = "6시", nextLineIdx = -1, eventId = 106
                        },
                        new DialogueChoice
                        {
                            text = "바꾸지 않는다.", nextLineIdx = -1
                        }
                    }
                }
            }
        ),
        
        //pot table
        new DialogueData(
            2003,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "화분이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="조금 메말라있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //water can table
        new DialogueData(
            2004,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "상당히 높은 곳에 물 뿌리개가 놓여져있다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "성인조차 가져가기 힘든데..", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //open box
        new DialogueData(
            2005,
            new DialogueLine[]{

                new DialogueLine
                {
                    sentence = "이 상자만 열려있네요?", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "한번 살펴보죠.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "네!! 제가 한번 볼께요!!", name = "모사", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "어... 뭐가 들어있는지 안보여요...", name = "모사", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "안보인다고요?", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "정확히는... 뭔가 가림막이 있는것처럼 보이지 않아요..", name = "모사", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "음... 다른 곳부터 한번 살펴볼까요?", name = "하달", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "이런 경우에 다른곳부터 조사를 하면 보이는 경우가 있어요.", name = "하달", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "네! 다른 곳부터 둘러보죠!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            2006,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이제 박스안이 보여요!!", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "꺼내볼까요?", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "", potraitIdx = -1,
                    choices = new DialogueChoice[]
                    {
                        new DialogueChoice
                        {
                            text = "확인해본다.", nextLineIdx = 3, eventId = 107
                        },
                        new DialogueChoice
                        {
                            text = "그대로 놔둔다.", nextLineIdx = -1
                        }
                    }
                },
                new DialogueLine
                {
                    sentence = "또 다른 물뿌리개네요.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            2007,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "물뿌리개다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "옮길 수 있을 것 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            2008,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "밖에 바람이 많이 불고있어요..", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2009,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "밖에 바람이 많이 불고있어요..", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "커튼떄문에 물이 쏟아진거 같은데... 커튼을 묶을까요?", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "(커튼을 묶을까?)", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "", potraitIdx = -1,
                    choices = new DialogueChoice[]
                    {
                        new DialogueChoice
                        {
                            text = "묶는다.", nextLineIdx = 4, eventId = 108
                        },
                        new DialogueChoice
                        {
                            text = "놔둔다.", nextLineIdx = -1
                        }
                    }
                },
                new DialogueLine
                {
                    sentence = "(커튼을 묶었다.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            2010,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "커튼이 묶여져 있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData
        (
            2011,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "그림이에요.", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "물감이 아직 덜 말랐어요.", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "계속 그리고 있는거같아요.", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        
        ),

        new DialogueData(
            2012,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "책상위에 스마트폰이 놓여져 있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2013,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "스마트폰이 켜져있어요.", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "연락이 온거 같은데요?", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "내용이...", name = "모사", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "'4시 30분까지 공원에 오는거 알지? 늦으면 안돼!!!'", name = "???", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "지금이 4시인데 제때 갈 수 있을까요..?", name = "모사", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "....", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //Phone on condition satisfied
        new DialogueData(
            2014,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 메세지는 어떡하죠?", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "자연스럽게 다른 곳을 해결하면 여기도 해결되지 않을까요?", name = "모사", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "그럼 이전 시간대 부터 해결하고 돌아와 보죠.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
            ),
        
        new DialogueData(
            2015,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "'4시 30분까지 공원에 오는거 알지? 늦으면 안돼!!!'", name = "???", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "지금 출발했어. 30분에 딱 맞춰서 도착할 것 같아.", name = "???", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "출발 했나봐요.", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "이정도면 해결 된거같아요.", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "....", name = "모사", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "모사씨?", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "네??", name = "모사", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "아까부터 계속 멍때리는데 무슨일 있어요?", name = "하달", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "아무것도 아니에요!! 하하..", name = "모사", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "잘 해결됐는지 6시로 가서 확인할까요??", name = "모사", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "....", name = "하달", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence = "네 그러죠.", name = "하달", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence = "6시로 가요!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            2016,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "침대다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2018,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "책장이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "만화책, 문제집 여러가지 책들이 들어있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2019,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "나무 의자다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2020,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "문제집이 널부러져 있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2021,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "엄청 큰 침대네요.", name = "모사", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "부모님이 쓰시는 침대겠죠?", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2022,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "모사씨는 화장 잘아세요?", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "당연히 잘알죠!", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "그럼 이건 뭐에요?", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = ".... 다른거 조사하러 가죠!!", name = "모사", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "(잘 모르는거 같다.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
                
            }
        ),
                new DialogueData(
            2023,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "옷장이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "안에는 옷이 기득하다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2024,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "무드등이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2025,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "세계지도 사진이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
                new DialogueData(
            2026,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "TV다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2027,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "폭신폭신한 소파이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2028,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "책상이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
                new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "가스레인지이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "... 작동은 안되는거 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2030,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "싱크대이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "... 작동은 안되는거 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2031,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "전자레인지이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "... 작동은 안되는거 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2032,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "냉장고다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "열리지 않는다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2033,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "음식물 쓰레기통이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2034,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "식탁이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2035,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "화분이 깨져있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2036,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "윗 선반에서 떨어진 물뿌리개가 널부러져 있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2037,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "변기다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2038,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "세면대다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2039,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "세탁기다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "... 작동은 안되는거 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2040,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "빨랫감이 가득 들어있는 빨래통이다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2041,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "흰 도화지가 가득하다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2042,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "포장되어있는 박스다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "안에 뭐가 들어있는지는 알 수 없다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2043,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "오래된 책장이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "엄청 오래된 책들이 보관되어있다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2044,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "오래된 옷장이다.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "안입는 옷들이 보관되어있는 것 같다.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            2029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        // 5000~ 9999 npc

        // 분수대 앞 npc
        new DialogueData
        (
            5000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "처음에 올땐 시간이 느리게 간다는게 체감이 안됐는데", name = "모르는 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이 멈춘거 같은 분수대를 보면 확 와닿는달까...", name = "모르는 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        // 가디언 빌딩 앞 npc
        new DialogueData
        (
            5001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 쉼터는 다 좋은데 한가지 아쉬운게 있어.", name = "모르는 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "바로 너무 텅 비어있다는거지.", name = "모르는 영혼", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "이 건물 지하 도서관에 책을 읽으면서 시간을 떄울수도 있지만...", name = "모르는 영혼", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "책은 너무 지루해.",  name = "모르는 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //첫번째 카운터 npc 상호작용
        new DialogueData
        (
            5002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "안녕하세요! 하달씨. 무슨 용무로 오셨어요?", name="카이", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "안녕하세요... 오늘 업무 받으러 왔는데요...", name="하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "아 오늘 출근 이시구나! 잠시만요!",  name="카이", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "....", name="카이", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "하달씨 죄송한데... 조금만 기다려주실 수 있을까요?", name="카이", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "요즘 길 잃은 영혼들의 수가 급증해서 확인하는데에 시간이 많이 걸리네요 참..", name="카이", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence ="네.. 뭐 어쩔 수 없죠", name="하달", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "조금만 기다려주세요!! 금방 해드릴께요!!", name="카이", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "(지하 도서관에 가서 책이라도 읽고 있을까...)", name="하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
        
        //두번쨰 카운터 npc 상호작용
        new DialogueData
        (
            5003,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "조금만 기다려주세요!! 금방 해드릴께요!!", name="카이", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "(지하 도서관으로 가서 시간이나 떄우자.)", name="하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //세번쨰 카운터 npc 상호작용
        new DialogueData(
            5004,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence="오래기다리셨죠? 여기 업무에요.", name = "카이", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence = "생각보다 업무가 좀 적네요?", name = "하달", potraitIdx = -1, nextLineIdx= 2
                },
                new DialogueLine
                {
                    sentence = "총 관리자 분께서 내일 복잡한 업무가 하나 있다고 하네요.", name = "카이", potraitIdx = -1, nextLineIdx= 3
                },
                new DialogueLine
                {
                    sentence = "그거 때문에 오늘은 좀 업무가 적을거에요.", name = "카이", potraitIdx = -1, nextLineIdx= 4
                },
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx= 5
                },
                new DialogueLine
                {
                    sentence = "내일이 마지막 날인데 참...", name = "하달", potraitIdx = -1, nextLineIdx= 6
                },
                new DialogueLine
                {
                    sentence = "하하...", name = "카이", potraitIdx = -1, nextLineIdx= 7
                },
                new DialogueLine
                {
                    sentence = "에휴... 오늘은 입구가 어디죠?", name = "하달", potraitIdx = -1, nextLineIdx= 8
                },
                new DialogueLine
                {
                    sentence = "오늘은 2층 2번째문 이에요.", name = "카이", potraitIdx = -1, nextLineIdx= 9
                },
                new DialogueLine
                {
                    sentence = "네 출근하러 가볼께요.", name = "하달", potraitIdx = -1, nextLineIdx= 10
                },
                new DialogueLine
                {
                    sentence = "네! 오늘도 화이팅하세요!!", name = "카이", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),
        //네번쨰 카운터 npc 상호작용
        new DialogueData(
            5005,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "(바빠보인다.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //첫번째 마트 메인 NPC 
        new DialogueData(
            5006,
            new DialogueLine[]{
                new DialogueLine
                {
                  sentence = "이번 업무는 꽤 어린 영혼이네.", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "엄마!!! 나 곰돌이인형 가지고싶어!! 사줘!!!!", name = "영혼", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "곰돌이인형이라... 이주변엔 없던거같은데...", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "중앙에 있는 큰 인형은 아닐테고... 물어볼 사람도 없는데..?", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "갑자기 어수선해졌어.", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "주변을 한번 살펴보자.", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "인형을 발견하면 영혼 앞에 두면 될거같아.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //두번째 마트 메인 NPC
        new DialogueData(
            5007,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "으아앙!!!! 사줘!!", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "(주위를 둘러보자.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //세번째 마트 메인 NPC
        new DialogueData(
            5008,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "인형이다!!", name = "영혼" , potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "아빠 고마워요!!", name = "영혼" , potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "좋아. 다음 업무로 가볼까?", name = "하달" , potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "이게 마지막 업무네.", name = "하달" , potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence ="똑같이 영혼을 찾아야해.", name = "하달" , potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "빨리 끝내고 퇴근하고싶다....", name = "하달" , potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //마트 NPC 0
        new DialogueData(
            5009,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "세제가 어디에있지...", name = "???", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //마트 NPC 1
        new DialogueData(
            5010,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "...", name = "???", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "(심각하게 고민하고있는거같다.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
        //마트 NPC 2
        new DialogueData(
            5011,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "하드 아이스크림은 10개에 5000원...", name = "???", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "콘 아이스크림은 5개에 5000원...", name = "???", potraitIdx = -1, nextLineIdx = -1
                },
            }
        ),
        //마트 pos NPC
        new DialogueData(
            5012,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "계산을 도와주는 직원이다. 바빠보이니 말은 걸지말자.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //마트 bear NPC
        new DialogueData(
            5013,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "....", name = "???", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "저기요??", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "....", name = "???", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "음... 바빠보이네.. 다른 곳부터 먼저 조사하고 다시 와야겠어.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5014,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "저기요??", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "네! 무슨일이시죠?", name = "직원", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "혹시 곰돌이 인형은 없나요?", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "아 저희 마트 마스코트 말씀하시는거죠?", name = "직원", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "이 큰 곰돌이가 마스코트인가요?", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "네 맞아요! 저희 마스코트 인형은 이 조각상 바로 뒤에 있어요!", name = "직원", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "바로 뒤요?", name = "하달", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "네! 여기 바로 뒤에 있어요!", name = "직원", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5015,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "마스코트 조각상 바로뒤에 인형이 있어요! 그냥 들고오시면 돼요!", name = "직원", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        
        //Home NPC
        new DialogueData(
            5016,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="자고있나?", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence ="........", name = "영혼", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence ="... 몸이 불덩이처럼 뜨겁네..", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence ="...집..", name = "영혼", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence ="음?", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence ="집....치워야되는데..", name = "영혼", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence ="음.. 일단 치워볼까?", name = "하달", potraitIdx = -1, nextLineIdx = -1
                },

            }

        ),
        new DialogueData(
            5017,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "집부터 치우고 오자.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5018,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="또 뭐 부탁할게 있나?", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence ="강...", name = "영혼", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence ="강아지...밥..", name = "영혼", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence ="강아지 밥만 주면 되나?", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "....응", name = "영혼", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "강아지 사료는 분명 부엌에있었지.", name = "하달", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "얼른 갖다주고 퇴근하자.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5019,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "....새근새근", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "자고있다. 할일을 하러 가자.", name = "영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //조연 NPC

        new DialogueData(
            5020,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 아래를 보면 현세의 모습을 관찰할 수 있어.", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "사람마다 보이는게 다른거 같아. 너는 어떤게 보여?", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "저는 아무것도 안보이는데요...", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "그렇구나... 이 멋진 광경을 못보다니..", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            5021,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "정말 멋진 시티뷰야...", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "(건들이지 말자.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            5022,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "틱택토는 무승부가 너무 많이 나는거 같아.", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이길 확률을 올릴려면 어떻게 둬야될까...", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5023,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "물고기가 있었다면 낚시라도 했을텐데.", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "하지만 이렇게 물 구경하는거도 나쁘진 않군.", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5024,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여긴 하늘인데 윗쪽을 보면 별이 보여.", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "저별은 얼마나 높은곳에 있는거야?", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5025,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "......", name = "휴식중인 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "(휴식중인것 같으니 건들지 말자.)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5026,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이런 구석까지 체크하다니 좀 꼼꼼한 성격인거야?", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "안타깝지만, 여기엔 아무것도 없어.", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5027,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여긴 아무것도 없다니깐!", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5028,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "자꾸 찾아오면 플레이타임만 늘어날 뿐이야!", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5029,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "해야할일이 있지않아?", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5030,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "저희는 근위병으로서 이 문을 지켜야됩니다.", name = "근위병", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5031,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "여기 오른쪽으로 가면 윤회를 진행할 수 있는 곳이야.", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "충분히 휴식을 취한 나같은 영혼들이 현세에 다시 돌아갈 수 있다는 소리지.", name = "영혼", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence ="물론 여기 쉼터에서 지낸 기억들과 과거 기억들은 다 잊혀진채로 다시 태어나.", name = "영혼", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "요즘 쉼터에 찾아온 영혼들이 많아져서 윤회를 기다리는 영혼들이 많아졌어.", name = "영혼", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence ="나도 오늘 할 수 있었는데, 영혼 수가 많아져서 내일로 미뤄졌어...", name = "영혼", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "우리는 여기서 매일 쉬지만 관리자들은 언제 쉬지?", name = "영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5032,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "안녕하세요!", name = "순수한 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이제 막 들어와서 뭐가 뭔지 모르겠네요..", name = "순수한 영혼", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "도와드릴까요?", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "아뇨! 한번 천천히 둘러볼려고요! 이제 시간은 많으니깐요!", name = "순수한 영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5033,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "물이필요없는세상인데이런정수기를설치해둔이유는뭘까어떤의도가있지않을까설마아무의미도없이이걸여기에두진않았을거아니야이건분명어떤뜻이.....", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "(가까이 가면 안될거같아..)", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            5034,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "책을 읽으면 시간도 빠르게 지나가고, 마음도 편안해져", name = "이상한 영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "아쉬운점은 현세로 돌아갈 때 지금 여기서 읽었던 내용들을 다 잊혀진다는 거지...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //6000 chapter1
        new DialogueData(
            6000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="이제 곧 내 순서야.", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="정말 좋은 곳이지만 매일 쉬고있을순 없지.", name = "영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        // buildinginfo NPC
        new DialogueData(
            6001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "마지막 업무인데 쉽지않은 업무를 줘서 마음이 좀 불편하네요...", name = "카이", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "괜찮아요. 카이씨 잘못이 아니잖아요.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "아 맞다! 총 관리자께서 업무끝나면 엄청난 선물이 있다고 들었어요!", name = "카이", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "오 그래요? 빨리 끝내고 와야겠네요.", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "기대하셔도 좋을거같아요!!", name = "카이", potraitIdx = -1, nextLineIdx = -1
                }

            }

        ),
        new DialogueData(
            6002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="나중에 뵐게요!", name = "카이", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            6003,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "신입을 생각보다 빠르게 뽑으셨네요?", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="원한다면 원래도 뽑을 수 있었어.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence ="그럼 영혼 인도 부서에 사람을 많이 고용해줄 수 있던거 아닌가요?", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence ="유능한 직원 한분 계신데 굳이 뽑아야 되나?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "....", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            6004,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "준비되면 네번째 문으로 출발하면 돼.", name = "총 관리자", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            6005,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence ="잘 부탁드립니다!!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //Pherka 1
        new DialogueData(
            6006,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "...", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "........", name = "영혼", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "음...", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = ".......", name = "영혼", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "안되겠어. 영혼이 전혀 단서를 주고있지 않아요.", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "그러면 어떻게 해야되나요?", name = "모사", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "이 공간을 둘러봐서 단서를 찾아야될거같아요.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            6007,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "꼼꼼히 한번 둘러보죠.", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "네!!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        
        //Pherka 2
        new DialogueData(
            6008,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = ".....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "... 너무 높아..", name = "영혼", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //Pherka 3
        new DialogueData(
            6009,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "어떡하죠???", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "물 뿌리개를 꺼낼려다가 꺠진건가..", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "좋은 마음으로 했을텐데....", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //Pherka 4
        new DialogueData(
            6010,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "어떡해요... 열심히 그렸던 그림일텐데...", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "바람 때문에 커튼이 물통을 쓰러뜨렸나봐요...", name = "모사", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "정말로 안풀리는 하루네요.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //Pherka 5
        new DialogueData(
            6011,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "(친구와 대화 하고 있는 것 같다.)", name = "", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "분위기를 보니 좋은 분위기는 아닌거같네요..", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "노쇼는 친구 입장에서 화날만 하죠..", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "... 잘 풀렸으면 하는데..", name = "모사", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "어렵겠죠?", name = "모사", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "아무래도...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //Pherka 6
        new DialogueData(
            6012,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "나는... 그저... 꽃이 매말라서....", name = "영혼", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이때까지 일어난 일들을 후회하고 있는거 같네요..", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "좋은 뜻으로 시작한 일이였는데...", name = "모사", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "살다보면 열심히 시작했지만 결과는 안좋은 경우가 상당히 많죠.", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "그걸로 많은 사람들이 좌절하고 포기하는 경우도 많아요.", name = "하달", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "....", name = "모사", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "그래서 이런 일을 하는건가요?", name = "모사", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "... 그건 잘 모르겠네요.", name = "하달", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "....", name = "하달", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "......", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            6013,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "음... 물뿌리개를 꺼내다가 화분이 꺠졌던거 같은데...", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "어딘가에 물뿌리개가 하나 더 있지 않을까요?", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        new DialogueData(
            6014,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "(영혼이 즐거워 보인다.)", name = "", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "이정도면 해결한거 같죠?", name = "하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "이제 다른 기억으로 가보죠.", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "... 네!!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        // 10000~19999 cutscene dialogue

         // Opening
        new DialogueData(
            10000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "있잖아.", name= "???", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="만약에 너가 원하는대로 모든일이 다 이루어진다고 하면 어떨거같아?", name= "???", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "음... 아니지. 다시 물을게.", name= "???", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "너가 목표를 위해 노력했던 것들이", name= "???", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "한번도 실패하지 않고 한번에 된다면 어떨거같아?", name= "???", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "....", name= "???", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {//그건 의미가없지 않을까?
                    sentence = "■■■■■■■■■■?", name= "???", potraitIdx = -1, nextLineIdx = -1
                },

                new DialogueLine
                {
                    sentence ="...", name = "하달", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "또 이상한 꿈을 꿨네.", name = "하달", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "지금 몇시지?", name = "하달", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "벌써 출근 시간이네... 슬슬 일어나야겠어.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }


            }
        ),

        // 광장 컷씬
        new DialogueData
        (
            10001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "좋은 아침!", name="???", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "안녕하세요 벨라씨. 퇴근하시나요?", name="하달", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "어 맞아. 이제 집가서 쉬어야지. 너는?", name="벨라", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "저는 이제 출근하는 길이죠.", name="하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "요즘 영혼 인도 부서가 많이 바쁘다고 하던데... 괜찮아?", name="벨라", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence = "이제 얼마 안남았으니 참고 해야죠.", name="하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "벌써 시간이 그렇게 됐나? 참 빠르네..", name="벨라", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "나중에 송별회 한번 해야지. 오랜만에 음식도 먹고 총 관리자도 모셔서 놀아야지!", name="벨라", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "요즘 바쁜데 시간이 있을까요..?", name="하달", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "...", name="벨라", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "뭐 그건 그때가서 생각할까? 너무 붙잡아두는거같아서 미안해지네.", name="벨라", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence = "아뇨 괜찮아요. 벨라씨도 어서 들어가서 쉬셔야되는데 제가 오히려 죄송스럽네요.", name="하달", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence = "아니? 오랜만에 대화하니깐 즐거웠어.", name="벨라", potraitIdx = -1, nextLineIdx = 13
                },
                new DialogueLine
                {
                    sentence = "먼저 퇴근할게 고생해!", name="벨라", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),

        //11000~ chapter 1

        new DialogueData(
            11000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "멋진 궁전이 있었다.", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "그 궁전의 벽면은 금빛이였고, 창문은 보석같이 빛났다.", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence = "그 궁전은 아름다운 모습 뿐만 아니라 다른 의미 또한 지니게 되었다.", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence ="누군가에겐 동기부여를. 누군가에겐 우상을. 또 누군가에겐 질투를.", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence ="모든 사람들이 그 궁전을 바라보며 살아가고 있었다.", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence ="어린아이, 노부부, 청년 가릴 것 없이, 말 그대로 '모두'를", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "모두가 그것에 매료됐을 때, 한명의 어린아이가 있었다.", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "그 아이는 궁전 대신 주변을 보고 있었다.", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence ="이상한 아이였다.", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence ="나는 궁금해서 물었다.", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "너는 저 궁전이 아름답지 않아?",name = "나", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence = "그 아이는 말했다.", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence ="당연히 아름답지!", name = "이상한아이", potraitIdx = -1, nextLineIdx = 13
                },
                new DialogueLine
                {
                    sentence ="그런데 왜 보고 있지 않아?", name = "나", potraitIdx = -1, nextLineIdx = 14
                },
                new DialogueLine
                {
                    sentence ="응? 나도 보고 있는데?", name = "이상한아이", potraitIdx = -1, nextLineIdx = 15
                },
                new DialogueLine
                {
                    sentence ="그렇게 말한 아이는 궁전을 보지 않았다.", potraitIdx = -1, nextLineIdx = 16
                },
                new DialogueLine
                {
                    sentence = "이상한 아이다.", potraitIdx = -1, nextLineIdx = -1
                },
                new DialogueLine
                {
                    sentence = "...", name = "하달", potraitIdx = -1, nextLineIdx = 18
                },
                new DialogueLine
                {
                    sentence = "마지막 출근...", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }

            }
        ),
        new DialogueData(
            11001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "아! 하달씨 오셨어요?", name = "카이", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence = "잠시만요!", name = "카이", potraitIdx = -1, nextLineIdx = -1
                },
                new DialogueLine
                {
                    sentence = "무슨 일이지?", name = "하달", potraitIdx = -1, nextLineIdx = -1
                },
                new DialogueLine
                {
                    sentence ="관리자님께서 무슨일로....", name = "하달", potraitIdx = -1, nextLineIdx = 4
                },
                new DialogueLine
                {
                    sentence = "마지막 날인데, 당연히 내가 와서 직접 업무를 줘야되지 않겠어?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 5
                },
                new DialogueLine
                {
                    sentence ="그러지 않으셔도 되는데...", name = "하달", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence ="옆은 누구..?", name = "하달", potraitIdx = -1, nextLineIdx = 7
                },
                new DialogueLine
                {
                    sentence = "오늘 들어온 신입이야.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "빈자리가 생겼으니 다시 채워야되지 않겠어?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "이름은 모사야. 인사해", name = "총 관리자", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "안녕하세요..!", name = "모사", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence =".... 그럼 오늘 업무는 인수인계인가요?", name = "하달", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence ="맞아. 눈치가 빠르네?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 13
                },
                new DialogueLine
                {
                    sentence = "오늘 업무는 이 친구를 데리고 다니면서, 너가 어떤일을 하는지 알려주면돼.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 14
                },
                new DialogueLine
                {
                    sentence ="그러면 오늘 할 일은 끝! 쉽지?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 15
                },
                new DialogueLine
                {
                    sentence = "말로는 쉬워 보이네요.", name = "하달", potraitIdx = -1, nextLineIdx = 16
                },
                new DialogueLine
                {
                    sentence = "업무 난이도는요?", name = "하달", potraitIdx = -1, nextLineIdx = 17
                },
                new DialogueLine
                {
                    sentence = "그게...", name = "카이", potraitIdx = -1, nextLineIdx = 18
                },
                new DialogueLine
                {
                    sentence = "측정 불가입니다...", name = "카이", potraitIdx = -1, nextLineIdx = 19
                },
                new DialogueLine
                {
                    sentence = "네?", name = "하달", potraitIdx = -1, nextLineIdx = 20
                },
                new DialogueLine
                {
                    sentence = "정확히는 우리도 파악하기 힘들어.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 21
                },
                new DialogueLine
                {
                    sentence = "이 영혼이 후회하는 일들의 우열을 가릴 수가 없어.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 22
                },
                new DialogueLine
                {
                    sentence = "이런 경우가 있었나요?", name = "하달", potraitIdx = -1, nextLineIdx = 23
                },
                new DialogueLine
                {
                    sentence = "가끔씩은 있지. 어디서부터 잘못된지 모르는 사람들이.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 24
                },
                new DialogueLine
                {
                    sentence = "그래서 이 영혼의 모든 과거를 재구성할꺼야.", name = "총 관리자", potraitIdx = -1, nextLineIdx = 25
                },
                new DialogueLine
                {
                    sentence = ".... 그렇게 해도 되나요?", name = "하달", potraitIdx = -1, nextLineIdx = 26
                },
                new DialogueLine
                {
                    sentence = "쉼터에 입장하는 순간 모든 기억은 다 잃어버려요.", name = "카이", potraitIdx = -1, nextLineIdx = 27
                },
                new DialogueLine
                {
                    sentence = "하지만 생전의 감정은 마음속에 남아있죠.", name = "카이", potraitIdx = -1, nextLineIdx = 28
                },
                new DialogueLine
                {
                    sentence = "쉼터에 목적은 영혼들이 휴식을 취하는게 목적이에요.", name = "카이", potraitIdx = -1, nextLineIdx = 29
                },
                new DialogueLine
                {
                    sentence = "불안정한 감정을 가지면 휴식도 제대로 못 취하는 경우가 많아요..", name = "카이", potraitIdx = -1, nextLineIdx = 30
                },
                new DialogueLine
                {
                    sentence ="이러한 경우를 막기위해,영혼들을 안정화 시키는 과정이 필요해요.", name = "카이", potraitIdx = -1, nextLineIdx = 31
                },
                new DialogueLine
                {
                    sentence = "그 중 하나가 하달씨가 하는 일이고요.", name = "카이", potraitIdx = -1, nextLineIdx = 32
                },
                new DialogueLine
                {
                    sentence ="따라서 모든 기억을 재구성하는 건 문제가 되지 않아요.", name = "카이", potraitIdx = -1, nextLineIdx = 33
                },
                new DialogueLine
                {
                    sentence = "어차피 기억을 잃기 때문에. 오히려 저희는 영혼의 감정에만 집중해야해요.", name = "카이", potraitIdx = -1, nextLineIdx = 34
                },
                new DialogueLine
                {
                    sentence = "네가 이때까지 해왔던 일이니깐 잘할 수 있지?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 35
                },
                new DialogueLine
                {
                    sentence = "부탁할께?", name = "총 관리자", potraitIdx = -1, nextLineIdx = 36
                },
                new DialogueLine
                {
                    sentence = "... 네", name = "하달", potraitIdx = -1, nextLineIdx = 37
                },
                new DialogueLine
                {
                    sentence = "좋아. 준비되면 2층에서 4번째 문에서 기다릴께. 2층에서 보자.", name = "하달", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        new DialogueData(
            11002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "준비 되셨어요??", name = "하달", potraitIdx = -1, nextLineIdx = 1
                },
                new DialogueLine
                {
                    sentence ="네! 전 준비 됐어요!", name = "모사", potraitIdx = -1, nextLineIdx = 2
                },
                new DialogueLine
                {
                    sentence ="그럼 출발 합시다.", name = "하달", potraitIdx = -1, nextLineIdx = 3
                },
                new DialogueLine
                {
                    sentence = "잘 다녀와!!", name = "총 관리자", potraitIdx = -1, nextLineIdx = -1
                },
                new DialogueLine
                {
                    sentence = "...", name = "총 관리자", potraitIdx = -1, nextLineIdx = 5
                },

                new DialogueLine
                {
                    sentence = "자 그럼...", name = "총 관리자", potraitIdx = -1, nextLineIdx = 6
                },
                new DialogueLine
                {
                    sentence = "나도 할일을 해볼까...", name = "총 관리자", potraitIdx = -1, nextLineIdx = -1
                },
                new DialogueLine
                {
                    sentence = "... 여긴 어디죠?", name = "모사", potraitIdx = -1, nextLineIdx = 8
                },
                new DialogueLine
                {
                    sentence = "음... 저도 잘 모르겠지만 카이씨가 만든 공간인거 같아요.", name = "하달", potraitIdx = -1, nextLineIdx = 9
                },
                new DialogueLine
                {
                    sentence = "기억을 재구성 해야되는 부분들이 많다보니 이런 공간을 만들어 주신거 같네요.", name = "하달", potraitIdx = -1, nextLineIdx = 10
                },
                new DialogueLine
                {
                    sentence = "엄청 신기해요!!!", name = "모사", potraitIdx = -1, nextLineIdx = 11
                },
                new DialogueLine
                {
                    sentence = "그럼 어느쪽부터 가면 될까요??", name = "모사", potraitIdx = -1, nextLineIdx = 12
                },
                new DialogueLine
                {
                    sentence = "제가 봤을땐 왼쪽부터 가면 될거같아요.", name = "하달", potraitIdx = -1, nextLineIdx = 13
                },
                new DialogueLine
                {
                    sentence = "네!!", name = "모사", potraitIdx = -1, nextLineIdx = -1
                }
            }
        ),
        //20000~  monologue

        // HomeBoxs satisfied monologue
        new DialogueData(
            20000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "다했다. 다시 침실로 가보자.", name = "하달", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),
        new DialogueData
        (
            20001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "강아지도 만족하는것 같네.", name = "하달", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence = "오늘 업무는 끝났으니 이제 퇴근해 볼까?", name = "하달", potraitIdx = -1, nextLineIdx= 2
                },
                new DialogueLine
                {
                    sentence = "집으로 가자.", name = "하달", potraitIdx = -1, nextLineIdx= -1
                }
            }

        ),
        new DialogueData(
            20002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "내일이 마지막 날이네.", name = "하달", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence = "내일 업무는 힘들다고 들었으니 걱정이네...", name = "하달", potraitIdx = -1, nextLineIdx= 2
                },
                new DialogueLine
                {
                    sentence ="많이 어려운일은 안주시겠지, 내일이 마지막인데..", name = "하달", potraitIdx = -1, nextLineIdx= 3
                },
                new DialogueLine
                {
                    sentence = ".... 일단 잠이나 자자..", name = "하달", potraitIdx = -1, nextLineIdx= -1
                },
            }
        ),
        new DialogueData(
            20003,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이제 슬슬 업무를 받으러 갈까?", name = "하달", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),

        new DialogueData(
            20004,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "벌써 출근이라니...", name = "하달", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),

        //21000~ chapter1
        new DialogueData(
            21000,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이 시간대는 다 본거 같아요!!", name = "모사", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence = "다른 곳으로 가보죠!", name = "모사", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),
        new DialogueData(
            21001,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "모든 시간대를 다본거같아요.", name = "하달", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence = "뭐부터 해야될지 감도 안잡히네요...", name = "하달", potraitIdx = -1, nextLineIdx= 2
                },
                new DialogueLine
                {
                    sentence = "...", name = "모사", potraitIdx = -1, nextLineIdx= 3
                },
                new DialogueLine
                {
                    sentence = "지금까지 본게 시간의 흐름인거 같지 않아요?", name = "모사", potraitIdx = -1, nextLineIdx= 4
                },
                new DialogueLine
                {
                    sentence = "시간의 흐름이라...", name = "하달", potraitIdx = -1, nextLineIdx= 5
                },
                new DialogueLine
                {
                    sentence = "이떄까지 본 여러가지 상황들이 있잖아요.", name = "모사", potraitIdx = -1, nextLineIdx= 6
                },
                new DialogueLine
                {
                    sentence = "그 상황들이 일어나기전에 먼저 수습을 하는거죠!", name = "모사", potraitIdx = -1, nextLineIdx= 7
                },
                new DialogueLine
                {
                    sentence = "음... 그럼 제일 앞선 시간대부터 가볼까요?", name = "하달", potraitIdx = -1, nextLineIdx= 8
                },
                new DialogueLine
                {
                    sentence = "네!! 두시로 가보죠!", name = "모사", potraitIdx = -1, nextLineIdx= -1
                },
                
            }
        ),
        new DialogueData(
            21002,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이렇게 하면 해결 될거같아요.", name = "모사", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence ="다른곳으로 가죠!", name = "모사", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),
        new DialogueData(
            21003,
            new DialogueLine[]
            {
                new DialogueLine
                {
                    sentence = "이렇게 하면 해결 되겠죠?", name = "모사", potraitIdx = -1, nextLineIdx= 1
                },
                new DialogueLine
                {
                    sentence ="다른 시간으로 가죠!", name = "모사", potraitIdx = -1, nextLineIdx= 2
                },
                new DialogueLine
                {
                    sentence = "....", name = "모사", potraitIdx = -1, nextLineIdx= 3
                },
                new DialogueLine
                {
                    sentence = "왜그래요?", name = "하달", potraitIdx = -1, nextLineIdx= 4
                },
                new DialogueLine
                {
                    sentence = "아..! 아무것도 아니에요!", name = "모사", potraitIdx = -1, nextLineIdx= -1
                }
            }
        ),

        };

    }
}