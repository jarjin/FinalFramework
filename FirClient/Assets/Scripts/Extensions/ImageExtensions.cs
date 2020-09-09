using UnityEngine;
using UnityEngine.UI;

namespace FirClient.Extensions
{
    public static class ImageExtensions
    {
        public static void Enable(this Image image)
        {
            if (image != null)
            {
                var c = image.color;
                image.color = new Color(c.r, c.g, c.b, 1);
            }
        }

        public static void Disable(this Image image)
        {
            if (image != null)
            {
                var c = image.color;
                image.sprite = null;
                image.color = new Color(c.r, c.g, c.b, 0);
            }
        }
    }
}
