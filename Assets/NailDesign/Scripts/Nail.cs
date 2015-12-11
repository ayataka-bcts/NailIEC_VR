using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class Nail : MonoBehaviour {

    // 手のスキンのテクスチャ画像
	public static Texture2D tex;

    // 各指の爪にあたる部分の座標を格納した配列
    // area[0][0]：爪部分のx座標の始点，area[0][1]：爪部分のy座標の始点
    // area[1][0]：爪部分のx座標の終点，area[1][1]：爪部分のy座標の終点
    private int[,] area_sum = new int[2, 2] { {33, 737}, {59, 765} };
    private int[,] area_idx = new int[2, 2] { {39, 687}, {63, 717} };
    private int[,] area_mdl = new int[2, 2] { {37, 637}, {63, 671} };
    private int[,] area_rng = new int[2, 2] { {35, 585}, {61, 616} };
    private int[,] area_pnk = new int[2, 2] { {39, 540}, {61, 564} };

    // ネイルデザインを描画
    void DrawingArt(int[,] area, Texture2D tex)
    {
        // 座標を指定してデザインを描画
        for (int x = area[0, 0]; x < area[1, 0]; x++)
        {
            for (int y = (area[1, 1] + area[0, 1]) / 2; y < area[1, 1]; y++)
            {
                tex.SetPixel(x, y, Color.red);
            }
        }

        // テクスチャの確定
        tex.Apply();

    }

    // マテリアルを適用
    void ApplyMaterial(Texture2D tex)
    {
        Material mat = GetComponent<Renderer>().material;
        mat.mainTexture = tex;
    }

    // 全ての指に対して描画を行う
    void DrawingAll(Texture2D tex)
    {
        // 各指部分のデザイン描画
        DrawingArt(area_sum, tex);
        DrawingArt(area_idx, tex);
        DrawingArt(area_mdl, tex);
        DrawingArt(area_rng, tex);
        DrawingArt(area_pnk, tex);
    }

	// Use this for initialization
	void Start () {

        string path = "Assets/Resources/Skin(1024).png";
        Texture2D design = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

        tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;

        // ネイルデザインの描画
        DrawingAll(design);

        // マテリアルに反映
        ApplyMaterial(design);

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
