using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class practice : time_display {

    //public static DateTime start;
    //public const int TIMELIMIT = 120;

	// Use this for initialization
	void Start () {

        start = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
        
        var time = (int)(TIME_LIMIT - (DateTime.Now - start).TotalSeconds);

        // 設定時間になったら終了
        if (time < 0)
        {
            // mainのシーンを読み込む
            SceneManager.LoadSceneAsync("main");
        }
	
	}

    void OnTriggerEnter()
    {
        // 擬似ダイアログを出現させる

        // YESならmainシーンを読み込む
        SceneManager.LoadSceneAsync("main");

        // NO ならなにもしない
    }
}
