using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class DustBreath : Effect, IAttackable
    {
        [SerializeField] private int attackPower;
        public int AttackPower => attackPower;

        private ParticleCallbackGetter _callbackGetter;

        protected override void Start()
        {
            _callbackGetter = GetComponentInChildren<ParticleCallbackGetter>();
            StartCoroutine(_DelayDestroy());
            StartCoroutine(_WaitCallBack());
        }

        private IEnumerator _WaitCallBack()
        {
            while (true)
            {
                if (_callbackGetter.IsHit)
                {
                    if (_callbackGetter.Damageable.TryGetDamage(AttackPower, -transform.forward * 500, true))
                    {
                        yield break;
                    }
                    else
                    {
                        _callbackGetter.IsHit = false;
                    }
                }
                yield return null;
            }
        }
    }
}
