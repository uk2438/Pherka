using System;
using UnityEngine;

public class TimeSenseTrigger : MonoBehaviour
{

    [Serializable]
    public class TransformGroup
    {
        public Transform[] transforms;
    }
    [Header("제어할 오브젝트들")]
    [SerializeField] private TransformGroup[] groups;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        int time = Chapter1Manager.Instance.GetHomeTime();
        switch (time)
        {
            case 2:
                // foreach(Transform transform in groups[0].transforms)
                //     transform.gameObject.SetActive(true);
                    //others deactive
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            default:
                break;
        }
    }
}
