using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Effect
{
    public class NormalSlash : Effect
    {
        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
        }
    }
}
