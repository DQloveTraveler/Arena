using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public abstract class ParticleCallbackGetter : MonoBehaviour
    {
        public IDamageable Damageable { get; protected set; } = null;
        public bool IsHit { get; set; } = false;
        protected ParticleSystem _particleSystem;
        protected abstract IEnumerator OnParticleCollision(GameObject other);
    }
}
