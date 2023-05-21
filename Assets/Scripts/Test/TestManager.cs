using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestManager : Singleton<TestManager>
{
    public static bool IsPlayerInvincibility;

    // Editor 모드일 때 설정해놨던 테스트 정보들을 런타임에 대입해서 테스트 환경에 맞춰줌
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        GameManager.Instance.IsPlayerInvincibility = IsPlayerInvincibility;
    }

    private void Start()
    {
        GameManager.Instance.IsPlayerInvincibility = IsPlayerInvincibility;
    }


    public void CardDraw()
    {

    }
}
