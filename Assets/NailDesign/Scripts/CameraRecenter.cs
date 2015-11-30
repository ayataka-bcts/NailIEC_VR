using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class CameraRecenter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // カメラ画面のセンターのリトラッキング
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InputTracking.Recenter();
        }
	
	}
}
