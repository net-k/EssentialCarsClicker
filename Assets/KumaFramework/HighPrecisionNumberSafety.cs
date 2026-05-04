using UnityEngine;
using System;

namespace KumaFramework
{
    /// <summary>
    /// HighPrecisionNumber の安全なファクトリメソッド・ユーティリティ集。
    /// MAX値超過を防ぎ、ゲーム中のバグを回避する。
    /// </summary>
    public static class HighPrecisionNumberSafety
    {
        /// <summary>
        /// 安全に加算を行う。超過時は MAX_VALUE にキャップ
        /// </summary>
        public static HighPrecisionNumber SafeAdd(HighPrecisionNumber a, HighPrecisionNumber b)
        {
            if (a == null) a = new HighPrecisionNumber(0);
            if (b == null) b = new HighPrecisionNumber(0);

            var result = a + b;
            if (result.IsMaxValue)
            {
                Debug.LogWarning($"HighPrecisionNumberSafety.SafeAdd: Result capped to MAX_VALUE");
                return new HighPrecisionNumber(HighPrecisionNumber.MaxValue.IntegerPart, HighPrecisionNumber.MaxValue.FractionalPart);
            }
            return result;
        }

        /// <summary>
        /// 安全に乗算を行う。超過時は MAX_VALUE にキャップ
        /// </summary>
        public static HighPrecisionNumber SafeMultiply(HighPrecisionNumber a, HighPrecisionNumber b)
        {
            if (a == null) a = new HighPrecisionNumber(0);
            if (b == null) b = new HighPrecisionNumber(0);

            var result = a * b;
            if (result.IsMaxValue)
            {
                Debug.LogWarning($"HighPrecisionNumberSafety.SafeMultiply: Result capped to MAX_VALUE");
                return new HighPrecisionNumber(HighPrecisionNumber.MaxValue.IntegerPart, HighPrecisionNumber.MaxValue.FractionalPart);
            }
            return result;
        }

        /// <summary>
        /// 安全に乗算を行う（long版）
        /// </summary>
        public static HighPrecisionNumber SafeMultiply(HighPrecisionNumber a, long b)
        {
            if (a == null) a = new HighPrecisionNumber(0);
            return SafeMultiply(a, new HighPrecisionNumber(b));
        }

        /// <summary>
        /// 安全に乗算を行う（double版）
        /// </summary>
        public static HighPrecisionNumber SafeMultiply(HighPrecisionNumber a, double b)
        {
            if (a == null) a = new HighPrecisionNumber(0);
            return SafeMultiply(a, new HighPrecisionNumber(b));
        }

        /// <summary>
        /// 値がMAX値に到達したかを確認
        /// </summary>
        public static bool IsAtMaxLimit(HighPrecisionNumber value)
        {
            if (value == null) return false;
            return value.IsMaxValue;
        }

        /// <summary>
        /// 値がMIN値に到達したかを確認
        /// </summary>
        public static bool IsAtMinLimit(HighPrecisionNumber value)
        {
            if (value == null) return false;
            return value.IsMinValue;
        }

        /// <summary>
        /// 加算結果をプレビュー（実際には加算しない）し、超過するかチェック
        /// </summary>
        public static bool WouldExceedMaxOnAdd(HighPrecisionNumber a, HighPrecisionNumber b)
        {
            if (a == null || b == null) return false;
            var preview = a + b;
            return preview > HighPrecisionNumber.MaxValue;
        }

        /// <summary>
        /// 乗算結果をプレビュー（実際には乗算しない）し、超過するかチェック
        /// </summary>
        public static bool WouldExceedMaxOnMultiply(HighPrecisionNumber a, HighPrecisionNumber b)
        {
            if (a == null || b == null) return false;
            try
            {
                var preview = a * b;
                return preview > HighPrecisionNumber.MaxValue;
            }
            catch
            {
                // 計算が失敗した場合も超過として扱う
                return true;
            }
        }

        /// <summary>
        /// ゲーム進行不可状態の通知（UI等で使用）
        /// </summary>
        public static void NotifyMaxReached(string valueName = "Value")
        {
            Debug.LogWarning($"HighPrecisionNumberSafety: {valueName} has reached MAX_VALUE. Further progression is limited.");
        }
    }
}
