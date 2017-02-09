using packt.FoodyGO.Managers;
using packt.FoodyGO.PhysicsExt;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace packt.FoodyGO.TouchInput
{
    [RequireComponent(typeof(Image))]
    public class ThrowTouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float throwSpeed = 35f;       
        public GameObject throwObject;
        private GameObject target;

        private float speed;
        private Vector2 lastPos;
        private Vector3 screenPosition;
        private Vector3 offset;

        private bool thrown;

        private Rigidbody rb;       
        private Vector3 startPosition;
        private Quaternion startRotation;

        bool m_Dragging;
        int m_Id = -1;
       

#if !UNITY_EDITOR
    private Vector3 m_Center;
    private Image m_Image;
#else
        Vector3 m_PreviousMouse;
        
#endif

        /// <summary>
        /// changed from Start to Enable, in order to reset when 
        /// game object is activated/deactivated
        /// </summary>
        void OnEnable()
        {
            if (throwObject != null)
            {
                startPosition = throwObject.transform.position; 
                startRotation = throwObject.transform.rotation;
                throwObject.SetActive(false);
                ResetTarget();
            }

#if !UNITY_EDITOR
            m_Image = GetComponent<Image>();
            m_Center = m_Image.transform.position;            
#endif
        }
               
        void OnDragging(Vector3 touchPos)
        {
            //track mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            target.transform.position = currentPosition;
        }

        //starting point to dragging an object
        public void OnPointerDown(PointerEventData data)
        {
            Ray ray = Camera.main.ScreenPointToRay(data.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                //check if target object was hit
                if (hit.transform == target.transform)
                {
                    //yes, start dragging the object
                    m_Dragging = true;
                    m_Id = data.pointerId;

                    screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                    offset = target.transform.position -
                        Camera.main.ScreenToWorldPoint(new Vector3(data.position.x, data.position.y, screenPosition.z));
                }
            }
        }

        void Update()
        {         
            if (!m_Dragging)
            {
                return;
            }           

#if !UNITY_EDITOR            
            lastPos = new Vector2(Input.touches[m_Id].position.x , Input.touches[m_Id].position.y );        
#else            
            lastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif
            OnDragging(lastPos);
        }


        public void OnPointerUp(PointerEventData data)
        {
            if (m_Dragging == false) return;

            ThrowObject (data.position);
            m_Dragging = false;
            m_Id = -1;            
        }

       
        void ThrowObject(Vector2 pos)
        {
            rb.useGravity = true;

            float y = (pos.y - lastPos.y) / Screen.height * 100;
            speed = throwSpeed * y;

            float x = (pos.x / Screen.width) - (lastPos.x / Screen.width);
            x = Mathf.Abs(pos.x - lastPos.x) / Screen.width * 100 * x;

            Vector3 direction = new Vector3(x, 0f, 1f);
            direction = Camera.main.transform.TransformDirection(direction);

            rb.AddForce((direction * speed * 2f ) + (Vector3.up * speed/2f));

            thrown = true;

            var ca = target.GetComponent<CollisionAction>();
            if(ca != null)
            {
                ca.disarmed = false;
            }

            Invoke("ResetTarget", 5);            
        }
               

        void ResetTarget()
        {
            if (isActiveAndEnabled)
            {
                thrown = false;
                Destroy(target);
                var pos = startPosition;// Camera.main.ScreenToWorldPoint(startPosition);
                var rot = startRotation;
                target = (GameObject)Instantiate(throwObject, pos, rot);
                target.transform.parent = transform.parent;
                rb = target.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                target.SetActive(true);
            }
        }
    }
}

