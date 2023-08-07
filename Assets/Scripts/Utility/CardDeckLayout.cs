using AYellowpaper.SerializedCollections;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트에서 카드를 스폰하고 처음 드로우된 카드는 이 스크립트에 의해 정렬됨

// 이 스크립트가 하는 역할 ===
// 카드 드로우
// 덱에 있는 카드 정렬

/// <summary>/// 인게임에서 덱에 있는 카드의 레이아웃 </summary>
[AddComponentMenu("MyComponent/Card Deck Layout", int.MinValue)]
[ExecuteAlways]
public class CardDeckLayout : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("카드 레이아웃의 크기 범위")]
    [SerializeField]
    private Vector3 leftPosition = new Vector3(-220, -30, 0);

    [SerializeField]
    private Vector3 rightPosition = new Vector3(440, -30, 0);

    [Header("카드 레이아웃의 회전 범위")]
    [SerializeField]
    private Quaternion leftRotation = Quaternion.Euler(0, 0, 15f);

    [SerializeField]
    private Quaternion rightRotation = Quaternion.Euler(0, 0, -15f);

    [SerializeField]
    private int childCount;

    List<float[]> lerpValues = new(10);

    private void Start()
    {
        lerpValues.Add(new float[] { 0.5f });
        lerpValues.Add(new float[] { 0.27f, 0.73f });
        lerpValues.Add(new float[] { 0.1f, 0.5f, 0.9f });

        for (int i = 3; i < 10; i++)
        {
            var lerpValue = new float[i + 1];

            float interval = 1f / i;

            for (int j = 0; j < i + 1; j++)
                lerpValue[j] = interval * j;

            lerpValues.Add(lerpValue);
        }

        print(lerpValues.Capacity);
    }

    private void OnTransformChildrenChanged()
    {
        if (childCount == transform.childCount)
            return;

        var lerpValue = lerpValues[transform.childCount - 1];

        for (var i = 0; i < transform.childCount; i++)
        {
            Vector3 targetPos = Vector3.Lerp(leftPosition, rightPosition, lerpValue[i]);
            Quaternion targetRos = Quaternion.identity;

            if (transform.childCount >= 4)
            {
                float curve = Mathf.Sqrt(0.25f - Mathf.Pow(lerpValue[i] - 0.5f, 2)) * 80f;
                targetPos.y += curve;
                targetRos = Quaternion.Slerp(leftRotation, rightRotation, lerpValue[i]);
            }

            RectTransform rect = transform.GetChild(i) as RectTransform;
            rect.anchoredPosition = targetPos;
            rect.localRotation = targetRos;
        }

        childCount = transform.childCount;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}