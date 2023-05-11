using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// TODO : RuntimeInitializeOnLoadMethod Attribute 써서 정적 클래스로 만들기

/// <summary> 이 스크립트는 Canvas에 있어야 합니다 </summary>
public class CanvasUtility : MonoBehaviour
{
    private static Vector2 screenPoint;
    private static RectTransform CanvasTr;

    public static RectTransform MyFieldRect { get; set; }
    public static RectTransform EnemyFieldRect { get; set; }

    private void Start()
    {
        CanvasTr = transform as RectTransform;
    }

    /// <summary> 현재 마우스 위치를 캔버스 좌표 기준으로 가져오는 함수 </summary>
    public static Vector2 GetMousePosToCanvasPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera.main, out screenPoint);

        return screenPoint;
    }

    /// <summary> World Position을 캔버스 좌표 기준으로 가져오는 함수</summary>
    public static Vector2 GetWorldPosToCanvasPos(Vector3 pos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, pos, Camera.main, out screenPoint);

        return screenPoint;
    }

    public static bool IsDropMyField()
    {
        var mousePos = GetMousePosToCanvasPos();

        // 마우스 포지션이 내 필드 안에 있는지 확인
        if (MyFieldRect.rect.xMin < mousePos.x && MyFieldRect.rect.yMin < mousePos.y
            && MyFieldRect.rect.xMax > mousePos.x & MyFieldRect.rect.yMax > mousePos.y)
        {
            return true;
        }
        else
            return false;

        // TODO : 이거 왜 안되는지 알아보기
        //RectTransformUtility.RectangleContainsScreenPoint
    }
}
