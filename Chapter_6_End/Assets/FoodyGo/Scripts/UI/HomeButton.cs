using UnityEngine;
using System.Collections;
using packt.FoodyGO.Managers;
using UnityEngine.UI;

namespace packt.FoodyGO.UI
{
    [RequireComponent(typeof(Button))]
    public class HomeButton : MonoBehaviour
    {
        void Start()
        {
            //wire itself up to Button
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }
        public void OnClick()
        {
            GameManager.Instance.OnHomeClicked();
        }
    }
}
