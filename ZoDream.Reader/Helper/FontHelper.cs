using System.Windows;

namespace ZoDream.Helper
{
    public class FontHelper
    {
        public static FontWeight GetFontWeight(int value)
        {
            if (value <= 100)
            {
                return FontWeights.Thin;
            }
            if (value <= 200)
            {
                return FontWeights.ExtraLight;
            }
            if (value <= 300)
            {
                return FontWeights.Light;
            }
            if (value <= 400)
            {
                return FontWeights.Normal;
            }
            if (value <= 500)
            {
                return FontWeights.Medium;
            }
            if (value <= 600)
            {
                return FontWeights.DemiBold;
            }
            if (value <= 700)
            {
                return FontWeights.Bold;
            }
            if (value <= 800)
            {
                return FontWeights.ExtraBold;
            }
            return value <= 900 ? FontWeights.Black : FontWeights.ExtraBlack;
        }
    }
}
