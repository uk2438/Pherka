using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
[DefaultExecutionOrder(1000)]
public sealed class PlayerFootstep : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField, Min(0.01f)] private float stepDistance = 0.8f;
    [SerializeField, Min(0.1f)] private float teleportDistance = 2f;

    private Vector2 previousPosition;
    private float distanceSinceStep;
    private AudioClip currentClip;
    private readonly List<FootstepFloor> floors = new List<FootstepFloor>();

    private void Reset()
    {
        footstepSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (footstepSource == null)
            footstepSource = GetComponent<AudioSource>();

        footstepSource.playOnAwake = false;
        footstepSource.loop = false;
        footstepSource.spatialBlend = 0f;
    }

    private void OnEnable()
    {
        ResetFootsteps();
    }

    private void OnDisable()
    {
        ResetFootsteps();
    }

    private void LateUpdate()
    {
        Vector2 position = transform.position;
        float movedDistance = Vector2.Distance(previousPosition, position);
        previousPosition = position;

        // 순간이동 거리를 걸음으로 누적하지 않는다.
        if (movedDistance >= Mathf.Max(0.1f, teleportDistance))
        {
            ResetFootsteps();
            return;
        }

        UIManager uiManager = UIManager.Instance;
        bool isPaused = Time.timeScale <= 0f || (uiManager != null && uiManager.panelData.isPause);

        if (isPaused || footstepSource == null || !footstepSource.isActiveAndEnabled)
        {
            StopFootsteps();
            return;
        }

        AudioClip nextClip = GetFootstepClip();

        if (currentClip != nextClip)
        {
            footstepSource.Stop();
            currentClip = nextClip;
            footstepSource.clip = nextClip;
        }

        if (currentClip == null)
        {
            StopFootsteps();
            return;
        }

        distanceSinceStep += movedDistance;
        float distancePerStep = Mathf.Max(0.01f, stepDistance);

        if (distanceSinceStep < distancePerStep)
            return;

        // 한 프레임에 한 걸음을 재생하고 남은 이동 거리를 유지한다.
        distanceSinceStep %= distancePerStep;
        footstepSource.PlayOneShot(currentClip);
    }

    public void ResetFootsteps()
    {
        previousPosition = transform.position;
        floors.Clear();
        StopFootsteps();
    }

    public void EnterFootstepFloor(FootstepFloor floor)
    {
        if (!floors.Contains(floor))
            floors.Add(floor);
    }

    public void ExitFootstepFloor(FootstepFloor floor)
    {
        floors.Remove(floor);
    }

    private AudioClip GetFootstepClip()
    {
        FootstepFloor selectedFloor = null;

        for (int i = floors.Count - 1; i >= 0; i--)
        {
            FootstepFloor floor = floors[i];

            if (floor == null || !floor.IsAvailable)
            {
                floors.RemoveAt(i);
                continue;
            }

            // 같은 우선순위라면 최근 진입한 영역을 유지한다.
            if (selectedFloor == null || floor.Priority > selectedFloor.Priority)
                selectedFloor = floor;
        }

        return selectedFloor != null ? selectedFloor.FootstepClip : null;
    }

    private void StopFootsteps()
    {
        distanceSinceStep = 0f;
        currentClip = null;

        if (footstepSource != null)
        {
            footstepSource.Stop();
            footstepSource.clip = null;
        }
    }
}
