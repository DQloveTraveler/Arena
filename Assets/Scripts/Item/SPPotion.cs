using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "SPPotion", menuName = "ScriptableObjects/IItem/SPPotion", order = 1)]
    public class SPPotion : BaseItem
    {
        [SerializeField] private float effectTime = 20;

        private Coroutine _coroutine = null;

        public override void Use(Status status)
        {
            status.SP.IsKeepMax = true;
            status.SP.Reset();

            if (_coroutine != null)
                CoroutineHandler.StopStaticCoroutine(_coroutine);
            else
                _coroutine = CoroutineHandler.StartStaticCoroutine(_SPKeepMaxSwitch(effectTime, status));
        }

        private IEnumerator _SPKeepMaxSwitch(float waitTime, Status status)
        {
            yield return new WaitForSeconds(waitTime);
            status.SP.IsKeepMax = false;
        }
    }
}


