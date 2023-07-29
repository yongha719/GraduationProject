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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera.main,
            out screenPoint);

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
    }

    /// <summary>
    /// Rect의 안에 마우스 위치에 따라 min과 max 사이에 값을 Vector2로 Lerp함
    /// <br></br>
    /// -- x와 y가 min과 max가 반대일 경우만 사용
    /// </summary>
    /// <param name="min">러프할 Min 값</param>
    /// <param name="max">러프할 Max 값</param>
    /// <param name="rect">RectTransform의 Rect</param>
    /// <param name="localPoint">마우스 포지션을 RectTransform의 로컬 좌표로 변환한 값이 필요함</param>
    /// <returns>Vector2로 Lerp 값</returns>
    public static Vector2 LerpVector2InRect(float min, float max, Rect rect, Vector2 localPoint)
    {
        Vector2 result = Vector2.zero;

        result.x = LerpValueInRect(min, max, rect, localPoint);
        result.y = LerpValueInRect(max, min, rect, localPoint);

        return result;
    }
    
    /// <summary>
    /// Rect의 안에 마우스 위치에 따라 min과 max 사이에 값을 Lerp한 값을 반환
    /// </summary>
    public static float LerpValueInRect(float min, float max, Rect rect, Vector2 localPoint)
    {
        float result = Mathf.Lerp(min, max, Clamp01(rect.xMin, rect.xMax, localPoint.x));

        return result;
    }


    /// <summary>
    /// 0과 1 사이의 값으로 반환
    /// </summary>
    /// <param name="min">Min 값</param>
    /// <param name="max">Max 값</param>
    /// <param name="value">0과 1 사이의 값의 기준</param>
    /// <returns></returns>
    private static float Clamp01(float min, float max, float value)
    {
        // 상대적인 위치 계산
        float relativePosition = (value - min) / (max - min);

        // 0과 1 사이의 범위로 매핑
        float mappedValue = Mathf.Clamp01(relativePosition);

        return mappedValue;
    }
}