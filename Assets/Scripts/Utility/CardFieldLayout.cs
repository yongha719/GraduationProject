using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class CardFieldLayout : MonoBehaviourPun
{
    [SerializeField] private bool IsMine;

    public List<UnitCard> Cards = new List<UnitCard>();

    private void Start()
    {
        IsMine = photonView.ViewID == (int)PhotonViewType.PlayerField;

        if (IsMine)
            CanvasUtility.MyFieldRect = (transform as RectTransform);
        else
            CanvasUtility.EnemyFieldRect = (transform as RectTransform);
    }
    
    public float spacing = 10f;

    private void UpdateLayout()
    {
        // float totalWidth = 0f;
        // int childCount = transform.childCount;
        //
        // for (int i = 0; i < childCount; i++)
        // {
        //     RectTransform rectTransform = transform.GetChild(i) as RectTransform;
        //     totalWidth += rectTransform.rect.width;
        //
        //     if (i < childCount - 1)
        //     {
        //         totalWidth += spacing;
        //     }
        // }
        //
        // float startX = -totalWidth / 2f;
        //
        // for (int i = 0; i < childCount; i++)
        // {
        //     RectTransform rectTransform = transform.GetChild(i) as RectTransform;
        //     Vector3 position = rectTransform.anchoredPosition;
        //     position.x = startX + rectTransform.rect.width / 2f;
        //     rectTransform.anchoredPosition = position;
        //
        //     startX += rectTransform.rect.width + spacing;
        // }
    }

    private void OnEnable()
    {
        UpdateLayout();
    }

    private void OnTransformChildrenChanged()
    {
        UpdateLayout();
    }

    private void OnValidate()
    {
        UpdateLayout();
    }
}