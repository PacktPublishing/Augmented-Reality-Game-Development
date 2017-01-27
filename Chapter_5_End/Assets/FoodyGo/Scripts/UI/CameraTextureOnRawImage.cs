using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace packt.FoodyGO.UI
{
    [RequireComponent(typeof(RawImage))]
    public class CameraTextureOnRawImage : MonoBehaviour
    {        
        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }

        public RawImage rawImage;
        public WebCamTexture webcamTexture;       
        public AspectRatioFitter aspectFitter;
        void Awake()
        {
            webcamTexture = new WebCamTexture(Screen.width, Screen.height);
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();

            rawImage.texture = webcamTexture;
            rawImage.material.mainTexture = webcamTexture;            
            webcamTexture.Play();
        }

        void Update()
        {
            if (webcamTexture.isPlaying == false) return;

            var camRotation = -webcamTexture.videoRotationAngle;
            if (webcamTexture.videoVerticallyMirrored)
            {
                camRotation += 180;
            }

            rawImage.transform.localEulerAngles = new Vector3(0f, 0f, camRotation);

            var videoRatio = (float)webcamTexture.width / (float)webcamTexture.height;
            aspectFitter.aspectRatio = videoRatio;

            if (webcamTexture.videoVerticallyMirrored)
            {
                rawImage.uvRect = new Rect(1, 0, -1, 1);
            }
            else
            {
                rawImage.uvRect = new Rect(0, 0, 1, 1);
            }
        }
    }
}
