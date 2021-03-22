using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PhysicsAttackable : MonoBehaviour, IAttackable
{
    [SerializeField] private int attackPower;
    [SerializeField] protected Collider myCollider;
    public int AttackPower { get => attackPower; }
    protected List<IDamageable> damagedList = new List<IDamageable>();


    protected abstract void OnTriggerEnter(Collider other);

    public void ResetAttackedList()
    {
        myCollider.enabled = true;
        damagedList.Clear();
    }

    public void ColliderOFF()
    {
        myCollider.enabled = false;
    }
}
