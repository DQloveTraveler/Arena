using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class GrenadeExplosion : Effect, IAttackable
    {
        [SerializeField] private int attackPower;
        [SerializeField] private float explosionRadias = 2;

        public int AttackPower => attackPower;

        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
            Explosion();
        }

        private void Explosion()
        {
            var layerMask = LayerMask.GetMask("Player", "Enemy");
            var hits = Physics.SphereCastAll(transform.position, explosionRadias, Vector3.up, 0.01f, layerMask);
            if (hits.Length > 0)
            {
                if (hits[0].collider.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
                {
                    damageGetter.TryGetDamage(AttackPower, transform.position, true);
                }
            }
        }

    }
}
