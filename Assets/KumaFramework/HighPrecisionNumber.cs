using System;
using System.Numerics;
using UnityEngine;

namespace KumaFramework
{
    /// <summary>
    /// 高精度数値クラス。BigInteger（整数部）と double（小数部）の組み合わせで、
    /// doubleの精度限界（10^53付近）を超える数値を扱える。
    /// 上限値（10^50）を超えないようキャップする機能を搭載。
    /// </summary>
    [System.Serializable]
    public class HighPrecisionNumber : IComparable<HighPrecisionNumber>, IEquatable<HighPrecisionNumber>
    {
        /// <summary>
        /// 最大値定数：10^50
        /// この値に到達したら、これ以上増えない（キャップされる）
        /// </summary>
        public static readonly HighPrecisionNumber MaxValue = new HighPrecisionNumber(
            BigInteger.Parse("100000000000000000000000000000000000000000000000000") // 10^50
        );

        /// <summary>
        /// 最小値定数（負数）：-10^50
        /// </summary>
        public static readonly HighPrecisionNumber MinValue = new HighPrecisionNumber(
            BigInteger.Parse("-100000000000000000000000000000000000000000000000000") // -10^50
        );

        // 整数部（無制限精度）
        public BigInteger IntegerPart { get; private set; }

        // 小数部（0.0 ～ 0.999...）
        public double FractionalPart { get; private set; }

        // 符号（-1, 0, 1）
        private int _sign;

        public HighPrecisionNumber()
        {
            IntegerPart = BigInteger.Zero;
            FractionalPart = 0.0;
            _sign = 1;
        }

        public HighPrecisionNumber(long value)
        {
            IntegerPart = new BigInteger(value);
            FractionalPart = 0.0;
            _sign = value >= 0 ? 1 : -1;
            if (value < 0)
                IntegerPart = BigInteger.Abs(IntegerPart);
        }

        public HighPrecisionNumber(BigInteger intPart, double fracPart = 0.0)
        {
            IntegerPart = BigInteger.Abs(intPart);
            FractionalPart = Math.Abs(fracPart);
            _sign = intPart >= 0 ? 1 : -1;

            // 小数部が1.0以上の場合は整数部に繰り上げ
            if (FractionalPart >= 1.0)
            {
                var carry = (long)FractionalPart;
                IntegerPart += carry;
                FractionalPart -= carry;
            }
        }

        public HighPrecisionNumber(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                IntegerPart = BigInteger.Zero;
                FractionalPart = 0.0;
                _sign = 0;
                return;
            }

            _sign = value >= 0 ? 1 : -1;
            value = Math.Abs(value);

            IntegerPart = new BigInteger((long)value);
            FractionalPart = value - (long)value;
        }

        public HighPrecisionNumber(string value)
        {
            Parse(value);
        }

        private void Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                IntegerPart = BigInteger.Zero;
                FractionalPart = 0.0;
                _sign = 1;
                return;
            }

            _sign = value.StartsWith("-") ? -1 : 1;
            value = value.TrimStart('-', '+');

            if (value.Contains("."))
            {
                var parts = value.Split('.');
                IntegerPart = BigInteger.Parse(parts[0] ?? "0");
                FractionalPart = double.Parse("0." + parts[1]);
            }
            else
            {
                IntegerPart = BigInteger.Parse(value);
                FractionalPart = 0.0;
            }
        }

        /// <summary>
        /// 絶対値を返す
        /// </summary>
        public HighPrecisionNumber Abs()
        {
            return new HighPrecisionNumber(IntegerPart, FractionalPart);
        }

        /// <summary>
        /// 符号を反転
        /// </summary>
        public HighPrecisionNumber Negate()
        {
            var result = new HighPrecisionNumber(IntegerPart, FractionalPart);
            result._sign *= -1;
            return result;
        }

        /// <summary>
        /// 加算（MAX値超過時はキャップ）
        /// </summary>
        public HighPrecisionNumber Add(HighPrecisionNumber other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            // 両方正の数の場合
            if (_sign >= 0 && other._sign >= 0)
            {
                var newFrac = FractionalPart + other.FractionalPart;
                var carry = 0L;
                var newInt = IntegerPart + other.IntegerPart;

                if (newFrac >= 1.0)
                {
                    carry = (long)newFrac;
                    newFrac -= carry;
                    newInt += carry;
                }

                var result = new HighPrecisionNumber(newInt, newFrac);
                // MAX値超過チェック
                if (result > MaxValue)
                {
                    Debug.LogWarning($"HighPrecisionNumber.Add: Result exceeded MAX_VALUE. Capping to MAX.");
                    return new HighPrecisionNumber(MaxValue.IntegerPart, MaxValue.FractionalPart);
                }
                return result;
            }

            // 両方負の数の場合
            if (_sign < 0 && other._sign < 0)
            {
                var result = Abs().Add(other.Abs());
                result._sign = -1;
                return result;
            }

            // 異なる符号の場合は減算に変換
            if (_sign < 0)
            {
                return other.Subtract(Abs());
            }
            else
            {
                return Subtract(other.Abs());
            }
        }

        /// <summary>
        /// 減算
        /// </summary>
        public HighPrecisionNumber Subtract(HighPrecisionNumber other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return Add(other.Negate());
        }

        /// <summary>
        /// 乗算（MAX値超過時はキャップ）
        /// </summary>
        public HighPrecisionNumber Multiply(HighPrecisionNumber other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            // 符号計算
            var resultSign = _sign * other._sign;

            // 整数部同士の乗算
            var newIntPart = IntegerPart * other.IntegerPart;

            // 小数部の乗算（簡略実装）
            var fracResult = FractionalPart * other.FractionalPart
                           + FractionalPart * (double)other.IntegerPart
                           + other.FractionalPart * (double)IntegerPart;

            // 小数部が1以上になった場合は整数部に繰り上げ
            if (fracResult >= 1.0)
            {
                var carry = (long)fracResult;
                newIntPart += carry;
                fracResult -= carry;
            }

            var result = new HighPrecisionNumber(newIntPart, fracResult);
            result._sign = resultSign;

            // MAX値超過チェック
            if (result._sign > 0 && result > MaxValue)
            {
                Debug.LogWarning($"HighPrecisionNumber.Multiply: Result exceeded MAX_VALUE. Capping to MAX.");
                return new HighPrecisionNumber(MaxValue.IntegerPart, MaxValue.FractionalPart);
            }
            if (result._sign < 0 && result < MinValue)
            {
                Debug.LogWarning($"HighPrecisionNumber.Multiply: Result exceeded MIN_VALUE. Capping to MIN.");
                return new HighPrecisionNumber(MinValue.IntegerPart, MinValue.FractionalPart);
            }

            return result;
        }

        /// <summary>
        /// 乗算（long値）
        /// </summary>
        public HighPrecisionNumber Multiply(long value)
        {
            return Multiply(new HighPrecisionNumber(value));
        }

        /// <summary>
        /// 乗算（double値）
        /// </summary>
        public HighPrecisionNumber Multiply(double value)
        {
            return Multiply(new HighPrecisionNumber(value));
        }

        /// <summary>
        /// 除算（簡略実装：完全な実装ではなく、近似値）
        /// </summary>
        public HighPrecisionNumber Divide(HighPrecisionNumber other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (other.IsZero)
                throw new DivideByZeroException();

            // 簡略実装：doubleで除算
            var thisDouble = this.ToDouble();
            var otherDouble = other.ToDouble();
            return new HighPrecisionNumber(thisDouble / otherDouble);
        }

        /// <summary>
        /// 除算（long値）
        /// </summary>
        public HighPrecisionNumber Divide(long value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            return Divide(new HighPrecisionNumber(value));
        }

        /// <summary>
        /// べき乗（指数が小さい場合）
        /// </summary>
        public HighPrecisionNumber Power(int exponent)
        {
            if (exponent < 0)
                throw new ArgumentException("Negative exponent not supported");

            if (exponent == 0)
                return new HighPrecisionNumber(1L);

            var result = new HighPrecisionNumber(this.IntegerPart, this.FractionalPart);
            result._sign = this._sign;

            for (int i = 1; i < exponent; i++)
            {
                result = result.Multiply(this);
            }

            return result;
        }

        /// <summary>
        /// 最大値を返す
        /// </summary>
        public static HighPrecisionNumber Max(HighPrecisionNumber a, HighPrecisionNumber b)
        {
            return a.CompareTo(b) >= 0 ? a : b;
        }

        /// <summary>
        /// 最小値を返す
        /// </summary>
        public static HighPrecisionNumber Min(HighPrecisionNumber a, HighPrecisionNumber b)
        {
            return a.CompareTo(b) <= 0 ? a : b;
        }

        /// <summary>
        /// ゼロかどうか
        /// </summary>
        public bool IsZero => IntegerPart == BigInteger.Zero && FractionalPart == 0.0;

        /// <summary>
        /// 正の数かどうか
        /// </summary>
        public bool IsPositive => _sign > 0 && !IsZero;

        /// <summary>
        /// 負の数かどうか
        /// </summary>
        public bool IsNegative => _sign < 0;

        /// <summary>
        /// MAX値（10^50）に到達したかどうか
        /// </summary>
        public bool IsMaxValue => this >= MaxValue;

        /// <summary>
        /// MIN値（-10^50）に到達したかどうか
        /// </summary>
        public bool IsMinValue => this <= MinValue;

        /// <summary>
        /// doubleに変換（精度注意）
        /// </summary>
        public double ToDouble()
        {
            try
            {
                var intPart = (double)IntegerPart;
                var result = intPart + FractionalPart;
                return _sign < 0 ? -result : result;
            }
            catch
            {
                return _sign < 0 ? double.NegativeInfinity : double.PositiveInfinity;
            }
        }

        /// <summary>
        /// longに変換（小数部は切り捨て）
        /// </summary>
        public long ToLong()
        {
            try
            {
                var result = (long)IntegerPart;
                return _sign < 0 ? -result : result;
            }
            catch
            {
                return _sign < 0 ? long.MinValue : long.MaxValue;
            }
        }

        /// <summary>
        /// 文字列化（シリアライゼーション用）
        /// </summary>
        public override string ToString()
        {
            if (IsZero) return "0";

            var sign = _sign < 0 ? "-" : "";
            if (FractionalPart == 0.0)
                return $"{sign}{IntegerPart}";
            else
                return $"{sign}{IntegerPart}.{FractionalPart.ToString("F15").TrimEnd('0').TrimEnd('.')}";
        }

        /// <summary>
        /// 簡潔な表現（整数部のみ）
        /// </summary>
        public string ToShortString()
        {
            if (IsZero) return "0";
            var sign = _sign < 0 ? "-" : "";
            return $"{sign}{IntegerPart}";
        }

        public int CompareTo(HighPrecisionNumber other)
        {
            if (other == null) return 1;

            // 符号が異なる場合
            if (_sign != other._sign)
                return _sign.CompareTo(other._sign);

            // 符号が同じ場合
            var intComparison = IntegerPart.CompareTo(other.IntegerPart);
            if (intComparison != 0)
                return _sign < 0 ? -intComparison : intComparison;

            // 小数部で比較
            var fracComparison = FractionalPart.CompareTo(other.FractionalPart);
            return _sign < 0 ? -fracComparison : fracComparison;
        }

        public bool Equals(HighPrecisionNumber other)
        {
            if (other == null) return false;
            return IntegerPart == other.IntegerPart
                && FractionalPart == other.FractionalPart
                && _sign == other._sign;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HighPrecisionNumber);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IntegerPart, FractionalPart, _sign);
        }

        // 演算子オーバーロード
        public static HighPrecisionNumber operator +(HighPrecisionNumber a, HighPrecisionNumber b) => a.Add(b);
        public static HighPrecisionNumber operator -(HighPrecisionNumber a, HighPrecisionNumber b) => a.Subtract(b);
        public static HighPrecisionNumber operator *(HighPrecisionNumber a, HighPrecisionNumber b) => a.Multiply(b);
        public static HighPrecisionNumber operator /(HighPrecisionNumber a, HighPrecisionNumber b) => a.Divide(b);
        public static HighPrecisionNumber operator *(HighPrecisionNumber a, long b) => a.Multiply(b);
        public static HighPrecisionNumber operator *(long a, HighPrecisionNumber b) => b.Multiply(a);
        public static HighPrecisionNumber operator *(HighPrecisionNumber a, double b) => a.Multiply(b);
        public static HighPrecisionNumber operator *(double a, HighPrecisionNumber b) => b.Multiply(a);
        public static bool operator >(HighPrecisionNumber a, HighPrecisionNumber b) => a.CompareTo(b) > 0;
        public static bool operator <(HighPrecisionNumber a, HighPrecisionNumber b) => a.CompareTo(b) < 0;
        public static bool operator >=(HighPrecisionNumber a, HighPrecisionNumber b) => a.CompareTo(b) >= 0;
        public static bool operator <=(HighPrecisionNumber a, HighPrecisionNumber b) => a.CompareTo(b) <= 0;
        public static bool operator ==(HighPrecisionNumber a, HighPrecisionNumber b) => a?.Equals(b) ?? (b == null);
        public static bool operator !=(HighPrecisionNumber a, HighPrecisionNumber b) => !(a == b);

        public static implicit operator HighPrecisionNumber(long value) => new HighPrecisionNumber(value);
        public static implicit operator HighPrecisionNumber(double value) => new HighPrecisionNumber(value);
        public static implicit operator HighPrecisionNumber(int value) => new HighPrecisionNumber((long)value);
    }
}
