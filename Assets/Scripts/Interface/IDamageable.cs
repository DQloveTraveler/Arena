using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    Transform Transform { get; }
    bool TryGetDamage(int damage, Vector3 hitPosition, bool isKnock);
}
