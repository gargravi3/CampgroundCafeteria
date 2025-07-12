using System;
using UnityEngine;

public class TelemetryLogger : MonoBehaviour
{
    private DateTime startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = DateTime.Now;
    }

    public void LogEvent(string EventName)
    {
        DateTime logTime = DateTime.Now;
        TimeSpan logSpan = logTime - startTime;
        Debug.Log(logTime.ToString(@"hh\:mm\:ss") + " - PLAYTEST LOG: " + EventName + " - Playtime: " + logSpan.ToString(@"mm\:ss"));
    }
}
