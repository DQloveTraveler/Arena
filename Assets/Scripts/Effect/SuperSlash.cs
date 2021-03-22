using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    [RequireComponent(typeof(Collider))]
    public class SuperSlash : PhysicsAttackable, IAttackable
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float colliderSpeed = 20;
        private BoxCollider boxCollider;
        public int PowerLevel { private get; set; } = 1;

        // Start is called before the first frame update
        protected void Start()
        {
            boxCollider = (BoxCollider)myCollider;
            StartCoroutine(_DelayDestroy());
        }

        void Update()
        {
            boxCollider.center += Vector3.forward * colliderSpeed * Time.deltaTime;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
            {
                if (!damagedList.Contains(damageGetter))
                {
                    var hitPos = other.ClosestPointOnBounds(transform.position);
                    int damage = AttackPower;
                    switch (PowerLevel)
                    {
                        case 2:
                            damage = (int)(damage * 1.5f);
                            break;
                        case 3:
                            damage = (int)(damage * 2);
                            break;
                    }
                    CameraController.Instance.RadialBlurEffect();
                    damageGetter.TryGetDamage(damage, hitPos, false);
                    damagedList.Add(damageGetter);
                }
            }
        }


        private IEnumerator _DelayDestroy()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }

    }
}
