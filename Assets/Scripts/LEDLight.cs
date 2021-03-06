﻿using UnityEngine;
using System.Collections;

public class LEDLight : TetsButton {

    // 手が入っているかどうか
    public bool enter_log = false;
    bool cool_time = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.gameObject.name);
        // 入室状態にする
        enter_log = true;
    }

    void OnTriggerStay()
    {
        //Debug.Log("T Stay !!");
    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("T Exit !!");
        if (col.name == "palm_left" && enter_log == true && cool_time == true)
        {
            LeftSelect();
            cool_time = false;
            Invoke("CoolSet", 2.0f);
        }
        if (col.name == "palm_right" && enter_log == true && cool_time == true)
        {
            RightSelect();
            cool_time = false;
            Invoke("CoolSet", 2.0f);
        }

        // 退室状態にする
        enter_log = false;
    }

    void CoolSet()
    {
        cool_time = true;
    }
}
