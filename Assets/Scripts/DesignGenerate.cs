using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;


public class DesignGenerate : Nail
{

    public static int[] test_L = new int[GENE_LENGTH];
    public static int[] test_R = new int[GENE_LENGTH];
    public static int[] color_1 = new int[COLOR_LENGTH];
    public static int[] color_2 = new int[COLOR_LENGTH];
    public static int[] line = new int[LINE_LENGTH];
    public static int[] french = new int[FRENCH_LENGTH];

    //ビット列をネイルデザインに変換
    public void Decode(Individual ind, int[] bits)
    {

        // ビット列を各パーツごとに分割
        for (int i = 0; i < COLOR_LENGTH; i++)
        {
            color_1[i] = bits[i];
            color_2[i] = bits[i + COLOR_LENGTH];
        }

        for (int i = 0; i < LINE_LENGTH; i++)
            line[i] = bits[2 * COLOR_LENGTH + i];

        for (int i = 0; i < FRENCH_LENGTH; i++)
            french[i] = bits[2 * FRENCH_LENGTH + i]; //bit[GENE_LENGTH - FRENCH_LENGTH + i];

        // 色のデコード
        ind.nail_color_1 = ColorDecode(color_1);
        ind.nail_color_2 = ColorDecode(color_2);

        // 境界線のデコード
        ind.line = LineDecode(line);

        // フレンチネイルの形状のデコード
        ind.french = FrenchDecode(french);
    }

    // デバッグ用の配列掃出し関数
    public void DebugArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
        }
    }

    // 色部分をRGB値に変換
    public Color ColorDecode(int[] bit)
    {
        int[] red = new int[3];
        int[] green = new int[3];
        int[] blue = new int[3];

        // ビット列をRGB毎に分割
        for (int i = 0; i < 3; i++)
        {
            red[i] = bit[i];
            green[i] = bit[i + 3];
            blue[i] = bit[i + 6];
        }

        float r = AllocateColor(ConvertDecimal(red));
        float g = AllocateColor(ConvertDecimal(green));
        float b = AllocateColor(ConvertDecimal(blue));


        Color color = new Color(r, g, b);

        return color;
    }

    // 境界線部をデコード
    public int LineDecode(int[] bit)
    {
        int line = ConvertDecimal(bit);
        return line;
    }

    // 形状部分をデコード
    public int FrenchDecode(int[] bit)
    {
        int french = ConvertDecimal(bit);
        return french;
    }

    // RGB値の割り当て
    public float AllocateColor(int col)
    {
        switch (col)
        {
            case 0:
                return 0.000f;
            case 1:
                return 0.143f;
            case 3:
                return 0.286f;
            case 2:
                return 0.429f;
            case 6:
                return 0.572f;
            case 7:
                return 0.715f;
            case 5:
                return 0.858f;
            case 4:
                return 1.000f;
        }

        Debug.Log("色の割り当てに失敗しました");
        return -1;
    }

    // ビット列を10進数に変換
    public int ConvertDecimal(int[] bit)
    {
        int dec = 0;

        for (int i = 0; i < bit.Length; i++)
        {
            dec += (int)(bit[i] * Mathf.Pow(2, i));
        }

        return dec;
    }

    // デザイン描画
    public void DrawingNail(Texture2D tex, Individual ind)
    {
        //Debug.Log("ind.french = " + ind.french + "");
        switch (ind.french)
        {
            // シンプルフレンチ
            case 0:
                DrawingAll(tex, ind.nail_color_1);
                DrawingNailLineRect(tex, ind.line);
                break;
            // ななめフレンチ
            case 1:
                DrawingAll(tex, ind.nail_color_1, normal);
                DrawingNailLineTri(tex, ind.line, slant);
                break;
            // ダブルフレンチ
            case 2:
                DrawingAll(tex, ind.nail_color_1, ind.nail_color_2);
                DrawingNailLineRect(tex, ind.line);
                break;
            // クロスフレンチ
            case 3:
                DrawingAll(tex, ind.nail_color_1, normal);
                DrawingAll(tex, ind.nail_color_2, reverse);
                DrawingNailLineTri(tex, ind.line, cross);
                break;
        }
    }

    // 境界線を描画(長方形)
    public void DrawingNailLineRect(Texture2D tex, int swch)
    {
        //Debug.Log("swch = " + swch + "");
        switch (swch)
        {
            case 0:
                Color gold = new Color(0.91f, 0.70f, 0.13f);
                DrawingAllLineRect(tex, gold);
                break;
            case 1:
                Color silver = new Color(0.5f, 0.5f, 0.5f);
                DrawingAllLineRect(tex, silver);
                break;
            case 2:
                Color white = new Color(0, 0, 0);
                DrawingAllLineRect(tex, white);
                break;
            case 3:
                Color black = new Color(1.0f, 1.0f, 1.0f);
                DrawingAllLineRect(tex, black);
                break;
        }
    }

    // 境界線を描画(三角形)
    public void DrawingNailLineTri(Texture2D tex, int swch, int slant_or_cross)
    {
        //Debug.Log("swch = " + swch + "");
        switch (swch)
        {
            case 0:
                Color gold = new Color(0.91f, 0.70f, 0.13f);
                DrawingAllLineTri(tex, gold, slant_or_cross);
                break;
            case 1:
                Color silver = new Color(0.5f, 0.5f, 0.5f);
                DrawingAllLineTri(tex, silver, slant_or_cross);
                break;
            case 2:
                Color white = new Color(0, 0, 0);
                DrawingAllLineTri(tex, white, slant_or_cross);
                break;
            case 3:
                Color black = new Color(1.0f, 1.0f, 1.0f);
                DrawingAllLineTri(tex, black, slant_or_cross);
                break;
        }
    }

    // Use this for initialization
    void Start()
    {

        //Texture2D texL = AssetDatabase.LoadAssetAtPath("Assets/Resources/Skin(1024)_L.png", typeof(Texture2D)) as Texture2D;
        //Texture2D texR = AssetDatabase.LoadAssetAtPath("Assets/Resources/Skin(1024)_R.png", typeof(Texture2D)) as Texture2D;
        //GameObject left, right;

        //left = GameObject.FindWithTag("Left_Nail");
        //right = GameObject.FindWithTag("Right_Nail");

        // 描画用のTexture2D
        //Texture2D design_L = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        //Texture2D design_R = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        //design_L = texL; design_R = texR;

        //for (int i = 0; i < GENE_LENGTH; i++)
        //{
        //    test_L[i] = Random.Range(0, 2);
        //    test_R[i] = Random.Range(0, 2);
        //}

        //Debugger.Array(test_L);
        //Debugger.Array(test_R);

        //Decode(test_L, ind_L);
        //Decode(test_R, ind_R);

        //DrawingNail(design_L, ind_L);
        //DrawingNail(design_R, ind_R);

        //string file1 = "Assets/test1.png";
        //string file2 = "Assets/test2.png";

        //byte[] pngData1 = design_L.EncodeToPNG();
        //byte[] pngData2 = design_R.EncodeToPNG();

        //File.WriteAllBytes(file1, pngData1);
        //File.WriteAllBytes(file2, pngData2);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
