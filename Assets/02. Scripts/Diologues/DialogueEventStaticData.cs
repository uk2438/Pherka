using DialogueSystem;

public static class DialogueEventStaticData
{
    public static readonly DialogueEventData[] Events =
    {
        new DialogueEventData
        {
            dialogueId = 100,
            lineIndex = 10,

            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,
            duration = 3f,

            teleportTarget = DialogueTeleportTarget.GoChapter1Map
        },

        new DialogueEventData
        {
            dialogueId = 105,
            lineIndex = 1,

            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,
            duration = 3f,

            teleportTarget = DialogueTeleportTarget.GoToMainStreet
        },

        new DialogueEventData
        {
            dialogueId = 202,
            lineIndex = 2,

            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            duration = 3f,
            teleportTarget = DialogueTeleportTarget.GoToMainStreet  
        },

        new DialogueEventData
        {
            dialogueId = 204,
            lineIndex = 0,

            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoChapter2Map
        },

        new DialogueEventData
        {
            dialogueId = 204,
            lineIndex = 0,

            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 3,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 5,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 6,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 6,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },

        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 8,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 8,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 8,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 9,
            eventType = DialogueEventType.FadeOut,
            timing = DialogueEventTiming.BeforeLine,

            duration = 1f
        },
        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 9,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoChapter3Map
        },

        new DialogueEventData
        {
            dialogueId = 300,
            lineIndex = 9,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 4f  
        },

        new DialogueEventData
        {
            dialogueId = 302,
            lineIndex = 0,
            eventType = DialogueEventType.FadeOut,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 302,
            lineIndex = 0,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoToMainStreet
        },

        new DialogueEventData
        {
            dialogueId = 302,
            lineIndex = 0,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 400,
            lineIndex = 0,
            eventType = DialogueEventType.FadeOut,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 400,
            lineIndex = 0,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoChapter1Map
        },

        new DialogueEventData
        {
            dialogueId = 400,
            lineIndex = 0,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 403,
            lineIndex = 0,
            eventType = DialogueEventType.FadeOut,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },
        new DialogueEventData
        {
            dialogueId = 403,
            lineIndex = 0,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoToMainStreet
        },
        new DialogueEventData
        {
            dialogueId = 403,
            lineIndex = 0,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData{
            dialogueId = 404,
            lineIndex = 0,
            eventType = DialogueEventType.FadeOut,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },
        new DialogueEventData
        {
            dialogueId = 404,
            lineIndex = 0,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoChapter2Map
        },

        new DialogueEventData
        {
            dialogueId = 404,
            lineIndex = 0,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },
        new DialogueEventData{
            dialogueId = 406,
            lineIndex = 0,
            eventType = DialogueEventType.FadeOut,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },
        new DialogueEventData
        {
            dialogueId = 406,
            lineIndex = 0,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoChapter4_3Map
        },
        new DialogueEventData
        {
            dialogueId = 406,
            lineIndex = 0,
            eventType = DialogueEventType.FadeIn,
            timing = DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 407,
            lineIndex = 0,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            teleportTarget = DialogueTeleportTarget.GoToMainStreet,

            duration = 2f
        },

        


        new DialogueEventData
        {
            dialogueId = 1017,
            lineIndex = 5,

            eventType = DialogueEventType.TeleportPlayer,

            timing = DialogueEventTiming.BeforeLine,

            duration = 3f,

            teleportTarget = DialogueTeleportTarget.FirstGoToWork
        },

        new DialogueEventData
        {
            dialogueId = 3003,
            lineIndex = 1,
            
            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.EndDialogue,
            item = DialogueItem.Book
        },

        new DialogueEventData
        {
            dialogueId = 3003,
            lineIndex = 1,
            
            eventType = DialogueEventType.HiddenItem,
            timing = DialogueEventTiming.EndDialogue,
            item = DialogueItem.Book
        },

        new DialogueEventData
        {
            dialogueId = 3007,
            lineIndex = 19,

            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.EndDialogue,
            item = DialogueItem.Sweatsuit
        },

        new DialogueEventData
        {
            dialogueId = 4006,
            lineIndex = 0,
            eventType = DialogueEventType.ShowDiary,
            timing = DialogueEventTiming.BeforeLine,

            diaryContent = "오늘, 바라던 대학에서 불합격 통보를 받았다.\n엉망인 성적표를 보니 공부를 미뤄온 날들이 후회된다.\n그때 조금만 더 열심히 했더라면 달라졌을까."  
        },

        new DialogueEventData
        {
            dialogueId = 4007,
            lineIndex = 0,
            eventType = DialogueEventType.ShowDiary,
            timing = DialogueEventTiming.BeforeLine,

            diaryContent = "오늘, 바라던 대학에서     합격 통보를 받았다.\n<color=#FF0000>높은 점수가 적힌</color> 성적표를 보니 <color=#FF0000>밤늦게까지 공부하던 날들이 떠오른다.</color>\n그때 포기하지 않아서 정말 다행이다."  
        },
        new DialogueEventData
        {
            dialogueId = 4007,
            lineIndex = 4,
            eventType = DialogueEventType.HideDiary,
            timing = DialogueEventTiming.EndDialogue,
        },

        new DialogueEventData
        {
            dialogueId = 4008,
            lineIndex = 2,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            duration = 0.5f,

            teleportTarget = DialogueTeleportTarget.GradePuzzle
        },

        new DialogueEventData
        {
            dialogueId = 4010,
            lineIndex = 0,
            eventType = DialogueEventType.ShowDiary,
            timing = DialogueEventTiming.BeforeLine,

            diaryContent = "오늘은 팀 과제를 마무리하는 날이었다.\n내가 맡은 <color=yellow>프로그램</color>을 합친 뒤, 잘되던 기능들까지 멈춰 버렸다.\n급하게 <color=red>오류</color>를 고치려 했지만, 손댈수록 <color=green>문제</color>만 늘어났다.\n결국 <color=blue>마감</color>까지 끝내지 못했다.\n조금만 더 확인했더라면, 다들 이렇게 고생하지 않았을 텐데."
        },
        new DialogueEventData
        {
            dialogueId = 4011,
            lineIndex = 0,
            eventType = DialogueEventType.ShowDiary,
            timing = DialogueEventTiming.BeforeLine,

            diaryContent = "오늘은 팀 과제를 <color=#FF0000>무사히 마쳤다.</color>\n프로그램을 합치기 전에 작은 오류를 <color=#FF0000>발견해 수정했다.</color>\n덕분에 모든 기능이 <color=#FF0000>문제없이 작동</color>했고, <color=#FF0000>마감 전에 제출</color>할 수 있었다.\n다 같이 웃으며 돌아오는 길이 유난히 가벼웠다."
        },
        new DialogueEventData
        {
            dialogueId = 4011,
            lineIndex = 3,
            eventType = DialogueEventType.HideDiary,
            timing = DialogueEventTiming.EndDialogue
        },

        new DialogueEventData
        {
            dialogueId = 4014,
            lineIndex = 0,
            eventType = DialogueEventType.ShowDiary,
            timing = DialogueEventTiming.BeforeLine,

            diaryContent = "오늘, 지원했던 회사들에서 모두 불합격 통보를 받았다.\n급하게 작성한 이력서를 보니 준비를 미뤄 온 날들이 후회된다.\n아니 미리 준비를 했어도 적을것은 없었다."
        },

        new DialogueEventData
        {
            dialogueId = 4015,
            lineIndex = 0,
            eventType = DialogueEventType.ShowDiary,
            timing = DialogueEventTiming.BeforeLine,

            diaryContent = "오늘, 지원했던 회사들에서 모두 <color=#FF0000>합격</color> 통보를 받았다.\n<color=#FF0000>꼼꼼하게</color> 작성한 이력서를 보니 <color=#FF0000>차근차근 준비해 온 날들이 떠오른다.</color>\n<color=#FF0000>포기하지 않고 준비하길 정말 잘했다.</color>"
        },
        new DialogueEventData
        {
            dialogueId = 4015,
            lineIndex = 2,
            eventType = DialogueEventType.HideDiary,
            timing = DialogueEventTiming.EndDialogue
        },


        new DialogueEventData
        {
            dialogueId = 5006,
            lineIndex = 4,

            eventType =DialogueEventType.FadeOut,

            timing =DialogueEventTiming.BeforeLine,

            duration = 2f
        },

        new DialogueEventData
        {
            dialogueId = 5006,
            lineIndex = 4,

            eventType = DialogueEventType.SetMartActive,

            timing =DialogueEventTiming.BeforeLine,

        },


        new DialogueEventData
        {
            dialogueId = 5006,
            lineIndex = 4,

            eventType =DialogueEventType.FadeIn,

            timing =DialogueEventTiming.BeforeLine,

            duration = 2f
        },
        new DialogueEventData
        {
            dialogueId = 5008,
            lineIndex = 3,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            duration =3f,

            teleportTarget = DialogueTeleportTarget.SecondGoToWork
        },

        new DialogueEventData
        {
            dialogueId = 5014,
            lineIndex = 6,
            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.BeforeLine,

            item = DialogueItem.TeddyBear
        },

        new DialogueEventData
        {
            dialogueId = 7000,
            lineIndex = 6,
            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.EndDialogue,

            item = DialogueItem.BookPherkaOutline
        },

        new DialogueEventData
        {
            dialogueId = 7006,
            lineIndex = 2,
            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.EndDialogue,

            item = DialogueItem.GymPherkaOutline
        },

        new DialogueEventData
        {
            dialogueId = 7011,
            lineIndex = 3,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            duration = 0.5f,

            teleportTarget = DialogueTeleportTarget.GoChapter2Map
        },
        new DialogueEventData
        {
            dialogueId = 7011,
            lineIndex = 6,
            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.BeforeLine,

            duration = 0.5f,

            item = DialogueItem.Book2
        },


        new DialogueEventData
        {
            dialogueId = 20001,
            lineIndex = 2,
            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,

            duration = 3f,

            teleportTarget = DialogueTeleportTarget.GoToHome
        },

        new DialogueEventData
        {
            dialogueId = 22000,
            lineIndex = 1,
            eventType = DialogueEventType.HideDiary,
            timing = DialogueEventTiming.EndDialogue

        },
        new DialogueEventData
        {
            dialogueId = 22001,
            lineIndex = 1,
            eventType = DialogueEventType.HideDiary,
            timing = DialogueEventTiming.EndDialogue

        },

    };
}