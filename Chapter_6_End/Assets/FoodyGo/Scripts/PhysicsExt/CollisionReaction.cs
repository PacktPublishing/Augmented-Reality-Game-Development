using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace packt.FoodyGO.PhysicsExt
{
    public class CollisionReaction : MonoBehaviour
    {
        public string collisionObjectName;

        public CollisionEvent collisionEvent;        
        public Transform particlePrefab;
        public float destroyParticleDelaySeconds;
        public bool destroyObject;
        public float destroyObjectDelaySeconds;

        public void OnCollisionReaction(GameObject go, Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (particlePrefab != null)
            {
                var particle = (Transform)Instantiate(particlePrefab, pos, rot);
                particle.parent = transform;
                Destroy(particle.gameObject, destroyParticleDelaySeconds);
            }
            if (destroyObject)
            {
                Destroy(go, destroyObjectDelaySeconds);
            }

            collisionEvent.Invoke(gameObject, collision);
        }        
    }

    [System.Serializable]
    public class CollisionEvent : UnityEvent<GameObject, Collision>
    {

    }

}
