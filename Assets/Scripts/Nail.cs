using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Collections;

public class Nail : Constant {

    // 手のスキンのテクスチャ画像
    public Texture2D tex;

    // 各指の爪にあたる部分の座標を格納した配列
    // area[0][0]：爪部分のx座標の始点，area[0][1]：爪部分のy座標の始点
    // area[1][0]：爪部分のx座標の終点，area[1][1]：爪部分のy座標の終点
    private int[,] area_sum = new int[2, 2] { {33, 737}, {59, 770} };
    private int[,] area_idx = new int[2, 2] { {39, 687}, {63, 717} };
    private int[,] area_mdl = new int[2, 2] { {37, 637}, {63, 671} };
    private int[,] area_rng = new int[2, 2] { {35, 585}, {61, 621} };
    private int[,] area_pnk = new int[2, 2] { {39, 540}, {61, 569} };

    ///(図形描画に関して：DrawTexture2D使った方がいいかも)
    
    // ダブルフレンチ描画用の長方形
    public void DrawingRectangle(int[,] area, Texture2D tex, Color col_1, Color col_2)
    {
        int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1]; 
        Color[] colors_1 = new Color[(width * height) / 4];
        Color[] colors_2 = new Color[(width * height) / 4];

        // 座標を指定してデザインを描画
        //for (int x = area[0, 0]; x < area[1, 0]; x++)
        //{
        //    for (int y = (area[1, 1] + area[0, 1]) / 2; y < area[1, 1]; y++)
        //    {
        //       tex.SetPixel(x, y, Color.red);
        //       
        //    }
        //}

        // 塗りつぶす分だけのピクセルを配列に格納
        for (int i = 0; i < (width * height) / 4; i++)
        {
            colors_1[i] = col_1;
            colors_2[i] = col_2;
        }

        // 範囲の半分位置から全体の1/4だけ描画
        tex.SetPixels(area[0, 0], area[0, 1] + (height / 2), width, (height / 4), colors_1);

        // 範囲の半分位置から残りの1/4描画
        tex.SetPixels(area[0, 0], area[0, 1] + (3 * height / 4), width, (height / 4), colors_2);

        // テクスチャの確定
        tex.Apply();
    }

    // シンプルフレンチ描画用の長方形
    public void DrawingRectangle(int[,] area, Texture2D tex, Color col)
    {
        int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1];
        Color[] colors = new Color[width * height];

        // 座標を指定してデザインを描画
        //for (int x = area[0, 0]; x < area[1, 0]; x++)
        //{
        //    for (int y = (area[1, 1] + area[0, 1]) / 2; y < area[1, 1]; y++)
        //    {
        //       tex.SetPixel(x, y, Color.red);
        //       
        //    }
        //}

        // 塗りつぶす分だけのピクセルを配列に格納
        for (int i = 0; i < (width * height) / 2; i++)
            colors[i] = col;

        // 範囲の半分位置から描画
        tex.SetPixels(area[0, 0], area[0, 1] + (height / 2), width, (height / 2), colors);

        // テクスチャの確定
        tex.Apply();
    }

    // 三角形の描画
    public void DrawingTriangle(int[,] area, Texture2D tex, Color col, int normal_or_reverse)
    {
        int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1];
        Color[] colors = new Color[height];

        for (int i = 0; i < width + 1; i++)
            colors[i] = col;

        if (normal_or_reverse == normal)
        {
            for (int y = area[0, 1] + (height / 4); y < area[1, 1]; y++)
                tex.SetPixels(area[0, 0], y, y - (area[0, 1] + (height / 4)) + 1, 1, colors);
        }
        else
        {
            for (int y = area[1, 1] - 1; y >= area[0, 1] + (height / 4) - 1; y--)
                tex.SetPixels((area[0, 1] + (height / 4) - y) + area[0, 0] + width, y, y - (area[0, 1] + (height / 4)) + 1, 1, colors);
        }

        // テクスチャの確定
        tex.Apply();
    }

    // 境界線の描画(長方形)
    public void DrawingLineRect(int[,] area, Texture2D tex, Color col)
    {
        int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1];
        Color[] colors = new Color[width * LINE_BOLD];

        for (int i = 0; i < width * LINE_BOLD; i++)
        {
            colors[i] = col;
        }

        tex.SetPixels(area[0, 0], area[0, 1] + (height / 2) - 3, width, LINE_BOLD, colors);

        // テクスチャの確定
        tex.Apply();
    }

    // 境界線の描画(三角形)
    public void DrawingLineTri(int[,] area, Texture2D tex, Color col, int slant_or_cross)
    {
        int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1];
        Color[] colors = new Color[width * LINE_BOLD];

        for (int i = 0; i < width * LINE_BOLD; i++)
        {
            colors[i] = col;
        }

        for (int i = 0; i < width - LINE_BOLD; i++)
            tex.SetPixels(area[0, 0] + i, area[0, 1] + (height / 4) + i, LINE_BOLD, 1, colors);

        if (slant_or_cross == cross)
        {
            for (int i = 0; i < width - LINE_BOLD; i++)
               tex.SetPixels(area[0, 0]  + 2 + i, area[1, 1] - 2 - i, LINE_BOLD, 1, colors);
        }

        // テクスチャの確定
        tex.Apply();
    }

    // マテリアルを適用
    public void ApplyMaterial(Texture2D tex, GameObject obj)
    {
        Material mat = obj.GetComponent<Renderer>().material;
        mat.mainTexture = tex;
    }

    // 全ての指に対して描画を行う(シンプル)
    public void DrawingAll(Texture2D tex, Color col)
    {
        // 各指部分のデザイン描画
        DrawingRectangle(area_sum, tex, col);
        DrawingRectangle(area_idx, tex, col);
        DrawingRectangle(area_mdl, tex, col);
        DrawingRectangle(area_rng, tex, col);
        DrawingRectangle(area_pnk, tex, col);
    }

    // 全ての指に対して描画を行う(ダブル)
    public void DrawingAll(Texture2D tex, Color col_1, Color col_2)
    {
        // 各指部分のデザイン描画
        DrawingRectangle(area_sum, tex, col_1, col_2);
        DrawingRectangle(area_idx, tex, col_1, col_2);
        DrawingRectangle(area_mdl, tex, col_1, col_2);
        DrawingRectangle(area_rng, tex, col_1, col_2);
        DrawingRectangle(area_pnk, tex, col_1, col_2);
    }

    // 全ての指に対して描画を行う(ななめ/クロス)
    public void DrawingAll(Texture2D tex, Color col, int normal_or_reverse)
    {
        // 各指部分のデザイン描画
        DrawingTriangle(area_sum, tex, col, normal_or_reverse);
        DrawingTriangle(area_idx, tex, col, normal_or_reverse);
        DrawingTriangle(area_mdl, tex, col, normal_or_reverse);
        DrawingTriangle(area_rng, tex, col, normal_or_reverse);
        DrawingTriangle(area_pnk, tex, col, normal_or_reverse);
    }

    // 全ての指に対して描画を行う(長方形ライン)
    public void DrawingAllLineRect(Texture2D tex, Color col)
    {
        DrawingLineRect(area_sum, tex, col);
        DrawingLineRect(area_idx, tex, col);
        DrawingLineRect(area_mdl, tex, col);
        DrawingLineRect(area_rng, tex, col);
        DrawingLineRect(area_pnk, tex, col);
    }

    // 全ての指に対して描画を行う(三角形ライン)
    public void DrawingAllLineTri(Texture2D tex, Color col, int slant_or_cross)
    {
        DrawingLineTri(area_sum, tex, col, slant_or_cross);
        DrawingLineTri(area_idx, tex, col, slant_or_cross);
        DrawingLineTri(area_mdl, tex, col, slant_or_cross);
        DrawingLineTri(area_rng, tex, col, slant_or_cross);
        DrawingLineTri(area_pnk, tex, col, slant_or_cross);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}
}
