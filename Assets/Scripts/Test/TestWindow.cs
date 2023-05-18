using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    bool prevPlayerInvincibility = false;
    bool curPlayerInvincibility = false;

    [MenuItem("TestWindows/Card Test")]
    private static void Init()
    {
        TestWindow window = GetWindow<TestWindow>("Test Window");
        window.minSize = new Vector2(100, 100);
        window.maxSize = new Vector2(500, 500);
        window.Show();
    }


    private void OnGUI()
    {
        GUILayout.Space(5);

        #region Card Draw
        // Card Draw 버튼 만들어주고 창 크기에 맞춰 자동으로 확장되게 해줌
        if (GUILayout.Button("Card Draw", GUILayout.Width(130), GUILayout.Height(40)))
        {
            // 여기에 버튼을 눌렀을 때 액션을 넣어주면됨
            Debug.Log("카드 소환");

            if (TurnManager.Instance != null)
                TurnManager.Instance.PlayerCardDraw();
        }
        #endregion

        GUILayout.Space(15);

        #region 플레이어 무적
        // 매 프레임마다 호출되는 함수이기 때문에 토글 값이 바뀔 때만 액션이 일어나도록 해야함
        // prevPlayerInvincibility을 값으로 토글을 만들어줌
        curPlayerInvincibility = GUILayout.Toggle(prevPlayerInvincibility, "Player Invincibility");

        // 토글 값이 바뀌었는지 체크
        if (curPlayerInvincibility != prevPlayerInvincibility)
        {
            // 토클 값이 true일 때
            if (curPlayerInvincibility)
                Debug.Log("무적");

            prevPlayerInvincibility = curPlayerInvincibility;
        }
        #endregion
    }
}
