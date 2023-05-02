using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun.UtilityScripts;


public enum ECardDeckState//현재 어디위치에 있는지
{
    HaveCardArea,
    PutOnCardArea,
    Dragging,
}
public class CardDeck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private Image illust;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI abilityExplaneText;
    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField]
    private TextMeshProUGUI powerText;
    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private GameObject selectCard;

    [SerializeField]
    private GameObject deSelectCard;

    
    private CardData data;
    public CardData Data 
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }

    /// <summary>
    /// CardDeckSetter함수
    /// </summary>
    private void SetCardDeck()
    {

    }


    private void Start()
    {

    }

    private void Update()
    {
        DrawRay();
    }

    private void DrawRay()
    {
        //Debug.DrawRay(transform.position + rayOriginPos, rayDir, Color.red, 10f);
        //print(ray.transform.position);
        //if (Physics.Raycast(transform.position + rayOriginPos, rayDir, out ray, 10f))
        //{
        //    print(ray.collider.name);
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        if(transform.position.x >= DeckManager.standardX)
        {

        }

    }

    private void ChangeState()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
