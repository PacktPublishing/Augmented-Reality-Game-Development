using UnityEngine;
using System.Collections;

namespace packt.FoodyGO.Database
{
    public class Monster 
    {
        public Mapping.MapLocation location;
        public Vector3 position;
        public double spawnTimestamp;
        public double lastHeardTimestamp;
        public double lastSeenTimestamp;
        public GameObject gameObject;
        public int footstepRange;
    }
}
