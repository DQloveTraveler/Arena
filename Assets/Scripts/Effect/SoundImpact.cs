using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class SoundImpact : Effect
    {
        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
        }
    }
}
