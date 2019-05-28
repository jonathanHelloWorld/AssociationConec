using UnityEngine;

namespace Assets.Scripts.Extension
{
    public static class RectTransformExtension
    {
        public static void SetUI(this RectTransform rect, Vector2 anchoredPosition, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta, Vector2 pivot)
        {
            rect.localScale = Vector3.one;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.anchorMax = anchorMax;
            rect.anchorMin = anchorMin;
            rect.sizeDelta = sizeDelta;
        }
    }
}