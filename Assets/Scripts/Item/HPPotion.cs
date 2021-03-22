using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "HPPotion", menuName = "ScriptableObjects/IItem/HPPotion", order = 1)]
    public class HPPotion : BaseItem
    {
        [SerializeField] private int healAmount = 40;

        public override void Use(Status status)
        {
            status.HP.Heal(healAmount);
        }
    }

}
