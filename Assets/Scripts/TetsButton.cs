using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class TetsButton : Tournament
{

    //現在の対戦個体（拡大表示されている個体）
    //[0]：左側個体
    //[1]：右側個体
    //[2]：対戦No.(0～3:一回戦第1～4試合，4～5：準決勝，6：決勝戦)
    public static int[] round_now = new int[3];

    // Tournamentクラスのインスタンス
    //public Tournament tor;

    // GAクラスのインスタンス化
    public GeneticAlgolithm ga;

    // 現在の世代数表示用テキスト
    public GameObject gene;

    // 提示個体用の右手と左手
    public GameObject left;
    public GameObject right;

    // 爪のスキンテクスチャ
    public Texture2D tex_L;
    public Texture2D tex_R;

    // 現在表示中の個体No表示用テキスト
    public GameObject left_number;
    public GameObject right_number;
    public string[] numbers = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

    // Constantクラスのインスタンス化(書かなくてもいいかも)
    //public Constant co;

    // 世代交代数カウント
    public static int generation = 1;

    // 対戦毎の評価時間計測
    float[,] time_round_record = new float[MEMBER - 1, GENERATION];

    // 1世代あたりの評価時間
    TimeSpan[] span_generation = new TimeSpan[GENERATION];
    // 1対戦あたりの評価時間
    TimeSpan[,] span_round = new TimeSpan[MEMBER, GENERATION];

    // 時間計測用関数(総合)
    public float start_time;
    public float end_time;

    // 世代毎の時間計測用
    public float time_generation;

    // 各対戦毎の時間計測用
    public float time_round;

    int[] non_normarized_point = new int[MEMBER];

    // コンストラクタみたいなもの
    void Start()
    {
        //tor = this.gameObject.GetComponent<Tournament>();
        ga = GameObject.Find("GA").GetComponent<GeneticAlgolithm>();
        //co = this.gameObject.GetComponent<Constant>();

        

        left_number = GameObject.Find("LeftNumber");
        right_number = GameObject.Find("RightNumber");

        gene = GameObject.Find("Generation");

        for (int i = 0; i < non_normarized_point.Length; i++)
            non_normarized_point[i] = 0;

        // round_nowの初期化
        round_now[0] = 0;
        round_now[1] = 1;
        round_now[2] = 0;

        // 開始時間の格納
        start_time = Time.time;
        time_generation = Time.time;
        time_round = Time.time;

    }

    // 毎フレーム呼び出される関数
    void Update()
    {
        left = GameObject.Find("Nail_left");
        right = GameObject.Find("Nail_right");

        RoundDisplay();
        //SupportDisplay();

        if (left == true)
        {
            DisplayNow(left, tex_L);
        }
        if (right == true)
        {
            DisplayNow(right, tex_R);
        }

    }

    // トーナメント表の更新
    public void UpdateTournament()
    {
        // 勝ち上がり個体の移動
        //WinTrans();

        // ラインの色変更
        LineChange();
    }

    // トーナメント表の表示更新
    public void LineChange()
    {
        // Lineを含んだ親
        GameObject parent;
        RawImage[] lines;

        // 一回戦終了時
        if (round_now[2] < 4)
        {

            parent = GameObject.Find("Line_" + numbers[result[round_now[2], 0]] + "");
            lines = parent.GetComponentsInChildren<RawImage>();

            lines[0].color = Color.red;
            lines[1].color = Color.red;
        }

        // 準決勝終了時
        else if (round_now[2] < 6)
        {
            switch (result[round_now[2], 0])
            {
                case 0:
                case 1:
                    parent = GameObject.Find("Line_semi_A");
                    lines = parent.GetComponentsInChildren<RawImage>();
                    break;
                case 2:
                case 3:
                    parent = GameObject.Find("Line_semi_B");
                    lines = parent.GetComponentsInChildren<RawImage>();
                    break;
                case 4:
                case 5:
                    parent = GameObject.Find("Line_semi_C");
                    lines = parent.GetComponentsInChildren<RawImage>();
                    break;
                case 6:
                case 7:
                    parent = GameObject.Find("Line_semi_D");
                    lines = parent.GetComponentsInChildren<RawImage>();
                    break;
                default:
                    lines = GetComponents<RawImage>();
                    break;
            }

            lines[0].color = Color.red;
            lines[1].color = Color.red;
        }

        // 決勝戦
        else
        {
            if (0 <= result[round_now[2], 0] && result[round_now[2], 0] <= 3)
            {
                parent = GameObject.Find("Line_final_A");
                lines = parent.GetComponentsInChildren<RawImage>();
            }
            else
            {
                parent = GameObject.Find("Line_final_B");
                lines = parent.GetComponentsInChildren<RawImage>();
            }

            lines[0].color = Color.red;
            lines[1].color = Color.red;

            RawImage line = GameObject.Find("Line_last").GetComponent<RawImage>();
            line.color = Color.red;
        }
    }


    // 勝ち個体の移動
    //public void WinTrans()
    //{
    // 勝ち個体の読み込み
    //  Debug.Log(result[round_now[2], 0]);
    //GameObject win = GameObject.Find("Individual" + (result[round_now[2], 0] + 1) + "");
    //}

    // トーナメントを補助するいろいろなテキスト表示
    public void SupportDisplay()
    {
        // 個体ナンバー
        left_number.GetComponent<Text>().text = numbers[round_now[0]];
        //Debug.Log(round_now[1]);
        right_number.GetComponent<Text>().text = numbers[round_now[1]];

        // 世代数
        gene.GetComponent<Text>().text = generation.ToString() + "世代目";
    }

    // 現在の対戦表示
    public void RoundDisplay()
    {
        Material mat_L;
        Material mat_R;

        // 対戦Noに応じて分岐
        switch (round_now[2])
        {
            case 0:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 1:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 2:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 3:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 4:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 5:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 6:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                mat_R = GameObject.Find("Materials/Sphere" + (round_now[1] + 1)).GetComponent<Renderer>().material;
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = (mat_R.mainTexture as Texture2D);
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
            case 7:
                mat_L = GameObject.Find("Materials/Sphere" + (round_now[0] + 1)).GetComponent<Renderer>().material;
                //mat_R = Resources.Load("Skin.png");
                tex_L = (mat_L.mainTexture as Texture2D);
                tex_R = Resources.Load("Skin.png", typeof(Texture2D)) as Texture2D;
                //tex_L = Resources.Load("Result/Generation" + generation + "/design" + (round_now[0] + 1), typeof(Texture2D)) as Texture2D;
                //tex_R = Resources.Load("Result/Generation" + generation + "/design" + (round_now[1] + 1), typeof(Texture2D)) as Texture2D;
                //DisplayNow(tex_L, tex_R);
                break;
        }

    }

    // 現在の対戦デザインを表示
    public void DisplayNow(GameObject nail, Texture tex)
    {
        nail.GetComponent<Renderer>().material.mainTexture = tex;
        //nail.GetComponent<Renderer>().material.mainTexture = tex;

    }

    // 左側のデザインを選択
    public void LeftSelect()
    {
        // 決勝戦が終了していれば
        if (match[7, 0] != -1)
        {
            // エリート個体を左側に表示
            round_now[0] = match[7, 0];

            // エリート画像の保存

            // 対戦表を出力
            match_output();

            // 次の行動を案内
            //EditorUtility.DisplayDialog("全ての対戦が終了しました", "続ける：次の世代へ  終了：システムを終了", "OK", "");

        }
        else
        {
            //Debug.Log("round_now[2]" + round_now[2]);
            // round_now[0]の勝ち
            // トーナメント処理
            round_result(round_now[0], round_now[1], round_now[2], 0);

            // １世代あたりの時間を保存
            time_round_record[round_now[2], generation] += Time.time - time_round;

            // トーナメント表の更新
            UpdateTournament();

            // 対戦更新
            for (int i = 0; i < MEMBER; i++)
            {
                if (match[i, 2] != -1)
                {
                    //Debug.Log("match[x][0] = [" + match[0, 0] + "][" + match[1, 0] + "][" + match[2, 0] + "][" + match[3, 0] + "][" + match[4, 0] + "][" + match[5, 0] + "][" + match[6, 0] + "][" + match[7, 0] + "]");
                    //Debug.Log("match[x][1] = [" + match[0, 1] + "][" + match[1, 1] + "][" + match[2, 1] + "][" + match[3, 1] + "][" + match[4, 1] + "][" + match[5, 1] + "][" + match[6, 1] + "][" + match[7, 1] + "]");
                    //Debug.Log("match[x][2] = [" + match[0, 2] + "][" + match[1, 2] + "][" + match[2, 2] + "][" + match[3, 2] + "][" + match[4, 2] + "][" + match[5, 2] + "][" + match[6, 2] + "][" + match[7, 2] + "]");
                    for (int j = 0; j < 3; j++)
                        round_now[j] = match[i, j];
                    break;
                }
            }

            // 現在の時刻を取得
            time_round = Time.time;

        }
        //}
    }

    // 右側のデザインを選択
    public void RightSelect()
    {
        // 決勝戦が終了していれば
        if (match[7, 0] != -1)
        {
            // エリート個体を左側に表示
            round_now[0] = match[7, 0];

            // エリート画像の保存

            // 対戦表を出力
            match_output();

            // 次の行動を案内
            //EditorUtility.DisplayDialog("全ての対戦が終了しました", "続ける：次の世代へ  終了：システムを終了", "OK", "");

        }
        else
        {
            //Debug.Log("round_now[2]" + round_now[2]);
            // round_now[1]の勝ち
            // トーナメント処理
            round_result(round_now[1], round_now[0], round_now[2], 1);

            // １世代あたりの時間を保存
            time_round_record[round_now[2], generation] += Time.time - time_round;

            // トーナメント表の更新
            UpdateTournament();

            // 対戦更新
            for (int i = 0; i < MEMBER; i++)
            {
                if (match[i, 2] != -1)
                {
                    //Debug.Log("match[x][0] = [" + match[0, 0] + "][" + match[1, 0] + "][" + match[2, 0] + "][" + match[3, 0] + "][" + match[4, 0] + "][" + match[5, 0] + "][" + match[6, 0] + "][" + match[7, 0] + "]");
                    //Debug.Log("match[x][1] = [" + match[0, 1] + "][" + match[1, 1] + "][" + match[2, 1] + "][" + match[3, 1] + "][" + match[4, 1] + "][" + match[5, 1] + "][" + match[6, 1] + "][" + match[7, 1] + "]");
                    //Debug.Log("match[x][2] = [" + match[0, 2] + "][" + match[1, 2] + "][" + match[2, 2] + "][" + match[3, 2] + "][" + match[4, 2] + "][" + match[5, 2] + "][" + match[6, 2] + "][" + match[7, 2] + "]");
                    for (int j = 0; j < 3; j++)
                        round_now[j] = match[i, j];
                    break;
                }
            }

        }
        //}
    }

    // 終了ボタンが押されたら
    public void EndSelect()
    {
        if (match[7, 0] != -1)
        {
            // メッセージボックス(終了するか否かの確認)
            //var check = EditorUtility.DisplayDialog("確認", "終了してもよろしいですか？？", "OK", "Cancel");

            //if (check == true)
            //{

            // 評価時間計測
            end_time = Time.time - start_time;
            //var time = end_time - start_time;

            // 評価情報，遺伝子列保存
            individual_save(1);

            // 対戦表の出力
            match_output();

            // 評価時間の出力

            // エリート個体保存

            // エリートを履歴に保存

            //check = EditorUtility.DisplayDialog("お疲れ様でした", "実験のご参加感謝いたします．", "OK", "");
            //if (check == true)
            Application.Quit();
            //}
        }
    }

    // 次の世代ボタンがクリックされたら
    public void NextSelect()
    {
        //var check = EditorUtility.DisplayDialog("確認", "次のトーナメントに進みます．よろしいですか？？", "OK", "Cancel");

        //if (check == true)
        //{
        if (match[7, 0] != -1)
        {
            // 各個体の評価
            evaluation_individual();

            // 評価点をgaに渡す
            for (int i = 0; i < MEMBER; i++)
                ga.evaluation_point[i] = tournament_point[i];

            //正規化の有無
            if (normarized == 1)
                Normarized_Value();

            // 時間取得評価時間測定

            // 評価情報，遺伝子列のファイル保存
            individual_save(0);

            /// ------------------------- 次世代の処理 ------------------------
            // GA処理
            ga.GAprocess(generation);

            // 世代更新
            generation++;

            // 新世代のトーナメント表作成
            create_tournament_table();

            // round_nowを初期化
            // 1回戦1試合目の個体を設定
            round_now[0] = match[0, 0];
            round_now[1] = match[0, 1];
            round_now[2] = match[0, 2];

            // トーナメントを初期化
            TournamentReset();

            // 表示更新

            // 現在の時刻取得
            time_generation = Time.time;
            time_round = Time.time;

            // シーンの再読み込み
            //SceneManager.LoadScene("main");

            // 次の世代の個体描画
            LoadScene lo = GameObject.Find("Setup").GetComponent<LoadScene>();
            int[] temp = new int[GENE_LENGTH];
            for (int i = 0; i < MEMBER; i++)
            {
                for (int j = 0; j < GENE_LENGTH; j++)
                    temp[j] = ga.individual[j, i];
                lo.CreateTexture(temp, i);
            }
        }
        //}
    }

    // 遺伝子列書き込み
    void individual_save(int check)
    {
        StreamWriter sw;
        FileInfo fi;

        fi = new FileInfo(LoadScene.output + "/Record.csv");
        sw = fi.AppendText();

        if (generation == 1 && normarized == 0)
            sw.Write("Generation,Solution No.,Evaluation value,,Gene row");

        if (generation == 1 && normarized == 1)
            sw.Write("Generation,Solution No.,before normarized,After normarized,,Gene row");

        sw.WriteLine();

        for (int i = 0; i < MEMBER; i++)
        {
            //評価点書き込み
            if (normarized == 0)
                sw.Write(generation.ToString() + "," + i.ToString()
                    + "," + ga.evaluation_point[i].ToString() + ",");

            if (normarized == 1)
                sw.Write(generation.ToString() + "," + i.ToString()
                    + "," + non_normarized_point[i].ToString() + ","
                        + ga.evaluation_point[i].ToString() + ",");

            sw.Write(",");

            //遺伝子列書き込み
            for (int j = 0; j < GENE_LENGTH; j++)
            {
                sw.Write(ga.individual[j, i].ToString());
                sw.Write(",");
            }

            //改行処理
            sw.WriteLine();
        }

        //評価時間の書き込み
        //if (check == 0)
        //{
        //    string copy_ts = end_time_generation.ToString("0.00[sec]");
        //    sw.Write("Evaluation time:," + copy_ts);
        //    sw.WriteLine();
        //}
        //「終了」ボタンが押された場合
        //if (check == 1)
        //{
        //    string copy_ts = end_time_generation.ToString("0.00[sec]");
        //    sw.Write("Evaluation time:," + copy_ts + "End");
        //    sw.WriteLine();
        //}
        //sw.Close();

        sw.Flush();
        sw.Close();
    }

    // トーナメント表の出力
    private void match_output()
    {
        StreamWriter sw;
        FileInfo fi;

        fi = new FileInfo(LoadScene.output + "/TournamentTable.csv");
        sw = fi.AppendText();

        if (generation == 1)
            sw.Write("Generation,The first round 1,The first round 2,The first round 3,The first round 4,The semi final 1,The semi final 2,The final");

        //改行処理
        sw.WriteLine();

        //世代交代数表示
        sw.Write(generation.ToString() + ",");

        //対戦表出力処理
        //見出し表示
        sw.Write("Winner,");

        for (int i = 0; i < MEMBER - 1; i++)
            sw.Write(result[i, 0].ToString() + ",");

        //改行処理
        sw.WriteLine();

        //見出し表示
        sw.Write(",Loser,");
        for (int i = 0; i < MEMBER - 1; i++)
            sw.Write(result[i, 1].ToString() + ",");

        //改行処理
        sw.WriteLine();

        //終了したか否か
        //見出し表示
        sw.Write(",Judge,");
        for (int i = 0; i < MEMBER - 1; i++)
        {
            if (match[i, 2] == -1)
                sw.Write("End,");
            else
                sw.Write("Yet,");
        }

        sw.Flush();
        sw.Close();

    }

    // 評価時間の出力
    private void record_evaluation_time(int check)
    {
        StreamWriter sw;
        FileInfo fi;
        string copy_time_round, copy_time_generation;

        fi = new FileInfo(LoadScene.output + "/Record.csv");
        sw = fi.AppendText();

        //見出し作成
        sw.Write("Generation,The first round 1,The first round 2,The first round 3,The first round 4,The semi final 1,The semi final 2,The final,Total");
        sw.WriteLine();

        //全評価時間を出力
        for (int i = 0; i < generation; i++)
        {
            //世代交代数出力                
            sw.Write((i + 1).ToString() + ",");

            //1対戦あたりの評価時間出力
            for (int j = 0; j < MEMBER - 1; j++)
            {
                copy_time_round = time_round_record[j, i].ToString("0.00");
                sw.Write(copy_time_round + ",");
            }

            //1世代あたりの評価時間出力
            copy_time_generation = span_generation[i].TotalSeconds.ToString("0.00[sec]");
            sw.Write(copy_time_generation + ",");

            //改行処理
            sw.WriteLine();
        }
        if (check == 1)
            sw.Write("End");

        sw.Flush();
        sw.Close();
    }

    // 評価値の正規化
    private void Normarized_Value()
    {
        //正規化前の評価値を保存
        int i;
        for (i = 0; i < MEMBER; i++)
            non_normarized_point[i] = ga.evaluation_point[i];

        //正規化処理
        int max = 10;
        int min = 4;

        if (max == min)
        {
            for (i = 0; i < MEMBER; i++)
                ga.evaluation_point[i] = 100;
        }
        else
        {
            for (i = 0; i < MEMBER; i++)
                ga.evaluation_point[i] =
                    (int)((double)(non_normarized_point[i] - min) / (double)(max - min) * 100.0);
        }

    }

    // ２世代目以降のトーナメント初期化
    void TournamentReset()
    {
        GameObject parent = GameObject.Find("Lines");
        RawImage[] images = parent.GetComponentsInChildren<RawImage>();

        foreach (RawImage img in images)
        {
            img.color = LINE_COLOR;
        }
    }

    
}
