using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : PhysicsAttackable
{
    private Status playerStatus;

    void Awake()
    {
        playerStatus = transform.root.GetComponent<Status>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.TryGetComponent<IDamageable>(out var damageGetter))
        {
            if (!damagedList.Contains(damageGetter))
            {
                var hitPos = other.ClosestPointOnBounds(transform.position);
                var attackPower = AttackPower;
                if (playerStatus.IsPowerUpping) attackPower *= 2;

                if(damageGetter.TryGetDamage(attackPower, hitPos, false))
                {
                    playerStatus.EP.Heal(AttackPower);
                    damagedList.Add(damageGetter);
                }
            }
        }
    }

}
