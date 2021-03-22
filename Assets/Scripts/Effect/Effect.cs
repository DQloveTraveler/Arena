using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public abstract class Effect : MonoBehaviour
    {
        [SerializeField] protected float lifeTime = 1;

        // Start is called before the first frame update
        protected abstract void Start();

        protected IEnumerator _DelayDestroy()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }

    }
}
