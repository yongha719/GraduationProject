using System;
using UnityEngine;

public class MyDebugCore : MonoBehaviour
{
    [SerializeField]
    private GameObject debugContent;

    [SerializeField]
    private RectTransform debugListParent;

    private void Awake() => MyDebug.debugCore = this;

    public void Log(string message)
    {
        var time = DateTime.Now;
        string log = $"[{time.Hour}:{time.Minute}:{time.Second}] {message}";

        Instantiate(debugContent, debugListParent).GetComponent<DebugContent>().SetLog(log);
        Debug.Log(message);
    }
}

public static class MyDebug
{
    public static MyDebugCore debugCore;

    public static void Log(string message) => debugCore.Log(message);
}
