using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUtility : MonoBehaviour
{
    private static Vector2 screenPoint;
    private static RectTransform CanvasTr;


    private void Start()
    {
        CanvasTr = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 현재 마우스 위치를 캔버스 좌표 기준으로 가져오는 함수
    /// </summary>
    public static Vector2 GetMousePosToCanvasPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera.main, out screenPoint);

        return screenPoint;
    }
}
