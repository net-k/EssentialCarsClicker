using System;
using System.Numerics;
using UnityEngine;

namespace KumaFramework
{
    /// <summary>
    /// HighPrecisionNumber のユニットテスト
    /// </summary>
    public class HighPrecisionNumberTests : MonoBehaviour
    {
        public void RunTests()
        {
            Debug.Log("=== HighPrecisionNumber Unit Tests ===");

            TestConstructors();
            TestAddition();
            TestMultiplication();
            TestComparison();
            TestConversion();
            TestSerialization();

            Debug.Log("=== All Tests Completed ===");
        }

        private void TestConstructors()
        {
            Debug.Log("--- Test Constructors ---");

            var hp1 = new HighPrecisionNumber(100L);
            Assert(hp1.ToLong() == 100, "Constructor(long)");

            var hp2 = new HighPrecisionNumber(123.456);
            Assert(hp2.ToDouble() > 123.4 && hp2.ToDouble() < 123.5, "Constructor(double)");

            var hp3 = new HighPrecisionNumber("999999999999");
            Assert(hp3.ToLong() == 999999999999, "Constructor(string)");

            Debug.Log("Constructors: OK");
        }

        private void TestAddition()
        {
            Debug.Log("--- Test Addition ---");

            var a = new HighPrecisionNumber(100L);
            var b = new HighPrecisionNumber(50L);
            var result = a + b;
            Assert(result.ToLong() == 150, "Addition 100 + 50");

            Debug.Log("Addition: OK");
        }

        private void TestMultiplication()
        {
            Debug.Log("--- Test Multiplication ---");

            var a = new HighPrecisionNumber(10L);
            var b = new HighPrecisionNumber(5L);
            var result = a * b;
            Assert(result.ToLong() == 50, "Multiplication 10 * 5");

            Debug.Log("Multiplication: OK");
        }

        private void TestComparison()
        {
            Debug.Log("--- Test Comparison ---");

            var a = new HighPrecisionNumber(100L);
            var b = new HighPrecisionNumber(50L);
            Assert(a > b, "100 > 50");
            Assert(b < a, "50 < 100");
            Assert(a >= a, "100 >= 100");

            Debug.Log("Comparison: OK");
        }

        private void TestConversion()
        {
            Debug.Log("--- Test Conversion ---");

            var hp = new HighPrecisionNumber(12345L);
            var d = hp.ToDouble();
            Assert(d == 12345.0, "ToDouble()");

            var l = hp.ToLong();
            Assert(l == 12345, "ToLong()");

            Debug.Log("Conversion: OK");
        }

        private void TestSerialization()
        {
            Debug.Log("--- Test Serialization ---");

            var hp1 = new HighPrecisionNumber("999999999999999");
            var str = hp1.ToString();
            var hp2 = new HighPrecisionNumber(str);
            Assert(hp1.Equals(hp2), "Serialization via ToString/Parse");

            Debug.Log("Serialization: OK");
        }

        private void Assert(bool condition, string message)
        {
            if (!condition)
                Debug.LogError($"Assertion failed: {message}");
            else
                Debug.Log($"✓ {message}");
        }
    }
}
