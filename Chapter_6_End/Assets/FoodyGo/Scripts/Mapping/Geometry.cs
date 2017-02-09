using UnityEngine;

namespace packt.FoodyGO.Mapping
{
    [System.Serializable]

    public class MapLocation
    {
        public float Latitude;
        public float Longitude;

        public MapLocation(float longitude, float latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }

    public class MapEnvelope
    {
        private float lon1;
        private float lon2;
        private float lat1;
        private float lat2;        

        public MapEnvelope(float lon1, float lat1, float lon2, float lat2)
        {
            this.lon1 = lon1;
            this.lon2 = lon2;
            this.lat1 = lat1;
            this.lat2 = lat2;
        }

        public bool Contains(MapLocation loc)
        {
            var xMin = Mathf.Min(lon1, lon2);
            var xMax = Mathf.Max(lon1, lon2);
            var yMin = Mathf.Min(lat1, lat2);
            var yMax = Mathf.Max(lat1, lat2);

            if ((loc.Longitude >= xMin) &&
                (loc.Longitude <= xMax) &&
                (loc.Latitude >= yMin) &&
                (loc.Latitude <= yMax)) return true;
            else return false;
        }
    }
}
