using System.Collections;
using UnityEngine;

namespace packt.FoodyGo.Services
{
    [AddComponentMenu("Services/GPSLocationService")]
    public class GPSLocationService : MonoBehaviour
    {
		[Header("Exposed for Debugging Purposes Only")]
        public bool IsServiceStarted;
        public float Latitude;
        public float Longitude;
        public float Altitude;
        public float Accuracy;
        public double Timestamp;

		//initialize the object
        void Start()
        {
            print("Starting GPSLocationService");
            StartCoroutine(StartService());
        }

		//StartService is a coroutine, to avoid blocking as the location service is started
        IEnumerator StartService()
        {
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                print("location not enabled by user, existing");
                yield break;
            }

            // Start service before querying location
            Input.location.Start();

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
                print("GSPLocationService started");
                IsServiceStarted = true;
                // Access granted and location value could be retrieved
                print("Location initialized at: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            }

           
        }

		//called once per frame
        void Update()
        {
            if(Input.location.status == LocationServiceStatus.Running)
            {        
				//updates the public values that can be consumed by other game objects
                Latitude = Input.location.lastData.latitude;
                Longitude = Input.location.lastData.longitude;
                Altitude = Input.location.lastData.altitude;
                Accuracy = Input.location.lastData.horizontalAccuracy;
                Timestamp = Input.location.lastData.timestamp;
            }
        }

		//called when the object is destroyed
        void OnDestroy()
        {
            if (IsServiceStarted)
                Input.location.Stop();
        }
    }
}
