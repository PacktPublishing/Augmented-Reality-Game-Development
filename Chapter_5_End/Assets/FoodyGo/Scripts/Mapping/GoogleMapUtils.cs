using UnityEngine;

namespace packt.FoodyGO.Mapping
{
    /// <summary>
    /// Original Author - RALPH BARBAGALLO
    /// https://gist.github.com/flarb/4980598
    /// Source has been extended
    /// </summary>
    public class GoogleMapUtils
    {
        static float GOOGLE_OFFSET = 268435456f;
        static float GOOGLE_OFFSET_RADIUS = 85445659.44705395f;  //GOOGLEOFFSET / Mathf.PI;
        static float MATHPI_180 = Mathf.PI / 180f;

        static private float preLonToX1 = GOOGLE_OFFSET_RADIUS * (Mathf.PI / 180f);

        public static int LonToX(float lon)
        {
            return ((int)Mathf.Round(GOOGLE_OFFSET + preLonToX1 * lon));
        }

        public static int LatToY(float lat)
        {
            return (int)Mathf.Round(GOOGLE_OFFSET - GOOGLE_OFFSET_RADIUS * Mathf.Log((1f + Mathf.Sin(lat * MATHPI_180)) / (1f - Mathf.Sin(lat * MATHPI_180))) / 2f);
        }

        public static float XToLon(float x)
        {
            return ((Mathf.Round(x) - GOOGLE_OFFSET) / GOOGLE_OFFSET_RADIUS) * 180f / Mathf.PI;
        }

        public static float YToLat(float y)
        {
            return (Mathf.PI / 2f - 2f * Mathf.Atan(Mathf.Exp((Mathf.Round(y) - GOOGLE_OFFSET) / GOOGLE_OFFSET_RADIUS))) * 180f / Mathf.PI;
        }

        public static float adjustLonByPixels(float lon, int delta, int zoom)
        {
            return XToLon(LonToX(lon) + (delta << (21 - zoom)));
        }

        public static float adjustLatByPixels(float lat, int delta, int zoom)
        {
            return YToLat(LatToY(lat) + (delta << (21 - zoom)));
        }

        public static float CalculateScaleX(float lat, int tileSizePixels, int tileSizeUnits, int zoom)
        {
            var offset = adjustLatByPixels(lat, tileSizePixels, zoom);
            var y0 = LatToY(lat); var y1 = LatToY(offset);
            var rng = y1 - y0;
            return (float)tileSizeUnits / (float)rng;
        }

        public static float CalculateScaleY(float lon, int tileSizePixels, int tileSizeUnits, int zoom)
        {
            var offset = adjustLonByPixels(lon, tileSizePixels, zoom);
            var x0 = LonToX(lon); var x1 = LonToX(offset);
            var rng = x1 - x0;
            return (float)tileSizeUnits / (float)rng;
        }

        
    }
}

