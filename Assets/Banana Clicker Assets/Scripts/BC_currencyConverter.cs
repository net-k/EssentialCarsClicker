using UnityEngine;
using System;
using KumaFramework;

/// <summary>
/// 数値を Cookie Clicker 形式の単位付き文字列に変換するクラス。
/// 巨大な数値を Million, Billion などの単位を用いて読みやすくフォーマットします。
/// HighPrecisionNumber に対応し、無制限の大きさの数値を処理できます。
/// </summary>
public class BC_currencyConverter : MonoBehaviour
{
    // シングルトンインスタンス
    private static BC_currencyConverter instance;
    public static BC_currencyConverter Instance { get { return instance; } }

    private bool _hasLoggedLanguage = false;

    void Awake()
    {
        CreateInstance();
    }

    /// <summary>
    /// シングルトンインスタンスを作成します。
    /// </summary>
    void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private static readonly string[] JapaneseSuffixes =
    {
        "万", "億", "兆", "京", "垓", "𥝱", "穣", "溝", "澗", "正", "載", "極",
        "恒河沙", "阿僧祇", "那由他", "不可思議", "無量大数",
        "グラハム数", "TREE(3)", "∞"
    };

    // 10^6 (Million) から 10^306 (Uncentillion) まで 101 段階の単位、その先も対応
    private static readonly string[] Suffixes =
    {
        "Million",                   // 10^6
        "Billion",                   // 10^9
        "Trillion",                  // 10^12
        "Quadrillion",               // 10^15
        "Quintillion",               // 10^18
        "Sextillion",                // 10^21
        "Septillion",                // 10^24
        "Octillion",                 // 10^27
        "Nonillion",                 // 10^30
        "Decillion",                 // 10^33
        "Undecillion",               // 10^36
        "Duodecillion",              // 10^39
        "Tredecillion",              // 10^42
        "Quattuordecillion",         // 10^45
        "Quindecillion",             // 10^48
        "Sexdecillion",              // 10^51
        "Septendecillion",           // 10^54
        "Octodecillion",             // 10^57
        "Novemdecillion",            // 10^60
        "Vigintillion",              // 10^63
        "Unvigintillion",            // 10^66
        "Duovigintillion",           // 10^69
        "Tresvigintillion",          // 10^72
        "Quattuorvigintillion",      // 10^75
        "Quinvigintillion",          // 10^78
        "Sexvigintillion",           // 10^81
        "Septenvigintillion",        // 10^84
        "Octovigintillion",          // 10^87
        "Novemvigintillion",         // 10^90
        "Trigintillion",             // 10^93
        "Untrigintillion",           // 10^96
        "Duotrigintillion",          // 10^99
        "Tretrigintillion",          // 10^102
        "Quattuortrigintillion",     // 10^105
        "Quintrigintillion",         // 10^108
        "Sextrigintillion",          // 10^111
        "Septentrigintillion",       // 10^114
        "Octotrigintillion",         // 10^117
        "Novemtrigintillion",        // 10^120
        "Quadragintillion",          // 10^123
        "Unquadragintillion",        // 10^126
        "Duoquadragintillion",       // 10^129
        "Trequadragintillion",       // 10^132
        "Quattuorquadragintillion",  // 10^135
        "Quinquadragintillion",      // 10^138
        "Sexquadragintillion",       // 10^141
        "Septquadragintillion",      // 10^144
        "Octoquadragintillion",      // 10^147
        "Novemquadragintillion",     // 10^150
        "Quinquagintillion",         // 10^153
        "Unquinquagintillion",       // 10^156
        "Duoquinquagintillion",      // 10^159
        "Trequinquagintillion",      // 10^162
        "Quattuorquinquagintillion", // 10^165
        "Quinquinquagintillion",     // 10^168
        "Sexquinquagintillion",      // 10^171
        "Septquinquagintillion",     // 10^174
        "Octoquinquagintillion",     // 10^177
        "Novemquinquagintillion",    // 10^180
        "Sexagintillion",            // 10^183
        "Unsexagintillion",          // 10^186
        "Duosexagintillion",         // 10^189
        "Tresexagintillion",         // 10^192
        "Quattuorsexagintillion",    // 10^195
        "Quinsexagintillion",        // 10^198
        "Sexsexagintillion",         // 10^201
        "Septsexagintillion",        // 10^204
        "Octosexagintillion",        // 10^207
        "Novemsexagintillion",       // 10^210
        "Septuagintillion",          // 10^213
        "Unseptuagintillion",        // 10^216
        "Duoseptuagintillion",       // 10^219
        "Treseptuagintillion",       // 10^222
        "Quattuorseptuagintillion",  // 10^225
        "Quinseptuagintillion",      // 10^228
        "Sexseptuagintillion",       // 10^231
        "Septseptuagintillion",      // 10^234
        "Octoseptuagintillion",      // 10^237
        "Novemseptuagintillion",     // 10^240
        "Octogintillion",            // 10^243
        "Unoctogintillion",          // 10^246
        "Duooctogintillion",         // 10^249
        "Treoctogintillion",         // 10^252
        "Quattuoroctogintillion",    // 10^255
        "Quinoctogintillion",        // 10^258
        "Sexoctogintillion",         // 10^261
        "Septoctogintillion",        // 10^264
        "Octooctogintillion",        // 10^267
        "Novemoctogintillion",       // 10^270
        "Nonagintillion",            // 10^273
        "Unnonagintillion",          // 10^276
        "Duononagintillion",         // 10^279
        "Trenonagintillion",         // 10^282
        "Quattuornonagintillion",    // 10^285
        "Quinnonagintillion",        // 10^288
        "Sexnonagintillion",         // 10^291
        "Septnonagintillion",        // 10^294
        "Octononagintillion",        // 10^297
        "Novemnonagintillion",       // 10^300
        "Centillion",                // 10^303
        "Uncentillion",              // 10^306
        "Duocentillion",             // 10^309 (拡張)
        "Trescentillion",            // 10^312 (拡張)
    };

    /// <summary>
    /// 数値を Cookie Clicker 風の文字列に変換する（4桁有効数字 + 単位名）。
    /// </summary>
    /// <param name="valueToConvert">変換対象の数値</param>
    /// <param name="currencyPerSec">true の場合、末尾に " Bananas/Sec" を付与</param>
    /// <param name="currencyPerClick">true の場合、末尾に " Bananas/Click" を付与</param>
    /// <returns>フォーマット済みの文字列</returns>
    public string GetCurrencyIntoString(double valueToConvert, bool currencyPerSec, bool currencyPerClick)
    {
        string converted;
        bool isJapanese = IsJapaneseLanguage();

        // 無効な数値の処理
        if (double.IsNaN(valueToConvert) || double.IsInfinity(valueToConvert))
        {
            converted = "MAX";
        }
        else if (valueToConvert < 0)
        {
            converted = "0";
        }
        else if (!isJapanese && valueToConvert < 1e6)
        {
            // 英語・100万未満: 小数点以下がある場合は表示する
            if (Math.Abs(valueToConvert - Math.Floor(valueToConvert)) > 0.001)
            {
                converted = valueToConvert.ToString("N1");
            }
            else
            {
                converted = ((long)valueToConvert).ToString("N0");
            }
        }
        else if (isJapanese && valueToConvert < 1e4)
        {
            // 日本語・1万未満: カンマ区切りで表示
            if (Math.Abs(valueToConvert - Math.Floor(valueToConvert)) > 0.001)
            {
                converted = valueToConvert.ToString("N1");
            }
            else
            {
                converted = ((long)valueToConvert).ToString("N0");
            }
        }
        else
        {
            if (isJapanese)
            {
                // 日本語の場合: 1万以上: 4桁区切りの単位（万, 億, 兆...）
                int tier = (int)Math.Floor(Math.Log10(valueToConvert) / 4.0);
                int suffixIndex = tier - 1;

                double scale = Math.Pow(10000.0, tier);
                double scaled = valueToConvert / scale;

                while (scaled >= 10000.0 && suffixIndex + 1 < JapaneseSuffixes.Length)
                {
                    scaled /= 10000.0;
                    suffixIndex++;
                }

                if (suffixIndex >= 0 && suffixIndex < JapaneseSuffixes.Length)
                {
                    converted = FormatScaledJapanese(scaled) + JapaneseSuffixes[suffixIndex];
                }
                else
                {
                    converted = valueToConvert.ToString("0.000E+0");
                }
            }
            else
            {
                // 100万以上: Cookie Clicker 風
                int tier = (int)Math.Floor(Math.Log10(valueToConvert) / 3.0);
                int suffixIndex = tier - 2;

                double scale = Math.Pow(1000.0, tier);
                double scaled = valueToConvert / scale;

                while (scaled >= 1000.0 && suffixIndex + 1 < Suffixes.Length)
                {
                    scaled /= 1000.0;
                    suffixIndex++;
                }

                if (suffixIndex >= 0 && suffixIndex < Suffixes.Length)
                {
                    converted = FormatScaled(scaled) + " " + Suffixes[suffixIndex];
                }
                else
                {
                    converted = valueToConvert.ToString("0.000E+0");
                }
            }
        }

        // 接尾辞の付与
        if (currencyPerSec)   converted += " Bananas/Sec";
        if (currencyPerClick) converted += " Bananas/Click";
        return converted;
    }

    /// <summary>
    /// HighPrecisionNumber を Cookie Clicker 風の文字列に変換する
    /// </summary>
    public string GetCurrencyIntoString(HighPrecisionNumber valueToConvert, bool currencyPerSec, bool currencyPerClick)
    {
        if (valueToConvert == null || valueToConvert.IsZero)
            return "0";

        // doubleに変換不可の場合は、BigIntegerの桁数で判定
        if (valueToConvert.IntegerPart.ToString().Length > 300)
        {
            // 超巨大数値
            bool isJapanese = IsJapaneseLanguage();
            string suffix = isJapanese ? "∞" : "∞";
            return $"超{suffix}" + (currencyPerSec ? " Bananas/Sec" : "") + (currencyPerClick ? " Bananas/Click" : "");
        }

        try
        {
            var d = valueToConvert.ToDouble();
            if (double.IsInfinity(d))
            {
                bool isJapanese = IsJapaneseLanguage();
                return $"超巨大数値" + (currencyPerSec ? " Bananas/Sec" : "") + (currencyPerClick ? " Bananas/Click" : "");
            }
            return GetCurrencyIntoString(d, currencyPerSec, currencyPerClick);
        }
        catch
        {
            bool isJapanese = IsJapaneseLanguage();
            return $"計算不可" + (currencyPerSec ? " Bananas/Sec" : "") + (currencyPerClick ? " Bananas/Click" : "");
        }
    }

    /// <summary>
    /// 1.0〜999.9 の範囲の値を 4桁有効数字でフォーマットする
    /// </summary>
    private static string FormatScaled(double value)
    {
        if (value >= 100.0) return value.ToString("F1");
        if (value >= 10.0)  return value.ToString("F2");
        return value.ToString("F3");
    }

    /// <summary>
    /// 日本語向けに 1.0〜9999.9 の範囲の値をフォーマットする
    /// </summary>
    private static string FormatScaledJapanese(double value)
    {
        if (value >= 1000.0) return value.ToString("F0");
        if (value >= 100.0)  return value.ToString("F1");
        if (value >= 10.0)   return value.ToString("F2");
        return value.ToString("F3");
    }

    /// <summary>
    /// 現在の言語設定が日本語かどうかを判定する
    /// </summary>
    private bool IsJapaneseLanguage()
    {
        string currentLang = "Unknown";
        if (I2.Loc.LocalizationManager.CurrentLanguage != null)
        {
            currentLang = I2.Loc.LocalizationManager.CurrentLanguage;
        }

        bool isJapanese = currentLang.Contains("ja") || currentLang.Contains("日本語");

        if (!_hasLoggedLanguage)
        {
            Debug.Log($"[BC_currencyConverter] CurrentLanguage: '{currentLang}', IsJapanese: {isJapanese}");
            _hasLoggedLanguage = true;
        }
        
        return isJapanese;
    }
}
