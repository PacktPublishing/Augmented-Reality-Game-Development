using UnityEngine;
using System.Collections;
using packt.FoodyGO.Services;
using UnityEngine.UI;

namespace packt.FoodyGO.UI
{
    public class FootstepTracker : MonoBehaviour
    {
        public MonsterService monsterService;
        public Texture oneFootstep;
        public Texture twoFootsteps;
        public Texture threeFootsteps;

        private RawImage image;

        // Use this for initialization
        void Start()
        {
            image = GetComponent<RawImage>();
            image.texture = null;
            image.color = new Color(0, 0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            var minFS = 4;  //set to max footsteps

            if (monsterService != null &&
                monsterService.monsters != null &&
                monsterService.monsters.Count > 0)
            {
                foreach (var m in monsterService.monsters)
                {
                    minFS = Mathf.Min(m.footstepRange, minFS);
                }
                if (minFS < 4)
                {
                    if (minFS == 1) image.texture = oneFootstep;
                    else if (minFS == 2) image.texture = twoFootsteps;
                    else if (minFS == 3) image.texture = threeFootsteps;
                    image.color = Color.white;
                }
                else
                {
                    image.texture = null;
                    image.color = new Color(0, 0, 0, 0);
                }
            }
        }
    }
}
