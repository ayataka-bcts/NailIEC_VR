using UnityEngine;
using System.Collections;

public class DesignGenerate : Nail {

    int[] test = new int[GENE_LENGTH];


    //ビット列をネイルデザインに変換
    public void Decode(int[] bit)
    {
        int[] color_1 = new int[COLOR_LENGTH];
        int[] color_2 = new int[COLOR_LENGTH];
        int[] line    = new int[LINE_LENGTH];
        int[] french  = new int[FRENCH_LENGTH];

        // ビット列を各パーツごとに分割
        for (int i = 0; i < COLOR_LENGTH; i++)
        {
            color_1[i] = bit[i];
            color_2[i] = bit[i + COLOR_LENGTH];
        }

        for (int i = 0; i < LINE_LENGTH; i++)
            line[i] = bit[2 * COLOR_LENGTH + i];

        for (int i = 0; i < FRENCH_LENGTH; i++)
            french[i] = bit[GENE_LENGTH - FRENCH_LENGTH + i];

        // 色のデコード
        ColorDecode(color_1);
        ColorDecode(color_2);

        // 境界線のデコード

        // フレンチネイルの形状のデコード

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

    public int LineDecode(int[] bit)
    {
        return 0;
    }

    public int FrenchDecode(int[] bit)
    {
        return 0;
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
            dec += bit[i];
        }

        return dec;
    }

	// Use this for initialization
	void Start () {

        for (int i = 0; i < GENE_LENGTH; i++)
        {
            test[i] = Random.Range(0, 1);
        }

        Decode(test);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
