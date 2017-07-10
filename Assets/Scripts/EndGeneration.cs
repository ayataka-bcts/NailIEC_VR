using UnityEngine;
using System.Collections;

public class EndGeneration : TetsButton {

    public GameObject canvas;

    // プレハブの読み込み
    public GameObject next_button;
    public GameObject end_button;
    public GameObject pop_up;

    // それぞれの位置
    public Vector3 next_pos;
    public Vector3 end_pos;
    public Vector3 pop_pos;

    // それぞれのボタンの角度
    //public Quaternion next_rot = new Quaternion();
    //public Quaternion end_rot = new Quaternion();

    // 一度表示したかどうか
    public static bool end = false;

	// Use this for initialization
	void Start () {

        next_pos = new Vector3(-198.269f, -453.145f, -3.042f);
        end_pos = new Vector3(-194.726f, -453.156f, -3.090f);
        pop_pos = new Vector3(-196.505f, -451.819f, -3.895f);
        //next_rot = Quaternion.Euler(0, -151.0485f, 0);
        //end_rot = Quaternion.Euler(0, 151.0485f, 0);

        end = true;

        canvas = GameObject.Find("Canvas");


	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(match[7, 0]);
        //Debug.Log(round_now[2]);
        // 決勝が終了していれば
        if (end == true && round_now[2] == 7)
        {
            /// ここもう少し短く簡潔に書けるかも
            // プレハブのインスタンス化
            GameObject prefab_next = (GameObject)Instantiate(next_button);
            GameObject prefab_end = (GameObject)Instantiate(end_button);
            GameObject prefab_pop = (GameObject)Instantiate(pop_up);

            // プレハブをCanvasの子要素としてインスタンス化
            prefab_next.transform.SetParent(canvas.transform, false);
            prefab_end.transform.SetParent(canvas.transform, false);
            prefab_pop.transform.SetParent(canvas.transform, false);

            // 位置の調整
            next_button.transform.position = next_pos;
            end_button.transform.position = end_pos;
            pop_up.transform.position = pop_pos;

            end = false;
        }
	}
}
