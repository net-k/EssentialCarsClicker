using UnityEngine;

namespace stickin
{
    public static class RectTransformExtension
    {
        public static Vector3 ResizeInParent(this RectTransform current, float minScale = 0f, float maxScale = 1f)
        {
            return current.ResizeInRT(current.parent, minScale, maxScale);
        }

        public static Vector3 ResizeInRT(this RectTransform current, Transform parent, float minScale = 0f,
            float maxScale = 1f)
        {
            return current.ResizeRTinRT(parent as RectTransform, minScale, maxScale);
        }

        public static Vector3 ResizeRTinRT(this RectTransform current, RectTransform parent, float minScale = 0f,
            float maxScale = 1f)
        {
            var scale = Mathf.Min(
                parent.rect.width / current.rect.width,
                parent.rect.height / current.rect.height);

            current.localScale = Vector3.one * Mathf.Clamp(scale, minScale, maxScale);
            return current.localScale;
        }

        public static void ResizeForContent(this RectTransform rt)
        {
            var minX = 0f;
            var minY = 0f;
            var maxX = 0f;
            var maxY = 0f;

            for (var i = 0; i < rt.childCount; i++)
            {
                var childRt = rt.GetChild(i) as RectTransform;

                // var mminX = childRt.anchoredPosition.x - childRt.sizeDelta.x / 2;
                // var mminY = childRt.anchoredPosition.y - childRt.sizeDelta.y / 2;
                // var mmaxX = childRt.anchoredPosition.x + childRt.sizeDelta.x / 2;
                // var mmaxY = childRt.anchoredPosition.y + childRt.sizeDelta.y / 2;
                
                var mminX = childRt.anchoredPosition.x - childRt.sizeDelta.x * childRt.pivot.x;
                var mminY = childRt.anchoredPosition.y - childRt.sizeDelta.y * childRt.pivot.y;
                var mmaxX = childRt.anchoredPosition.x + childRt.sizeDelta.x * (1 - childRt.pivot.x);
                var mmaxY = childRt.anchoredPosition.y + childRt.sizeDelta.y * (1 - childRt.pivot.y);

                if (i == 0)
                {
                    minX = mminX;
                    minY = mminY;
                    maxX = mmaxX;
                    maxY = mmaxY;
                }
                else
                {
                    minX = Mathf.Min(minX, mminX);
                    minY = Mathf.Min(minY, mminY);
                    maxX = Mathf.Max(maxX, mmaxX);
                    maxY = Mathf.Max(maxY, mmaxY);
                }
            }

            for (var i = 0; i < rt.childCount; i++)
            {
                var childRt = rt.GetChild(i) as RectTransform;
                childRt.anchoredPosition -= new Vector2(minX, minY);
            }

            rt.sizeDelta = new Vector2(maxX - minX, maxY - minY);
        }
    }
}