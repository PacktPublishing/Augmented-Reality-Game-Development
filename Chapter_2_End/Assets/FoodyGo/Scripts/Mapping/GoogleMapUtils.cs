using UnityEngine;

namespace packt.FoodyGO.Mapping
{
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
    }
}

