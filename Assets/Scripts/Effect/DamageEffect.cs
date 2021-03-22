using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class DamageEffect : Effect
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
        }
    }
}
