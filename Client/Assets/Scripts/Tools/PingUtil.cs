using UnityEngine;
using System.Collections;

public class PingUtil : MonoBehaviour
{

    public string ip = string.Empty;
    Ping ping;
    string label;
    GUIStyle guiStyle;

    void Start()
    {
        SendPing();


        guiStyle = new GUIStyle();
        guiStyle.normal.background = null;
        guiStyle.fontSize = 40;

    }

    bool isNetWorkLose = false;
    void OnGUI()
    {
        if (null != ping && (isNetWorkLose || ping.isDone))
        {
            isNetWorkLose = false;
            label = ping.time.ToString();
            SetColor(ping.time);
            ping.DestroyPing();
            ping = null;
            Invoke("SendPing", 1);//每秒Ping一次
        }

        GUI.Label(new Rect(10, 60, 200, 50), "Ping:" + label + "ms", guiStyle);
    }

    void SendPing()
    {
        ping = new Ping(ip);
    }

    void SetColor(int pingValue)
    {
        if (pingValue < 100)
        {
            guiStyle.normal.textColor = new Color(0, 1, 0);
        }
        else if (pingValue < 200)
        {
            guiStyle.normal.textColor = new Color(1, 1, 0);
        }
        else
        {
            guiStyle.normal.textColor = new Color(1, 0, 0);
        }
    }
}