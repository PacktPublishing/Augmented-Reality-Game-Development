using UnityEngine;
using System.Collections;
using packt.FoodyGO.UI;
using packt.FoodyGO.Services;
using packt.FoodyGO.Managers;
using System;
using packt.FoodyGO.Database;

namespace packt.FoodyGO.Controllers
{
    public class InventorySceneController : MonoBehaviour
    {
        public InventoryContent inventoryContent;

        void Start()
        {
            var monsters = InventoryService.Instance.ReadMonsters();
            if(inventoryContent != null)
            {
                inventoryContent.FillInventory(monsters);
            }
        }

        public void OnCloseInventory()
        {
            GameManager.Instance.CloseMe(this);
        }

        public void OpenDetailScene(Monster m)
        {
            print("Detail scene " + m);
        }

        public void ResetScene()
        {
            if (inventoryContent != null)
            {
                inventoryContent.ClearInventory();
            }
            Start();
        }
    }
}
