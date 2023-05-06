using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour,IPointerUpHandler
{
    public Transform target;

    public CardData data;

    public GameObject selectCardObj;

    private void Start()
    {
        
    }

    public void SetDragCard(CardData data, GameObject selectCardObj)
    {
        
    }
    private void Update()
    {
        transform.position = target.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
