using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public abstract class BaseItem : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemImage;
        [SerializeField] private int maxNum = 3;
        [SerializeField] private GameObject usingEffect;

        public string Name => itemName;
        public Sprite Sprite => itemImage;
        public int MaxNum => maxNum;
        public GameObject UsingEffect => usingEffect;

        public abstract void Use(Status status);

        public GameObject GetEffect()
        {
            return usingEffect;
        }
    }
}


