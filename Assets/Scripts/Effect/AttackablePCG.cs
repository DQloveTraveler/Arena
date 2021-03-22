using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class AttackablePCG : ParticleCallbackGetter
    {
        protected override IEnumerator OnParticleCollision(GameObject other)
        {
            if (!IsHit)
            {
                if (other.TryGetComponent<IDamageable>(out var damageable))
                {
                    IsHit = true;
                    Damageable = damageable;
                }
            }
            yield return null;
        }
    }
}
