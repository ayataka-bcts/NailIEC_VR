using UnityEngine;
using System.Collections;

public class NextPanel : TetsButton {

    //public bool check = false;

    // GAクラスのインスタンス
    //public GeneticAlgolithm ga;

	// Use this for initialization
	void Start () {

        // インスタンス化
        //ga = GameObject.Find("GA").GetComponent<GeneticAlgolithm>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Enter");

            NextSelect();
            
            //check = true;
            // 出現したボタンを消す
            //DeleteButtons();

            EndGeneration.end = true;

    }
}
