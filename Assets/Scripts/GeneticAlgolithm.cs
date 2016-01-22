using UnityEngine;
using System.Collections;
using System;

public class GeneticAlgolithm : Constant
{

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

    public System.Random ran = new System.Random();

    /// GA処理に用いる関数

    // コンストラクタ
    public void Awake()
    {
        int generation, length, member;

        // 初期個体生成
        for (length = 0; length < GENE_LENGTH; length++)
            for (member = 0; member < MEMBER; member++)
                individual[length, member] = UnityEngine.Random.Range(0, 2);

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
        check_candidates();

    }

    // 配列の要素の中で一番大きい値の場所を返す
    int MaxReturn(int[] array, int value)
    {
        int ret = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value)
                ret = i;
        }

        return ret;
    }

    // GA処理
    public int GAprocess(int generation)
    {
        //エリート保存
        record_elite_individual(generation);


        //(MEMBER-1)個分のGA処理
        for (int i = 0; i < MEMBER - 1; i++)
        {
            //選択
            selection();
            //交叉
            crossover(i);
            //突然変異
            mutation(i);
        }

        int[,] ham = new int[MEMBER, MEMBER];

        for (int i = 0; i < MEMBER; i++)
            for (int j = 0; j < MEMBER; j++)
                ham[i, j] = 0;

        //エリート個体のコピー
        for (int i = 0; i < GENE_LENGTH; i++)
            individual[i, 0] = elite[i];
        //子個体をコピー
        for (int i = 0; i < MEMBER - 1; i++)
            for (int j = 0; j < GENE_LENGTH; j++)
                individual[j, i + 1] = children[j, i];

        //生成個体の確認
        check_candidates();

        //数値変換
        //exchange_individual();

        return generation + 1;
    }

    // エリート保存
    public void record_elite_individual(int generation)
    {
        int k = 0;
        //エリート保存処理
        //初期化
        for (int i = 0; i < GENE_LENGTH; i++)
            elite[i] = 0;

        //エリート検索
        int max = 100;
        int max_number = MaxReturn(evaluation_point, max);//Array.IndexOf(evaluation_point, max);

        //エリート保存
        for (int i = 0; i < GENE_LENGTH; i++)
            elite[i] = individual[i, max_number];

        //エリートを履歴に保存
        for (int i = 0; i < MEMBER; i++)
        {
            if (max == evaluation_point[i])
            {
                for (int j = 0; j < GENE_LENGTH; j++)
                    elite_record[generation, k, j] = individual[j, i];

                k++;
            }
        }
    }

    // 選択処理
    public void selection()
    {
        int total_eval = 0;     //評価値の和
        int temp = 0;	        //ルーレット判定時に使用
        int r = 0;
        int sn = SCALENUMBER;   //スケーリング乗数

        // 選択個体No初期化
        selected_number[0] = -1;
        selected_number[1] = -1;

        // 評価値の和を求める(ルーレット台を作成)
        for (int i = 0; i < MEMBER; i++)
        {
            // スケーリング実行
            switch (sn)
            {
                case 3:// 3乗スケーリング
                    total_eval += evaluation_point[i] * evaluation_point[i] * evaluation_point[i]; break;
                case 2:// 2乗スケーリング
                    total_eval += evaluation_point[i] * evaluation_point[i]; break;
                case 1:// 1乗スケーリング
                    total_eval += evaluation_point[i]; break;
                case 0:// 1/2乗スケーリング
                    total_eval += (int)Mathf.Sqrt(evaluation_point[i]); break;
            }
        }

        // ランダム関数の定義
        // Random rand = new Random();
        // 乱数発生
        r = ran.Next(0, total_eval);

        //1つ目の交叉用個体の抽出
        for (int i = 0; i < MEMBER; i++)
        {
            //スケーリング実行
            switch (sn)
            {
                case 3://3乗スケーリング
                    temp += (evaluation_point[i]) * (evaluation_point[i]) * (evaluation_point[i]); break;
                case 2://2乗スケーリング
                    temp += (evaluation_point[i]) * (evaluation_point[i]); break;
                case 1://1乗スケーリング
                    temp += (evaluation_point[i]); break;
                case 0:// 1/2乗スケーリング
                    temp += (int)Mathf.Sqrt(evaluation_point[i]); break;
            }

            if (r < temp)
            {
                selected_number[0] = i;
                break;
            }
        }

        //2つ目の交叉用個体の抽出
        //do
        {
            r = UnityEngine.Random.Range(0, total_eval);	//乱数発生
            temp = 0;
            for (int i = 0; i < MEMBER; i++)
            {
                //スケーリング実行
                switch (sn)
                {
                    case 3://3乗スケーリング
                        temp += (evaluation_point[i]) * (evaluation_point[i]) * (evaluation_point[i]); break;
                    case 2://2乗スケーリング
                        temp += (evaluation_point[i]) * (evaluation_point[i]); break;
                    case 1://1乗スケーリング
                        temp += (evaluation_point[i]); break;
                    case 0:// 1/2乗スケーリング
                        temp += (int)Mathf.Sqrt(evaluation_point[i]); break;
                }

                if (r < temp)
                {
                    selected_number[1] = i;
                    break;
                }
            }
        } //while (selected_number[0] == selected_number[1]);

    }

    // 交叉処理
    public void crossover(int individual_number)
    {
        int[] mask = new int[GENE_LENGTH];//マスク
        //Random ran=new Random();
        int total = 0, r = 0;

        //選択個体の評価点を足し合わせる
        total = evaluation_point[selected_number[0]] + evaluation_point[selected_number[1]];

        //乱数が選択個体の評価点より低ければ0,高ければ1でマスク作成
        for (int i = 0; i < GENE_LENGTH; i++)
        {
            r = ran.Next(0, total);

            if (r < evaluation_point[selected_number[0]])
                mask[i] = 0;
            else
                mask[i] = 1;
        }

        //マスクが0であれば親1の遺伝子を代入，1であれば親2の遺伝子を代入
        for (int i = 0; i < GENE_LENGTH; i++)
        {
            if (mask[i] == 0)
                children[i, individual_number] = individual[i, selected_number[0]];
            else
                children[i, individual_number] = individual[i, selected_number[1]];
        }
    }

    // 突然変異処理
    public void mutation(int individual_number)
    {
        // 突然変異実行
        for (int i = 0; i < GENE_LENGTH; i++)
        {
            int r = ran.Next(0, 100);
            // 反転処理
            if (r < MUTATION)
                children[i, individual_number] = 1 - children[i, individual_number];
        }
    }

    //個体の確認
    public void check_candidates()
    {
        int i, j, k;
        int similar = 0;

        for (j = 0; j < MEMBER; j++)
        {
            for (k = 0; k < MEMBER; k++)
            {
                similar = 0;

                for (i = 0; i < GENE_LENGTH; i++)
                {
                    //類似度算出
                    if (j != k)
                    {
                        if (individual[i, j] == individual[i, k])
                        {
                            similar++;
                        }
                    }
                }

                //同じ個体が存在→突然変異処理
                if (similar == GENE_LENGTH)
                {
                    int r = ran.Next(0, GENE_LENGTH);

                    //反転処理
                    individual[r, j] = 1 - individual[r, j];

                    //繰り返し変数初期化
                    j = 0;
                    k = -1;
                }

            }
        }
    }
}
