using packt.FoodyGo.Utils;
using packt.FoodyGO.Mapping;
using System.Collections;
using UnityEngine;

namespace packt.FoodyGO.Services
{
    [AddComponentMenu("Services/GPSLocationService")]
    public class GPSLocationService : MonoBehaviour
    {
        //Redraw Event
        public delegate void OnRedrawEvent(GameObject g);
        public event OnRedrawEvent OnMapRedraw;
        [Header("GPS Accuracy")]
        public float DesiredAccuracyInMeters = 10f;
        public float UpdateAccuracyInMeters = 10f;

        [Header("Map Tile Parameters")]
        public int MapTileScale;
        public int MapTileSizePixels;
        public int MapTileZoomLevel;

        [Header("GPS Simulation Settings")]
        public bool Simulating;
        public MapLocation StartCoordinates;        
        public float Rate = 1f;
        public Vector2[] SimulationOffsets;
        private int simulationIndex;

        [Header("Exposed for GPS Debugging Purposes Only")]
        public bool IsServiceStarted;
        public float Latitude;
        public float Longitude;
        public float Altitude;
        public float Accuracy;
        public double Timestamp;
        public double PlayerTimestamp;
        public MapLocation mapCenter;
        public MapEnvelope mapEnvelope;        
        public Vector3 mapWorldCenter;
        public Vector2 mapScale;
        

		//initialize the object
        void Start()
        {
            print("Starting GPSLocationService");

#if !UNITY_EDITOR
            StartCoroutine(StartService());
            Simulating = false;
#else            
            StartCoroutine(StartSimulationService());
            Latitude = StartCoordinates.Latitude;
            Longitude = StartCoordinates.Longitude;
            Accuracy = 10;
            Timestamp = 0;
            CenterMap();
#endif
        }

        IEnumerator StartSimulationService()
        {
            while (Simulating)
            {
                IsServiceStarted = true;

                if (simulationIndex++ >= SimulationOffsets.Length-1)
                {
                    simulationIndex = 0;
                }

                Longitude += SimulationOffsets[simulationIndex].x;
                Latitude += SimulationOffsets[simulationIndex].y;

                PlayerTimestamp = Epoch.Now;

                yield return new WaitForSeconds(Rate);
            }
            IsServiceStarted = false;
        }

		//StartService is a coroutine, to avoid blocking as the location service is started
        IEnumerator StartService()
        {
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                print("location not enabled by user, exiting");
                yield break;
            }

            // Start service before querying location            
            Input.location.Start(DesiredAccuracyInMeters, UpdateAccuracyInMeters);

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location.");
                yield break;
            }
            else
            {
                //make sure simulation is disbaled
                Simulating = false;
                print("GSPLocationService started");                
                // Access granted and location value could be retrieved
                print("Location initialized at: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                CenterMap();
                IsServiceStarted = true;
            }

           
        }

		//called once per frame
        void Update()
        {
            if(Input.location.status == LocationServiceStatus.Running  && IsServiceStarted)
            {
                //updates the public values that can be consumed by other game objects
                Latitude = Input.location.lastData.latitude;
                Longitude = Input.location.lastData.longitude;
                Altitude = Input.location.lastData.altitude;
                Accuracy = Input.location.lastData.horizontalAccuracy;
                PlayerTimestamp = Input.location.lastData.timestamp;
                MapLocation loc = new MapLocation(Input.location.lastData.longitude, Input.location.lastData.latitude);
                if (mapEnvelope.Contains(loc) == false)
                {
                    Timestamp = Input.location.lastData.timestamp;
                    CenterMap();
                }
            }
            else if (Simulating && IsServiceStarted)
            {                                
                MapLocation loc = new MapLocation(Longitude, Latitude);
                if (mapEnvelope.Contains(loc) == false)
                {
                    Timestamp = PlayerTimestamp;
                    CenterMap();
                }
            }
        }

        public void MapRedrawn()
        {
            if(OnMapRedraw != null)
            {
                OnMapRedraw(this.gameObject);
            }
        }

        private void CenterMap()
        {
            mapCenter.Latitude = Latitude;
            mapCenter.Longitude = Longitude;
            mapWorldCenter.x = GoogleMapUtils.LonToX(mapCenter.Longitude);
            mapWorldCenter.y = GoogleMapUtils.LatToY(mapCenter.Latitude);

            mapScale.x = GoogleMapUtils.CalculateScaleX(Latitude, MapTileSizePixels, MapTileScale, MapTileZoomLevel);
            mapScale.y = GoogleMapUtils.CalculateScaleY(Longitude, MapTileSizePixels, MapTileScale, MapTileZoomLevel);

            var lon1 = GoogleMapUtils.adjustLonByPixels(Longitude, -MapTileSizePixels/2, MapTileZoomLevel);
            var lat1 = GoogleMapUtils.adjustLatByPixels(Latitude, MapTileSizePixels/2, MapTileZoomLevel);

            var lon2 = GoogleMapUtils.adjustLonByPixels(Longitude, MapTileSizePixels/2, MapTileZoomLevel);
            var lat2 = GoogleMapUtils.adjustLatByPixels(Latitude, -MapTileSizePixels/2, MapTileZoomLevel);

            mapEnvelope = new MapEnvelope(lon1, lat1, lon2, lat2);
        }

        //called when the object is destroyed
        void OnDestroy()
        {
            if (IsServiceStarted)
                Input.location.Stop();
        }
    }
}
