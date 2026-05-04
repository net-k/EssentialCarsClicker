using UnityEngine;

namespace stickin
{
    public static class Vector2Extensions
    {
        
        public static bool SegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            var a = p2 - p1;
            var b = p3 - p4;
            var c = p1 - p3;

            var alphaNumerator = b.y * c.x - b.x * c.y;
            var alphaDenominator = a.y * b.x - a.x * b.y;
            var betaNumerator = a.x * c.y - a.y * c.x;
            var betaDenominator = a.y * b.x - a.x * b.y;

            var result = true;

            if (alphaDenominator == 0 || betaDenominator == 0)
                result = false;
            else
            {
                if (alphaDenominator > 0)
                {
                    if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                        result = false;
                }
                else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
                    result = false;

                if (result && betaDenominator > 0)
                {
                    if (betaNumerator < 0 || betaNumerator > betaDenominator)
                        result = false;
                }
                else if (betaNumerator > 0 || betaNumerator < betaDenominator)
                    result = false;
            }

            return result;
        }
    }
}