using UnityEngine;
using System.Collections;

public class Tournament : Constant {

    /// --------------------------トーナメント処理に用いる変数-------------------------

    //対戦終了判定用変数
    public int round_1 = 0;
    public int round_2 = 0;
    public int semi_fin = 1;

    //対戦処理用の対戦表
    //[x][0]：対戦個体１
    //[x][1]：対戦個体２
    //[x][2]：対戦No. → 終了した対戦は，－１になる
    public int[,] match = new int[MEMBER, 3];

    //勝敗結果格納
    //[x][0]：勝ち個体
    //[x][1]：負け個体
    public int[,] result = new int[MEMBER, 2];

    //各個体の評価点
    public int[] tournament_point = new int[MEMBER];

    /// -----------------------トーナメント処理に用いる変数(ここまで)--------------------

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
