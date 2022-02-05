using UnityEngine;
using UnityEngine.UI;

namespace Common.Extensions
{
    public static class ImageExtensions
    {
        #region Methods

        public static void SetColorAlpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = Mathf.Clamp01(alpha); // Clamp as any float that isn't between zero and one is redundant
                                            // Unity doesn't do an internal clamp and will just assigned any sized float
            image.color = color;
        }

        #endregion
    }
}