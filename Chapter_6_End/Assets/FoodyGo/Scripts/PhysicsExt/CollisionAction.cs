using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace packt.FoodyGO.PhysicsExt
{
    public class CollisionAction : MonoBehaviour
    {        
        private CollisionReaction[] reactions;
               
        public bool disarmed = true;

        void OnCollisionEnter(Collision collision)
        {
            if (disarmed == false)
            {
                reactions = collision.gameObject.GetComponents<CollisionReaction>(); 
                if(reactions != null && reactions.Length>0)
                {
                    foreach (var reaction in reactions)
                    {
                        if (gameObject.name.StartsWith(reaction.collisionObjectName))
                        {
                            reaction.OnCollisionReaction(gameObject, collision);
                        }
                    }
                }          
            }
        }        
    }
}
