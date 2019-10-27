using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFps : MonoBehaviour
{
   
    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;

    private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;
    [SerializeField]
    private Text _text;
    private int m_FrameUpdate = 0;//帧数;

    private float m_FPS = 0;


    private void Awake()
    {
       

        Application.targetFrameRate = -1;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
        StartCoroutine(showfps());
    }

    IEnumerator showfps()
    {

        while (true)
        {
            _text.text = "FPS: " + m_FPS.ToString("f2");
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
       
    }
   
    //void OnGUI()
    //{
    //    GUIStyle gUI = new GUIStyle();
    //    gUI.fontSize = 40;
    //    GUI.Label(new Rect(Screen.width / 2, 0, 100, 100), "FPS: " + m_FPS.ToString("f2"),gUI);
    //}



}

