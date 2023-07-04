using System;
using System.Collections;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public static string Message;

    [SerializeField] private GameObject logContent;

    [SerializeField] private RectTransform logListParent;

    private WaitForEndOfFrame wait = new WaitForEndOfFrame();

    private void Start()
    {
        // 유니티에서 지원하는 로그를 보내주는 이벤트
        Application.logMessageReceived += LogHandler;
    }

    /// <summary>
    /// 이벤트로 받은 로그 처리하는 함수
    /// </summary>
    /// <param name="message"> 받은 로그 메시지 </param>
    /// <param name="stackTrace"> 함수 호출 트리 </param>
    /// <param name="logType"> 로그 타입 (경고인지 에러인지 알려줌) </param>
    void LogHandler(string message, string stackTrace, LogType logType)
    {
        if (logType != LogType.Warning)
            StartCoroutine(ECreateLog(message));
    }

    // 그래픽 리빌드 루프가 진행 중일 때 실행이 돼 오류가 나서 루프가 끝난뒤 출력하려고 코루틴을 사용했음
    private IEnumerator ECreateLog(string message)
    {
        yield return wait;

        var time = DateTime.Now;
        Message = $"[{time.Hour:D2}:{time.Minute:D2}:{time.Second:D2}] {message}";

        Instantiate(logContent, logListParent);        
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= LogHandler;
    }
}