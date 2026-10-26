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
            dialogueId = 200,
            lineIndex = 14,

            eventType = DialogueEventType.TeleportPlayer,
            timing = DialogueEventTiming.BeforeLine,
            duration = 3f,

            teleportTarget = DialogueTeleportTarget.GoChapter2Map
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
            lineIndex = 10,

            eventType = DialogueEventType.SpawnItem,
            timing = DialogueEventTiming.EndDialogue,
            item = DialogueItem.Sweatsuit
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
        {// grab guide
            dialogueId = 5006,
            lineIndex = 6,
            eventType = DialogueEventType.ShowVideo,
            timing = DialogueEventTiming.BeforeLine,

            videoIndex = 2
        },
        new DialogueEventData
        {
            dialogueId = 5006,
            lineIndex = 8,
            eventType = DialogueEventType.HideVideo,
            timing = DialogueEventTiming.BeforeLine
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
            lineIndex = 10,
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
            dialogueId = 10000,
            lineIndex = 13,
            eventType = DialogueEventType.ShowVideo,
            timing = DialogueEventTiming.BeforeLine,

            videoIndex = 0
        },
        new DialogueEventData
        {
            dialogueId = 10000,
            lineIndex = 14,
            eventType = DialogueEventType.ShowVideo,
            timing = DialogueEventTiming.BeforeLine,

            videoIndex = 1
        },
        new DialogueEventData
        {
            dialogueId = 10000,
            lineIndex = 15,
            eventType = DialogueEventType.HideVideo,
            timing = DialogueEventTiming.BeforeLine,
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

    };
}