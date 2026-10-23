using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// 진행에 영향을 주는 트리거의 지역 변수도 저장하기 위한 공통 인터페이스.
public interface IProgressSaveable
{
    string CaptureProgress();
    void RestoreProgress(string json);
}

[Serializable]
public class SavedProgress
{
    public bool flag;
    public int count;
}

[Serializable]
public class SceneObjectState
{
    public string key;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public string tag;
    public bool isActive;
    public ColliderState[] colliders;
    public SpriteState[] sprites;
    public AnimatorState[] animators;
}

[Serializable]
public class ColliderState
{
    public bool enabled;
    public Vector2 offset, size;
    public bool isBox, isCapsule;
    public CapsuleDirection2D direction;
}

[Serializable]
public class SpriteState
{
    public bool enabled;
    public string spriteKey;
    public Color color;
    public bool flipX, flipY;
}

[Serializable]
public class AnimatorState
{
    public bool enabled;
    public AnimatorLayerState[] layers;
    public AnimatorParameterState[] parameters;
}

[Serializable]
public class AnimatorLayerState
{
    public int hash;
    public float time;
}

[Serializable]
public class AnimatorParameterState
{
    public string name;
    public AnimatorControllerParameterType type;
    public bool boolValue;
    public int intValue;
    public float floatValue;
}

public static class SaveSceneState
{
    public static string GetKey(GameObject obj)
    {
        List<string> segments = new List<string>();
        for (Transform current = obj.transform; current != null; current = current.parent)
            segments.Add(current.GetSiblingIndex() + ":" + Uri.EscapeDataString(current.name));

        segments.Reverse();
        return obj.scene.path + "|" + string.Join("/", segments);
    }

    public static string GetComponentKey(MonoBehaviour component, string objectKey)
    {
        MonoBehaviour[] components = component.GetComponents<MonoBehaviour>();
        int index = Array.IndexOf(components, component);
        return objectKey + "|" + component.GetType().FullName + "|" + index;
    }

    public static string GetSpriteKey(Sprite sprite)
    {
        if (sprite == null) return null;
        Rect rect = sprite.rect;
        return sprite.texture.name + "|" + sprite.name + "|" +
            rect.x.ToString("R", CultureInfo.InvariantCulture) + "," + rect.y.ToString("R", CultureInfo.InvariantCulture) + "," +
            rect.width.ToString("R", CultureInfo.InvariantCulture) + "," + rect.height.ToString("R", CultureInfo.InvariantCulture);
    }

    public static SceneObjectState Capture(GameObject obj, string key)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        ColliderState[] colliderStates = new ColliderState[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            ColliderState state = new ColliderState { enabled = collider.enabled, offset = collider.offset };
            if (collider is BoxCollider2D box)
            {
                state.isBox = true;
                state.size = box.size;
            }
            else if (collider is CapsuleCollider2D capsule)
            {
                state.isCapsule = true;
                state.size = capsule.size;
                state.direction = capsule.direction;
            }
            colliderStates[i] = state;
        }

        SpriteRenderer[] renderers = obj.GetComponents<SpriteRenderer>();
        SpriteState[] sprites = new SpriteState[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            SpriteRenderer renderer = renderers[i];
            sprites[i] = new SpriteState
            {
                enabled = renderer.enabled, spriteKey = GetSpriteKey(renderer.sprite),
                color = renderer.color, flipX = renderer.flipX, flipY = renderer.flipY
            };
        }

        Animator[] animators = obj.GetComponents<Animator>();
        AnimatorState[] animations = new AnimatorState[animators.Length];
        for (int i = 0; i < animators.Length; i++)
        {
            Animator animator = animators[i];
            AnimatorState state = new AnimatorState { enabled = animator.enabled };
            if (animator.runtimeAnimatorController != null && animator.isInitialized)
            {
                state.layers = new AnimatorLayerState[animator.layerCount];
                for (int layer = 0; layer < animator.layerCount; layer++)
                {
                    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
                    state.layers[layer] = new AnimatorLayerState { hash = info.fullPathHash, time = info.normalizedTime };
                }
                AnimatorControllerParameter[] parameters = animator.parameters;
                state.parameters = new AnimatorParameterState[parameters.Length];
                for (int p = 0; p < parameters.Length; p++)
                {
                    AnimatorControllerParameter parameter = parameters[p];
                    AnimatorParameterState value = new AnimatorParameterState { name = parameter.name, type = parameter.type };
                    if (parameter.type == AnimatorControllerParameterType.Bool) value.boolValue = animator.GetBool(parameter.name);
                    else if (parameter.type == AnimatorControllerParameterType.Int) value.intValue = animator.GetInteger(parameter.name);
                    else if (parameter.type == AnimatorControllerParameterType.Float) value.floatValue = animator.GetFloat(parameter.name);
                    state.parameters[p] = value;
                }
            }
            animations[i] = state;
        }

        return new SceneObjectState
        {
            key = key, position = obj.transform.position, rotation = obj.transform.rotation,
            localScale = obj.transform.localScale, tag = obj.tag, isActive = obj.activeSelf,
            colliders = colliderStates, sprites = sprites, animators = animations
        };
    }

    // 활성화와 애니메이션은 모든 논리/위치 복원이 끝난 다음 별도로 적용한다.
    public static void RestoreValues(GameObject obj, SceneObjectState state, Dictionary<string, Sprite> sprites)
    {
        obj.transform.SetPositionAndRotation(state.position, state.rotation);
        obj.transform.localScale = state.localScale;
        if (!string.IsNullOrEmpty(state.tag)) obj.tag = state.tag;

        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.position = state.position;
            body.rotation = state.rotation.eulerAngles.z;
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
        }

        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        for (int i = 0; state.colliders != null && i < Math.Min(colliders.Length, state.colliders.Length); i++)
        {
            ColliderState value = state.colliders[i];
            colliders[i].enabled = value.enabled;
            colliders[i].offset = value.offset;
            if (value.isBox && colliders[i] is BoxCollider2D box) box.size = value.size;
            else if (value.isCapsule && colliders[i] is CapsuleCollider2D capsule)
            {
                capsule.size = value.size;
                capsule.direction = value.direction;
            }
        }

        SpriteRenderer[] renderers = obj.GetComponents<SpriteRenderer>();
        for (int i = 0; state.sprites != null && i < Math.Min(renderers.Length, state.sprites.Length); i++)
        {
            SpriteState value = state.sprites[i];
            renderers[i].enabled = value.enabled;
            renderers[i].color = value.color;
            renderers[i].flipX = value.flipX;
            renderers[i].flipY = value.flipY;
            if (string.IsNullOrEmpty(value.spriteKey)) renderers[i].sprite = null;
            else if (sprites.TryGetValue(value.spriteKey, out Sprite sprite)) renderers[i].sprite = sprite;
            else Debug.LogWarning($"저장한 스프라이트를 찾지 못했습니다: {value.spriteKey}", obj);
        }
    }

    public static void RestoreAnimations(GameObject obj, SceneObjectState state)
    {
        Animator[] animators = obj.GetComponents<Animator>();
        for (int i = 0; state.animators != null && i < Math.Min(animators.Length, state.animators.Length); i++)
        {
            Animator animator = animators[i];
            AnimatorState value = state.animators[i];
            animator.enabled = value.enabled;
            if (animator.runtimeAnimatorController == null || !animator.isActiveAndEnabled) continue;
            if (value.parameters != null)
            {
                foreach (AnimatorParameterState parameter in value.parameters)
                {
                    if (parameter.type == AnimatorControllerParameterType.Bool) animator.SetBool(parameter.name, parameter.boolValue);
                    else if (parameter.type == AnimatorControllerParameterType.Int) animator.SetInteger(parameter.name, parameter.intValue);
                    else if (parameter.type == AnimatorControllerParameterType.Float) animator.SetFloat(parameter.name, parameter.floatValue);
                    else animator.ResetTrigger(parameter.name);
                }
            }
            for (int layer = 0; value.layers != null && layer < Math.Min(animator.layerCount, value.layers.Length); layer++)
            {
                AnimatorLayerState saved = value.layers[layer];
                if (saved.hash != 0 && animator.HasState(layer, saved.hash)) animator.Play(saved.hash, layer, saved.time);
            }
            // 즉시 평가하면 Animation Event가 실행될 수 있으므로 Update(0)는 호출하지 않는다.
        }
    }
}
