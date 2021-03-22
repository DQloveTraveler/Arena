using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class GroundImpactSmall : Effect, IAttackable
    {
        [SerializeField] private float maxRadiasSize = 3;
        [SerializeField] private Transform impactPoint;
        [SerializeField] private int attackPower = 20;
        public int AttackPower { get => attackPower; }
        // Start is called before the first frame update
        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
            StartCoroutine(ColliderExplosion());
        }

        private IEnumerator ColliderExplosion()
        {
            int loop = 5;
            RaycastHit[] hits;
            int layerMask = LayerMask.GetMask("Player");
            for (int i = 0; i < loop; i++)
            {
                hits = Physics.SphereCastAll(impactPoint.position, maxRadiasSize, Vector3.up, 0.01f, layerMask);
                if (hits.Length > 0)
                {
                    if (hits[0].collider.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
                    {
                        damageGetter.TryGetDamage(AttackPower, impactPoint.position, true);
                        yield break;
                    }
                }
                yield return null;
            }
        }

    }
}
