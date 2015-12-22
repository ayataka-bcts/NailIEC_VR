using UnityEngine;
using UnityEditor;
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

    // 長方形の描画
    ///(DrawTexture2D使った方がいいかも)
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
        {
            colors[i] = col;
        }

         // 範囲の半分位置から描画
        tex.SetPixels(area[0, 0], area[0, 1] + (height / 2), width, (height / 2), colors);

        // テクスチャの確定
        tex.Apply();
    }

    // 三角形の描画
    public void DrawingTriangle(int[,] area, Texture2D tex, Color col)
    {
        //int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1];
        Color[] colors = new Color[height];

        for (int i = 0; i < height; i++)
        {
            colors[i] = col;
        }

        for (int y = area[0, 1] + ((1/ 4) * height); y < area[1, 1]; y++)
        {
            tex.SetPixels(area[0, 0], y, y - (area[0, 1] + ((3 / 4) * height)) + 1, 1, colors);
        }

        // テクスチャの確定
        tex.Apply();
    }

    // 三角形の描画
    public void DrawingTriangle2(int[,] area, Texture2D tex, Color col)
    {
        //int width = area[1, 0] - area[0, 0];
        int height = area[1, 1] - area[0, 1];
        Color[] colors = new Color[height];

        for (int i = 0; i < height; i++)
        {
            colors[i] = col;
        }

        for (int y = area[0, 1]; y > area[1, 1] + height * (1 / 4); y--)
        {
            tex.SetPixels(area[1, 0], y, y - (area[0, 1] + ((3 / 4) * height)) + 1, 1, colors);
        }

        // テクスチャの確定
        tex.Apply();
    }

    // 境界線の描画
    public void DrawingLine(int[,] area, Texture2D tex, Color col)
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

    // マテリアルを適用
    public void ApplyMaterial(Texture2D tex, GameObject obj)
    {
        Material mat = obj.GetComponent<Renderer>().material;
        mat.mainTexture = tex;
    }

    // 全ての指に対して描画を行う
    public void DrawingAllRect(Texture2D tex, Color col)
    {
        // 各指部分のデザイン描画
        DrawingRectangle(area_sum, tex, col);
        DrawingRectangle(area_idx, tex, col);
        DrawingRectangle(area_mdl, tex, col);
        DrawingRectangle(area_rng, tex, col);
        DrawingRectangle(area_pnk, tex, col);
    }

    // 全ての指に対して描画を行う
    public void DrawingAllTri(Texture2D tex, Color col)
    {
        // 各指部分のデザイン描画
        DrawingTriangle(area_sum, tex, col);
        DrawingTriangle(area_idx, tex, col);
        DrawingTriangle(area_mdl, tex, col);
        DrawingTriangle(area_rng, tex, col);
        DrawingTriangle(area_pnk, tex, col);
    }

    // 全ての指に対して描画を行う
    public void DrawingAllTri2(Texture2D tex, Color col)
    {
        // 各指部分のデザイン描画
        DrawingTriangle2(area_sum, tex, col);
        DrawingTriangle2(area_idx, tex, col);
        DrawingTriangle2(area_mdl, tex, col);
        DrawingTriangle2(area_rng, tex, col);
        DrawingTriangle2(area_pnk, tex, col);
    }

    // 全ての指に対して描画を行う
    public void DrawingAllLine(Texture2D tex, Color col)
    {
        DrawingLine(area_sum, tex, col);
        DrawingLine(area_idx, tex, col);
        DrawingLine(area_mdl, tex, col);
        DrawingLine(area_rng, tex, col);
        DrawingLine(area_pnk, tex, col);
    }

	// Use this for initialization
	void Start () {

        tex = AssetDatabase.LoadAssetAtPath("Assets/Resources/Skin(1024).png", typeof(Texture2D)) as Texture2D;
        
        // 描画用のTexture2D
        Texture2D design = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

        // コピー
        design = tex;

        // ネイルデザインの描画
        //DrawingAll(design);

        //DrawingAll2(design);

        // マテリアルに反映
        //ApplyMaterial(design);

        string file = "Assets/test.png";
        byte[] pngData = design.EncodeToPNG();
        File.WriteAllBytes(file, pngData);

        Object.DestroyImmediate(tex);
        tex = AssetDatabase.LoadAssetAtPath(file, typeof(Texture2D)) as Texture2D;

	}
	
	// Update is called once per frame
	void Update () {
	}
}
