using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun.UtilityScripts;

public class CardDeck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rect;

    [SerializeField]
    private Image illust;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI abilityExplaneText;
    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField]
    private TextMeshProUGUI power;
    [SerializeField]
    private TextMeshProUGUI hp;

    [SerializeField]
    private CardData cardData;

    private Vector3 rayOriginPos = new Vector3(0, 0, -1f);
    private Vector3 rayDir = Vector3.forward;

    private RaycastHit ray;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        DrawRay();
    }
    private void SetCardDeck()
    {

    }

    private void DrawRay()
    {
        Debug.DrawRay(transform.position + rayOriginPos, rayDir, Color.red, 10f);
        print(ray.transform.position);
        if (Physics.Raycast(transform.position + rayOriginPos, rayDir, out ray, 10f))
        {
            print(ray.collider.name);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
