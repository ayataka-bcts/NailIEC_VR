using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class time_display : MonoBehaviour {

    public const double TIME_LIMIT = 40.0;
    public Text td;
    public DateTime start;

	// Use this for initialization
	void Start () {
       td = GameObject.Find("time_display").GetComponent<Text>();
       start = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {

        var time = (int)(TIME_LIMIT - (DateTime.Now - start).TotalSeconds);

        if (time < 0)
        {
            time = 0;
            SceneManager.LoadScene("main");
        }

        td.text = "残り時間：" + time;
	}
}
