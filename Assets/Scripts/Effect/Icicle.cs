using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Effect
{
    public class Icicle : Effect, IAttackable
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private int attackPower = 15;
        [SerializeField] private bool isKnock = true;
        [SerializeField] private Vector3 maxSize = new Vector3(3, 4, 4);
        public int AttackPower => attackPower;
        private float _distanceToGround;
        private static int layerMask = 0;

        private List<IDamageable> damagedList = new List<IDamageable>();

        protected override void Start()
        {
            if (layerMask == 0) layerMask = LayerMask.GetMask("Ground");
            transform.eulerAngles = new Vector3(0, Random.Range(0, 360), transform.eulerAngles.z);
            StartCoroutine(_DelayDestroy());
            StartCoroutine(_Fall());
            StartCoroutine(_SizeOverLifeTime());
        }

        private IEnumerator _Fall()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, 100, layerMask))
            {
                float fallSpeed = 0.00001f;
                for(int i = 0; i < 100; i++)
                {
                    transform.position = Vector3.MoveTowards(transform.position, hit.point, fallSpeed * Time.deltaTime);
                    fallSpeed *= 1.3f;

                    _distanceToGround = (hit.point - transform.position).sqrMagnitude;
                    if(_distanceToGround <= 0.1f) myCollider.enabled = false;
                    yield return null;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
            {
                if (!damagedList.Contains(damageGetter))
                {
                    if (damageGetter.TryGetDamage(AttackPower, transform.position, isKnock))
                    {
                        damagedList.Add(damageGetter);
                    }
                }
            }
        }

        private IEnumerator _SizeOverLifeTime()
        {
            float elapsedTime = 0;
            while(elapsedTime / lifeTime < 0.2f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, maxSize, 0.2f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(lifeTime * 0.6f);

            while (true)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.15f);
                yield return null;
            }
        }

    }
}
