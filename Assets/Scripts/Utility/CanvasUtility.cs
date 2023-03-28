using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ��ũ��Ʈ�� Canvas�� �־�� �մϴ�
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
    ///  - ���� ���콺 ��ġ�� ĵ���� ��ǥ �������� �������� �Լ�
    /// </summary>
    public static Vector2 GetMousePosToCanvasPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera.main, out screenPoint);

        return screenPoint;
    }

    /// <summary>
    ///  - World Position�� ĵ���� ��ǥ �������� �������� �Լ�
    /// </summary>
    public static Vector2 GetWorldPosToCanvasPos(Vector3 pos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, pos, Camera.main, out screenPoint);

        return screenPoint;
    }

    /// <returns> 
    ///  - ��� �ʵ��� Bottom Left�� Top Right�� (Vector2, Vector2) Ʃ�÷� ��ȯ
    /// </returns>
    public static (Vector2, Vector2) GetEnemyFieldPosition()
    {
        return (EnemyFieldBottomLeft, EnemyFieldBottomRight);
    }

    /// <returns> 
    ///  - �� �ʵ��� Bottom Left�� Top Right�� AnchorPosition (Vector2, Vector2) Ʃ�÷� ��ȯ
    /// </returns>
    public static (Vector2, Vector2) GetMyFieldPosition()
    {
        return (MyFieldBottomLeft, MyFieldBottomRight);
    }
}
