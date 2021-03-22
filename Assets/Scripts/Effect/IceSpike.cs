using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class IceSpike : Effect, IAttackable
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private float colliderActivateDelay = 3.1f;
        [SerializeField] private float colliderPositionLimit = 1;
        [SerializeField] private GameObject audios;
        [SerializeField] private int attackPower;
        [SerializeField] private bool isKnock = true;

        public int AttackPower { get => attackPower; }
        private List<IDamageable> damagedList = new List<IDamageable>();

        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
            StartCoroutine(_DelayColliderActivate());
        }
        private IEnumerator _DelayColliderActivate()
        {
            yield return new WaitForSeconds(colliderActivateDelay);
            myCollider.enabled = true;
            audios.gameObject.SetActive(true);
            float delta = Mathf.Abs(myCollider.transform.localPosition.y - colliderPositionLimit); 
            for(int i = 0; i < 5; i++)
            {
                myCollider.transform.localPosition += Vector3.up * delta / 5;
                yield return null;
            }
            yield return null;

            myCollider.isTrigger = false;

            yield return new WaitForSeconds(3);
            myCollider.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
            {
                if (!damagedList.Contains(damageGetter))
                {
                    var hitPos = other.ClosestPointOnBounds(transform.position);
                    if (damageGetter.TryGetDamage(AttackPower, hitPos, isKnock))
                    {
                        damagedList.Add(damageGetter);
                    }
                }
            }
        }

    }
}
