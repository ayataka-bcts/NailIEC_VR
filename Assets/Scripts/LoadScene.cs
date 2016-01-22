using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class LoadScene : Constant
{
    // 個体を格納する配列
    public static int[,] individual = new int[MEMBER, GENE_LENGTH];

    // DesignGenerateのインスタンス化
    DesignGenerate de;

    // テクスチャ読み込み用の配列
    Material mat;

    // 個体クラスのインスタンス化
    public static Individual ind;

    // 初期個体を生成する配列
    public static int[] bits;

    // テクスチャ出力用のパス
    public static string texture_out;

    // テクスチャ出力用のパス(Resource用)
    //public static string texture_resources;

    // 起動している場所のパス
    public static string path;

    // 出力のディレクトリパス
    public static string output;

    // Use this for initialization
    void Start()
    {
        // 現在起動しているパスの取得(C:/Users/kis_user/Desktop/(exe形式の名前)_Data)
        path = Application.streamingAssetsPath;
        output = path + "/Output";

        // 同じフォルダが存在すれば，中身ごと削除
        if (Directory.Exists(output) == true)
            Directory.Delete(output, true);

        // フォルダ作成
        Directory.CreateDirectory(output);

        // 生成された個体のテクスチャを保存するディレクトリ作成(世代毎)
        Directory.CreateDirectory(output + "/texture");
        for (int i = 0; i < GENERATION; i++)
            Directory.CreateDirectory(output + "/texture/Generation" + (i + 1));

        // 初期個体生成
        for (int i = 0; i < MEMBER; i++)
        {
            int[] bits = new int[GENE_LENGTH];
            for (int j = 0; j < GENE_LENGTH; j++)
                bits[j] = Random.Range(0, 2);

            // 初期個体のビット列に応じてテクスチャを作成
            CreateTexture(bits, i);
        }


    }

    // テクスチャを作成する
    public void CreateTexture(int[] bits, int number)
    {
        // 書き込んだネイルデザイン用スキン
        //Texture2D[] texs = new Texture2D[MEMBER];
        // 読み込み用のスキン
        //Texture2D[] load_texs = new Texture2D[MEMBER];

        // 個体用の配列
        //Individual[] inds = new Individual[MEMBER];

        // 一時的なgameobject格納用
        GameObject temp;

        // インスタンス化
        de = GameObject.Find("DesignGenerate").GetComponent<DesignGenerate>();
        ind = GameObject.Find("Individual").GetComponent<Individual>();

        // 読み込みと生成
        Texture2D load_tex = Resources.Load("Skin" + (number + 1), typeof(Texture2D)) as Texture2D;
        Texture2D tex = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

        // テクスチャのコピー
        Color[] copys = load_tex.GetPixels();
        tex.SetPixels(copys);
        tex.Apply();

        // デコードしてパーツごとに割り振り
        de.Decode(ind, bits);

        // デザインの描画
        de.DrawingNail(tex, ind);

        temp = GameObject.Find("Materials/Sphere" + (number + 1));
        mat = temp.GetComponent<Renderer>().material;
        mat.mainTexture = tex;

        // 出力用のディレクトリパス
        texture_out = output + "/texture/Generation" + TetsButton.generation + "/design" + (number + 1) + ".png";
        // Resource用のディレクトリパス
        //texture_resources = "Assets/Resources/Result/Generation" + TetsButton.generation + "/design" + (number + 1) + ".png";

        // PNG形式に変換
        byte[] pngData = tex.EncodeToPNG();

        // ファイルを書き込み
        File.WriteAllBytes(texture_out, pngData);

        // Resourcesに移動
        //Directory.Move(texture_out, texture_resources);

    }
}
