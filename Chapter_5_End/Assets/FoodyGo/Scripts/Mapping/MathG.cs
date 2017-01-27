using UnityEngine;
using System.Collections;
using System;

namespace packt.FoodyGO.Mapping
{
    public class MathG
    {
        /// <summary>
        /// Haversine calculation of distance
        /// Converted from javascript
        /// Original Author - Andrew Hedges, andrew(at)hedges(dot)name
        /// http://andrew.hedges.name/experiments/haversine/
        /// </summary>        
        public static float Distance(MapLocation mp1, MapLocation mp2)
        {
            double R = 6371.0; //avg radius of earth in km

            //convert to double in order to increase
            //precision and avoid rounding errors
            double lat1 = mp1.Latitude;
            double lat2 = mp2.Latitude;
            double lon1 = mp1.Longitude;
            double lon2 = mp2.Longitude;

            // convert coordinates to radians
            lat1 = deg2rad(lat1);
            lon1 = deg2rad(lon1);
            lat2 = deg2rad(lat2);
            lon2 = deg2rad(lon2);

            // find the differences between the coordinates
            var dlat = (lat2 - lat1);
            var dlon = (lon2 - lon1);

            // haversine formula 
            var a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));            
            var d = c * R; 

            //convert back to float and from km to m
            return (float)d * 1000;
        }

        // convert degrees to radians
        public static double deg2rad(double deg)
        {
            var rad = deg * Math.PI / 180; // radians = degrees * pi/180
            return rad;
        }

        public static float Distance(float x1, float y1, float x2, float y2)
        {
            return Distance(new MapLocation(x1, y1), new MapLocation(x2, y2));
        }
    }
}
