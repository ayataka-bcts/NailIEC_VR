using UnityEngine;
using System.Collections;

public class Tournament : Constant
{

    /// -------------------------- トーナメント処理に用いる変数 -----------------------------------

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

    /// ----------------------- トーナメント処理に用いる変数(ここまで) ----------------------------

    public Tournament()
    {
        create_tournament_table();
    }

    /// ----------------------- トーナメント表作成 ------------------------------------------------
    public void create_tournament_table()
    {
        int member, i = 0, k = 0;

        // 結果表の初期化
        for (member = 0; member < MEMBER; member++)
            for (i = 0; i < 2; i++)
                result[member, i] = 0;

        // 対戦表の初期化
        for (member = 0; member < MEMBER; member++)
        {
            i = 0;

            if (k < MEMBER)
            {
                match[member, 0] = k;
                match[member, 1] = k + 1;
                match[member, 2] = member;
                k += 2;
            }

            else
            {
                match[member, 0] = -1;
                match[member, 1] = -1;
                match[member, 2] = member;
            }
        }

        // 対戦終了判定用変数
        round_1 = 0;
        round_2 = 0;
        semi_fin = 1;
    }
    /// ----------------------- トーナメント表作成(ここまで) --------------------------------------

    /// ----------------------- 対戦結果処理 ------------------------------------------------------
    /// 引数1；勝ち個体  引数2：負け個体  引数3：対戦No.  引数4：選ばれた手
    public void round_result(int win_num, int lose_num, int match_num, int left_or_rigth)
    {
        // 勝ち個体の記録
        result[match_num, 0] = win_num;

        // 負け個体の記録
        result[match_num, 1] = lose_num;

        // 対戦終了判定
        match[match_num, 2] = -1;
        match[match_num, left_or_rigth] = -1;

        //対戦表更新
        switch (match_num)
        {
            case 0://一回戦第1試合
                match[4, 0] = win_num;
                break;
            case 1://一回戦第2試合
                match[4, 1] = win_num;
                break;
            case 2://一回戦第3試合
                match[5, 0] = win_num;
                break;
            case 3://一回戦第4試合
                match[5, 1] = win_num;
                break;
            case 4://準決勝第1試合
                match[6, 0] = win_num;
                break;
            case 5://準決勝第2試合
                match[6, 1] = win_num;
                break;
            case 6://決勝
                match[7, 0] = win_num;
                break;
        }

        //対戦終了判定
        round_judge();
    }
    /// -------------------------- 対戦結果処理(ここまで) -----------------------------------------

    /// -------------------------- 対戦終了判定 ---------------------------------------------------
    public void round_judge()
    {
        // 一回戦終了判定
        for (int i = 0; i < 4; i++)
        {
            if (i < 2)
            {
                if (match[i, 2] == -1)
                    round_1 = 1;
                else
                {
                    round_1 = 0;
                    break;
                }
            }
            else
            {
                if (match[i, 2] == -1)
                    round_2 = 1;
                else
                {
                    round_2 = 0;
                    break;
                }
            }
        }

        //準決勝終了判定
        //両対戦とも終了
        if (match[4, 2] == -1 && match[5, 2] == -1)
            semi_fin = 0;

        //少なくとも片方の対戦が終了していない
        if (match[4, 2] != -1 || match[5, 2] != -1)
            semi_fin = 1;
    }
    /// ------------------------- 対戦終了判定(ここまで) ------------------------------------------

    /// ------------------------- 各個体の評価 ----------------------------------------------------
    public void evaluation_individual()
    {
        for (int i = 0; i < 6; i++)
        {
            //Debug.Log(result[i, 1]);
            if (i < 4)
                // 一回戦負けの個体
                tournament_point[result[i, 1]] = NO_WIN_POINT;
            else
                // 準決勝負けの個体
                tournament_point[result[i, 1]] = ONE_WIN_POINT;

            // 準優勝個体
            tournament_point[result[6, 1]] = SEMI_POINT;

            // 優勝個体
            tournament_point[result[6, 0]] = TOP_POINT;
        }
    }
    /// ------------------------- 各個体の評価(ここまで) ------------------------------------------

}
