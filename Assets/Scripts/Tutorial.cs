using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

    public Camera cam;
    public RaycastHit hit;
    public Ray ray;

	// Use this for initialization
	void Start () {
        ray = cam.ScreenPointToRay(Input.mousePosition);

	}
	
	// Update is called once per frameq
	void Update () {

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "chara")
            {
                Transform objectHit = hit.transform;
                Debug.Log(objectHit);
            }
        }
	    
	}
}
