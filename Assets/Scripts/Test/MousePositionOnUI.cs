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
        // 1. Screen 좌표를 UI 좌표로 변환
        Vector2 mousePosition = Input.mousePosition;
        Vector2 uiPosition;

        // 2. Screen 좌표를 UI 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, mousePosition, Camera.main, out uiPosition);

        // 3. UI 좌표의 유효성 확인
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePosition, Camera.main))
        {
            // UI 좌표를 사용하여 원하는 작업 수행
            // Debug.Log("Mouse Position on UI: " + uiPosition);

            Vector3 rotation = Vector3.one;
            
            rotation.x = Mathf.Lerp(15, -15, MapToZeroToOneX(uiPosition.x));
            rotation.y = Mathf.Lerp(-15, 15, MapToZeroToOneY(uiPosition.y));

            rect.rotation = Quaternion.Euler(rotation);
            
            print("rect rotation : " + rect.rotation.eulerAngles);
            print("rotation : " + rotation);

            Vector2 effectDistance;
            
            effectDistance.x = Mathf.Lerp(10, -10, MapToZeroToOneX(uiPosition.x));
            effectDistance.y = Mathf.Lerp(10, -10, MapToZeroToOneY(uiPosition.y));
            
            shadow.effectDistance = effectDistance;
        }
    }

    
    float MapToZeroToOneX(float value)
    {
        // 상대적인 위치 계산
        float relativePosition = (value - rectMin.x) / (rectMax.x - rectMin.x);

        // 0과 1 사이의 범위로 매핑
        float mappedValue = Mathf.Clamp01(relativePosition);

        return mappedValue;
    }
    
    float MapToZeroToOneY(float value)
    {
        // 상대적인 위치 계산
        float relativePosition = (value - rectMin.y) / (rectMax.y - rectMin.y);

        // 0과 1 사이의 범위로 매핑
        float mappedValue = Mathf.Clamp01(relativePosition);

        return mappedValue;
    }
}