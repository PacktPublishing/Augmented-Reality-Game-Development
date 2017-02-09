using UnityEngine;
using System.Collections;
using packt.FoodyGO.Mapping;
using packt.FoodyGO.Services;
using packt.FoodyGO.Database;

namespace packt.FoodyGO.Controllers
{
    public class MonsterController : MonoBehaviour
    {
        public MapLocation location;
        public MonsterService monsterService;
        public MonsterSpawnLocation monsterSpawnLocation;
        public Animation anim;
        public float animationSpeed = 1;
        public float monsterWarmRate = .0001f;
        
        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animation>();
            anim["Walk_Loop"].speed = animationSpeed;
        }
    
        // Update is called once per frame
        void Update()
        {
            if (animationSpeed == 0)
            {
                //monster is frozen solid
                anim["Walk_Loop"].speed = 0;
                return;
            }
            //if monster is moving they will warm up a bit
            animationSpeed = Mathf.Clamp01(animationSpeed + monsterWarmRate);
            anim["Walk_Loop"].speed = animationSpeed;
        }

    }
}
