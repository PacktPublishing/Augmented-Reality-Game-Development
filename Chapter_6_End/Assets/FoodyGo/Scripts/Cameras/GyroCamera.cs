using UnityEngine;
using System.Collections;

namespace packt.FoodyGO.Cameras
{
    public class GyroCamera : MonoBehaviour
    {
        public float heightSensitivity = 2.5f;
        public float distanceSensitivity = 2.5f;
        private bool gyroBool;
        private Gyroscope gyro;

        private Transform mTrans;
        private Vector2 dist = Vector2.zero;
        private float camStartY;
        private float camStartZ;
        private float distY;
        private float distZ;

        // the camera will continue to look at this as it moves, keep it in perspective mode
        public GameObject cameraTarget;

        void Start()
        {
            gyroBool = SystemInfo.supportsGyroscope;

            if (gyroBool)
            {

                mTrans = transform;

                gyro = Input.gyro;
                gyro.enabled = true;
                camStartY = transform.position.y;
                camStartZ = transform.position.z;
                distY = transform.position.y;
                distZ = transform.position.z;
            }
            else
            {
                print("NO GYRO");
            }
        }

        void Update()
        {
            if (gyroBool)
            {
                float delta = Time.deltaTime;

                //we only want to adjust the camera range
                //based on the vertical orientation of the device
                print(Input.gyro.attitude.eulerAngles.y);

                var angle = Input.gyro.attitude.eulerAngles.y * Mathf.Deg2Rad;

                distZ = -10 + Mathf.Abs(Mathf.Cos(angle) * distanceSensitivity);
                distY = 1 + Mathf.Abs(Mathf.Cos(angle) * heightSensitivity);

                print(angle);

                dist = Vector2.Lerp(dist, new Vector2(distY, distZ), delta * 5f);

                transform.localPosition = new Vector3(0, dist.x, dist.y);
                transform.LookAt(cameraTarget.transform.position);
            }
        }
    }
}