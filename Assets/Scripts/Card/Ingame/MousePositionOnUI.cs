using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MousePositionOnUI : MonoBehaviour
{
    private RectTransform rect;
    private Shadow shadow;
    
    private Vector2 rectMax;
    private Vector2 rectMin;
    
    private void Start()
    {
        rect = transform as RectTransform;
        shadow = GetComponent<Shadow>();
        
        rectMax = rect.rect.max;
        rectMin = rect.rect.min;
    }
    

    private void Update()
    {
 
    }

    

}