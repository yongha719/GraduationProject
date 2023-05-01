using System;
using UnityEngine;

public class MyDebugCore : MonoBehaviour
{
    [SerializeField]
    private GameObject debugContent;

    [SerializeField]
    private RectTransform debugListParent;

    private void Start()
    {
        Application.logMessageReceived += LogHandler;
    }

    void LogHandler(string message, string stackTrace, LogType logType)
    {
        Log(message);
    }
    
    private void Log(string message)
    {
        var time = DateTime.Now;
        string log = $"[{time.Hour:D2}:{time.Minute:D2}:{time.Second:D2}] {message}";

        Instantiate(debugContent, debugListParent).GetComponent<DebugContent>().SetLog(log);
    }
}

