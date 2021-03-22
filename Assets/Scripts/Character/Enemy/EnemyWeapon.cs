using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : PhysicsAttackable
{
    [SerializeField] private bool isKnock = true;
    [SerializeField] private bool isHitBack = false;
    private Vector3 hitPos;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
        {
            if (!damagedList.Contains(damageGetter))
            {
                hitPos = isHitBack ? 
                    transform.root.forward * -100 : 
                    other.ClosestPointOnBounds(transform.position);

                if(damageGetter.TryGetDamage(AttackPower, hitPos, isKnock))
                {
                    damagedList.Add(damageGetter);
                }
            }
        }
    }
}
