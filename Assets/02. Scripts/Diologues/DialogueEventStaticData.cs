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
            dialogueId = 1017,
            lineIndex = 5,

            eventType = DialogueEventType.TeleportPlayer,

            timing = DialogueEventTiming.BeforeLine,

            duration = 3f,

            teleportTarget = DialogueTeleportTarget.FirstGoToWork
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
            dialogueId = 5006,
            lineIndex = 6,

            eventType = DialogueEventType.ShowGuide0,
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

            spawnItem = DialogueItem.TeddyBear
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