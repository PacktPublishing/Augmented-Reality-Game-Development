using UnityEngine;
using System;
using System.Collections;
using packt.FoodyGO.Services;
using SQLite4Unity3d;

namespace packt.FoodyGO.Database
{
    /// <summary>
    /// Monster Inventory object for database persistance
    /// </summary>
    public class Monster
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Power { get; set; }
        public string Skills { get; set; }
        public double CaughtTimestamp { get; set; }
        public override string ToString()
        {
            return string.Format("Monster: {0}, Level: {1}, Power:{2}, Skills:{3}",
                Name, Level, Power, Skills);
        }
    }

    /// <summary>
    /// MonsterService object for tracking monster spawn locations
    /// </summary>
    public class MonsterSpawnLocation
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
