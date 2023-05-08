using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 덱 편성하는 카드
/// </summary>
public class DeckBuildingCard : MonoBehaviour, IPointerDownHandler
{
    [Header("UI")]
    [SerializeField]
    private Button selectBtn;

    [SerializeField]
    private Button activeMode;

    [SerializeField]
    private GameObject activeUI;

    public CardData data;
    void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
