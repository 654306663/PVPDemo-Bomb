using UnityEngine;
using System.Collections;

/// <summary>
/// œ‘ æFPS
/// </summary>

public class FpsUtil : MonoBehaviour
{

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;
    private string str = "";


    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }

    void OnGUI()
    {
        GUI.color = Color.red;
        GUIStyle style = new GUIStyle();
        style.fontSize = 40;
        style.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 10, 400, 400), "FPS:" + f_Fps.ToString("f2"), style);
    }

    void Update()
    {
        if (true)
        {
            ++i_Frames;
            if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

                i_Frames = 0;

                f_LastInterval = Time.realtimeSinceStartup;
            }
        }
    }
}
