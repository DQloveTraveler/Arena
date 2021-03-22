using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class BlizzardBreath : Effect, IAttackable
    {
        [SerializeField] private int attackPower;
        [SerializeField] private GameObject freezeEffect;
        [SerializeField] private AttackablePCG _attackablePCG;
        [SerializeField] private StandardPCG _standardPCG;
        public int AttackPower => attackPower;


        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
            StartCoroutine(_WaitCallBack());
        }

        private IEnumerator _WaitCallBack()
        {
            while (true)
            {
                if (_attackablePCG.IsHit)
                {
                    if (_attackablePCG.Damageable.TryGetDamage(AttackPower, -transform.forward * 500, true))
                    {
                        yield break;
                    }
                    else
                    {
                        _attackablePCG.IsHit = false;
                    }
                }
                yield return null;
            }
        }

        public void GenerateFreezeDecal()
        {
            if(_standardPCG.HitPosList.Count > 0)
            {
                Instantiate(freezeEffect, _standardPCG.HitPosList[0], Quaternion.identity);
                _standardPCG.HitPosList.RemoveAt(0);
            }
        }

    }
}
