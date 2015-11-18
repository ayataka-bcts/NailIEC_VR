using UnityEngine;
using System.Collections;

public class GeneticAlgolithm : Constant {

	/// ----------------------GA処理に用いる変数-----------------------------
    
    // 提示用個体
    public int[,] individual = new int[GENE_LENGTH, MEMBER];

    // 提示用に変換したナンバー
    public int[,] display = new int[PATTERN, MEMBER];

    // 子個体格納用
    public int[,] children = new int[GENE_LENGTH, MEMBER];

    // エリート個体格納用
    public int[] elite = new int[GENE_LENGTH];

    // 各個体の点数
    public int[] evaluation_point = new int[MEMBER];

    //選択個体No
    public int[] selected_number = new int[2];

    /// --------------------GA処理に用いる変数(ここまで)----------------------
    
    // 履歴保存用エリート個体保存
    public int[, ,] elite_record = new int[GENERATION, MEMBER, GENE_LENGTH];

    public Random ran = new Random();

    /// GA処理に用いる関数

    // コンストラクタ
    public GeneticAlgolithm()
    {
        int generation, length, member;

        // 初期個体生成
        for (length = 0; length < GENE_LENGTH; length++)
            for (member = 0; member < MEMBER; member++)
                individual[length, member] = Random.Range(0, 2);

        // 子個体初期化
        for (length = 0; length < GENE_LENGTH; length++)
            for (member = 0; member < MEMBER; member++)
                children[length, member] = -1;

        // 履歴配列初期化
        for (generation = 0; generation < GENERATION; generation++)
            for (member = 0; member < MEMBER; member++)
                for (length = 0; length < GENE_LENGTH; length++)
                    elite_record[generation, member, length] = -1;

        // 生成個体の確認

        // 数値変換


    }
}
