using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 스크립트는 Canvas에 있어야 합니다
/// </summary>
public class CanvasUtility : MonoBehaviour
{
    private static Vector2 screenPoint;
    private static RectTransform CanvasTr;

    private static Vector2 MyFieldBottomLeft = new Vector2(230, 130);
    private static Vector2 MyFieldBottomRight = new Vector2(1690, 0);
    private static Vector2 EnemyFieldBottomLeft = new Vector2(230, 0);
    private static Vector2 EnemyFieldBottomRight = new Vector2(1690, 950);


    private void Start()
    {
        CanvasTr = transform as RectTransform;
    }

    /// <summary>
    ///  - 현재 마우스 위치를 캔버스 좌표 기준으로 가져오는 함수
    /// </summary>
    public static Vector2 GetMousePosToCanvasPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera.main, out screenPoint);

        return screenPoint;
    }

    /// <summary>
    ///  - World Position을 캔버스 좌표 기준으로 가져오는 함수
    /// </summary>
    public static Vector2 GetWorldPosToCanvasPos(Vector3 pos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, pos, Camera.main, out screenPoint);

        return screenPoint;
    }

    /// <returns> 
    ///  - 상대 필드의 Bottom Left와 Top Right를 (Vector2, Vector2) 튜플로 반환
    /// </returns>
    public static (Vector2, Vector2) GetEnemyFieldPosition()
    {
        return (EnemyFieldBottomLeft, EnemyFieldBottomRight);
    }

    /// <returns> 
    ///  - 내 필드의 Bottom Left와 Top Right를 AnchorPosition (Vector2, Vector2) 튜플로 반환
    /// </returns>
    public static (Vector2, Vector2) GetMyFieldPosition()
    {
        return (MyFieldBottomLeft, MyFieldBottomRight);
    }
}
