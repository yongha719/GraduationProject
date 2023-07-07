using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary> 이 스크립트는 Canvas에 있어야 합니다 </summary>
public class CanvasUtility : MonoBehaviour
{
    private static Vector2 screenPoint;
    private static RectTransform CanvasTr;

    public static RectTransform MyFieldRect { get; set; }
    public static RectTransform EnemyFieldRect { get; set; }

    private static RaycastHit2D[] raycastHits = new RaycastHit2D[10];

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

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is null ||
                hit.collider.TryGetComponent(out CardFieldLayout field) == false)
                continue;

            return true;
        }

        return false;

        //// 마우스 포지션이 내 필드 안에 있는지 확인
        //if (MyFieldRect.rect.xMin < mousePos.x && MyFieldRect.rect.yMin < mousePos.y
        //    && MyFieldRect.rect.xMax > mousePos.x & MyFieldRect.rect.yMax > mousePos.y)
        //{
        //    return true;
        //}
        //else
        //    return false;
    }
}
