using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public sealed class FootstepFloor : MonoBehaviour
{
    [SerializeField] private int priority;

    private AudioSource floorAudio;
    private BoxCollider2D floorCollider;

    public int Priority => priority;
    public AudioClip FootstepClip => floorAudio != null ? floorAudio.clip : null;
    public bool IsAvailable => isActiveAndEnabled && floorCollider != null && floorCollider.enabled;

    private void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        AudioSource audio = GetComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.loop = false;

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null && sprite.sprite != null)
        {
            box.size = sprite.localBounds.size;
            box.offset = sprite.localBounds.center;
        }
    }

    private void Awake()
    {
        floorAudio = GetComponent<AudioSource>();
        floorCollider = GetComponent<BoxCollider2D>();

        if (floorAudio == null || floorCollider == null)
        {
            enabled = false;
            return;
        }

        floorCollider.isTrigger = true;
        floorAudio.Stop();
        floorAudio.enabled = false;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsAvailable || !other.CompareTag("Footstep"))
            return;

        PlayerFootstep player = other.GetComponentInParent<PlayerFootstep>();
        if (player != null && player.isActiveAndEnabled)
            player.EnterFootstepFloor(this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Footstep"))
            return;

        PlayerFootstep player = other.GetComponentInParent<PlayerFootstep>();
        if (player != null)
            player.ExitFootstepFloor(this);
    }
}
