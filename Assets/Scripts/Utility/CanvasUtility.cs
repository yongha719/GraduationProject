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


    private void Start()
    {
        CanvasTr = transform as RectTransform;
    }

    /// <summary>
    /// ���� ���콺 ��ġ�� ĵ���� ��ǥ �������� �������� �Լ�
    /// </summary>
    public static Vector2 GetMousePosToCanvasPos()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTr, Input.mousePosition, Camera.main, out screenPoint);

        return screenPoint;
    }
}
