using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using packt.FoodyGO.Controllers;

namespace packt.FoodyGO.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Splash Screen")]
        public bool ShowSplashScreen = true;
        public string SplashSceneName;

        [Header("Game Scenes")]
        public string MapSceneName;
        public string CatchSceneName;
        public string InventorySceneName;
        
        [Header("Layer Names")]
        public string MonsterLayerName = "Monster";
        
        private Scene SplashScene;
        private GameScene MapScene;        
        private GameScene InventoryScene;        
        private GameScene CatchScene;
        private GameScene lastScene;

        // Use this for initialization
        void Start()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded; 

            if(ShowSplashScreen && string.IsNullOrEmpty(SplashSceneName)==false)
            {                
                SceneManager.LoadSceneAsync(SplashSceneName, LoadSceneMode.Additive);
            }
            else if(string.IsNullOrEmpty(MapSceneName) == false)
            {
                SceneManager.LoadSceneAsync(MapSceneName, LoadSceneMode.Additive);
            }
        }

        //run when a new scene is loaded
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode lsm)
        {            
            if (scene.name == SplashSceneName)
            {
                SplashScene = scene;
                StartCoroutine(DisplaySplashScene());
            }
            else if(scene.name == MapSceneName)
            {
                MapScene = new GameScene();
                MapScene.scene = scene;                          
            }
            else if(scene.name == InventorySceneName)
            {
                InventoryScene = new GameScene();
                InventoryScene.scene = scene;
            }
            else if (scene.name == CatchSceneName)
            {
                CatchScene = new GameScene();
                CatchScene.scene = scene;                
            }
        }

        public void OnHomeClicked()
        {
            print("InventoryClicked");

            if (MapScene != null && MapScene.RootGameObject != null)
            {
                if (MapScene.RootGameObject.activeInHierarchy) lastScene = MapScene;
                MapScene.RootGameObject.SetActive(false);
            }
            if(CatchScene != null && CatchScene.RootGameObject != null)
            {
                if (CatchScene.RootGameObject.activeInHierarchy) lastScene = CatchScene;
                CatchScene.RootGameObject.SetActive(false);
            }
            
            //check if the scene has already been run
            if (InventoryScene == null)
            {
                SceneManager.LoadSceneAsync(InventorySceneName, LoadSceneMode.Additive);
            }
            else
            {
                //scene has been run before, reactivate it
                InventoryScene.RootGameObject.SetActive(true);
                var isc = InventoryScene.RootGameObject.GetComponent<InventorySceneController>();
                if (isc != null) isc.ResetScene();
            }
        }

        public void CloseMe(InventorySceneController inventorySceneController)
        {
            InventoryScene.RootGameObject.SetActive(false);
            lastScene.RootGameObject.SetActive(true);
        }

        public void CloseMe(CatchSceneController inventorySceneController)
        {
            CatchScene.RootGameObject.SetActive(false);
            MapScene.RootGameObject.SetActive(true);            
        }

        //display the Splash scene and then load the game start scene
        IEnumerator DisplaySplashScene()
        {
            SceneManager.LoadSceneAsync(MapSceneName, LoadSceneMode.Additive);   
            //set a fixed amount of time to wait before unloading splash scene   
            //we could also check if the GPS service was started and running
            //or any other requirement   
            yield return new WaitForSeconds(5);
            SceneManager.UnloadScene(SplashScene);            
        }
                
        /// <summary>
        /// Checks if a relevant game object has been hit
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool RegisterHitGameObject(PointerEventData data)
        {
            int mask = BuildLayerMask();
            Ray ray = Camera.main.ScreenPointToRay(data.position);            
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
            {
                print("Object hit " + hitInfo.collider.gameObject.name);
                var go = hitInfo.collider.gameObject;
                HandleHitGameObject(go);

                return true;
            }
            return false;
        }

        private void HandleHitGameObject(GameObject go)
        {
            if(go.GetComponent<MonsterController>()!= null)
            {
                print("Monster hit, need to open catch scene ");
                //check if the scene has already been run
                if (CatchScene == null )
                {
                    SceneManager.LoadSceneAsync(CatchSceneName, LoadSceneMode.Additive);
                }
                else
                {
                    //the scene has run before, reactivate it
                    CatchScene.RootGameObject.SetActive(true);
                    var csc = CatchScene.RootGameObject.GetComponent<CatchSceneController>();
                    if (csc != null) csc.ResetScene();
                }
                //remove the monster, he will either be caught or run away
                var mc = go.GetComponent<MonsterController>();
                mc.monsterService.RemoveMonster(mc.monsterSpawnLocation);
                MapScene.RootGameObject.SetActive(false);               
            }
        }
        
        private int BuildLayerMask()
        {
            return 1 << LayerMask.NameToLayer(MonsterLayerName);
        }
    }

    public class GameScene
    {
        public Scene scene;

        private GameObject _rootGameObject;
        public GameObject RootGameObject
        {
            get
            {
                if (scene.isLoaded == false) return null;
                if(_rootGameObject == null)
                {
                    _rootGameObject = scene.GetRootGameObjects()[0];
                }
                return _rootGameObject;
            }
        }

    }
}
