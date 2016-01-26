using UnityEngine;
using System.Collections;

public class EndPanel : TetsButton {

    //public GeneticAlgolithm ga;

	// Use this for initialization
	void Start () {
        //ga = GameObject.Find("GA").GetComponent<GeneticAlgolithm>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Enter");
        EndSelect();

        //DeleteButtons();

        EndGeneration.end = true;
    }
}
