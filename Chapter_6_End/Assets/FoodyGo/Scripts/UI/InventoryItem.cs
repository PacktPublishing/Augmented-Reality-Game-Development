using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using packt.FoodyGO.Database;
using System;
using packt.FoodyGO.Controllers;

namespace packt.FoodyGO.UI
{
    public class InventoryItem : MonoBehaviour
    {
        protected Text TopText;
        protected Text BottomText;
        protected RawImage Image;
        
        public void Start()
        {
            TopText = transform.Find("TopText").gameObject.GetComponent<Text>();
            BottomText = transform.Find("BottomText").gameObject.GetComponent<Text>();
            Image = transform.Find("RawImage").gameObject.GetComponent<RawImage>();            
        }
        
        public virtual void SetContent(object value)
        {
            
        }

    }
}
