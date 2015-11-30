using UnityEngine;
using System.Collections;

public class MovingSky : MonoBehaviour {

    public static float axis;

	// Use this for initialization
	void Start () {
        axis = 0;
	}
	
	// Update is called once per frame
	void Update () { 
        transform.Rotate(new Vector3(0, 0, axis + 0.05f));
	}
}
