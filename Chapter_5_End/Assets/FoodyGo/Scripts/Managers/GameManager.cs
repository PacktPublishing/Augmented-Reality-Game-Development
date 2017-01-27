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
        
        [Header("Layer Names")]
        public string MonsterLayerName = "Monster";

        private Scene SplashScene;
        private Scene MapScene;
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
                MapScene = scene;                
            }
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

        // Update is called once per frame
        void Update()
        {

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
            }
        }

        private int BuildLayerMask()
        {
            return 1 << LayerMask.NameToLayer(MonsterLayerName);
        }
    }
}
