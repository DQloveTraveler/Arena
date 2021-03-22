using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class FreezeDecal : MonoBehaviour
    {
        [SerializeField] private Renderer myRenderer;
        void Start()
        {
            StartCoroutine(_RendererON(myRenderer));
        }

        private IEnumerator _RendererON(Renderer renderer)
        {
            yield return new WaitForEndOfFrame();
            renderer.enabled = true;
        }

    }
}
