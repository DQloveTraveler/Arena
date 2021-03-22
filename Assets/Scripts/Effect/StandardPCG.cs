using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Effect
{
    public class StandardPCG : ParticleCallbackGetter
    {
        [SerializeField] private UnityEvent onHit = new UnityEvent();
        private List<ParticleCollisionEvent> collisionEventList = new List<ParticleCollisionEvent>();
        public List<Vector3> HitPosList { get; private set; } = new List<Vector3>();

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        protected override IEnumerator OnParticleCollision(GameObject other)
        {
            _particleSystem.GetCollisionEvents(other, collisionEventList);
            foreach (var ce in collisionEventList)
            {
                HitPosList.Add(ce.intersection);
            }
            yield return null;
            onHit.Invoke();
        }
    }
}
