using UnityEngine;
using System.Collections;

public class Constant : MonoBehaviour
{

    /// ------------------GA処理に用いる定数------------------------------

    // 世代交代のリミット
    public const int GENERATION = 50;
    //public int generation = GENERATION;

    // 個体数
    public const int MEMBER = 8;
    //public int member = MEMBER;

    // 遺伝子長
    public const int GENE_LENGTH = 22;
    //public int geneLength = GENE_LENGTH;

    // 選択処理のスケーリング
    public const int SCALENUMBER = 1;

    // 突然変異率(%)
    public const int MUTATION = 10;

    /// -----------------GA処理に用いる関数(ここまで)--------------------------------

    /// ------------------評価点関係-------------------------------------------------

    // 優勝個体の獲得点数
    public const int TOP_POINT = 10;

    // 準優勝個体の点数
    public const int SEMI_POINT = 8;

    // 一勝の個体の点数
    public const int ONE_WIN_POINT = 6;

    // 一回戦負けの個体の点数
    public const int NO_WIN_POINT = 4;

    // デザインの要素数
    public const int PATTERN = 4;
    public int pattern = PATTERN;

    // 評価点正規化の有無(0:なし  1:あり)
    public const int NORMARIZED = 1;
    public int normarized = NORMARIZED;

    /// ------------------評価点関係(ここまで)-------------------------------------------

    /// ------------------ ビット列関係 -------------------------------------------------

    public const int COLOR_LENGTH = 9;

    public const int LINE_LENGTH = 2;

    public const int FRENCH_LENGTH = 2;

    /// ----------------- ビット列関係(ここまで) ----------------------------------------

    /// ----------------- その他 --------------------------------------------------------

    // 三角形描画の際の反転
    public const int reverse = 1;

    // 四角形描画の際の大きさ
    public const int minimal = 1;

    // 図形描画を標準で行うための変数
    public const int normal = 0;

    // 境界線の太さ(単位：ピクセル)
    public const int LINE_BOLD = 3;

    // ライン描画(クロスフレンチ)
    public const int cross = 1;

    // ライン描画(ななめフレンチ)
    public const int slant = 0;

    public Color LINE_COLOR = new Color(0.715f, 1.0f, 0.004f);
}
