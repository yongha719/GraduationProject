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
        }
    }

    /// <summary>
    /// 마우스 위치를 캔버스 좌표 기준으로 가져오는 함수
    /// </summary>
    public Vector2 GetMousePosToCanvasPos() => screenPoint;

}
