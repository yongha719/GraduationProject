using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasExtension : MonoBehaviour
{
    public static Vector2 screenPoint;

    private RectTransform CanvasTr;
    private Camera Camera;


    private void Start()
    {
        CanvasTr = GetComponent<RectTransform>();
        Camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera, out screenPoint);
            print(screenPoint);
        }
    }

}
