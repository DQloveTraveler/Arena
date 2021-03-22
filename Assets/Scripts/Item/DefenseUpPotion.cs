using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "DefenseUpPotion", menuName = "ScriptableObjects/IItem/DefenseUpPotion", order = 1)]
    public class DefenseUpPotion : BaseItem
    {
        [SerializeField] private float effectTime = 30;

        private Coroutine _coroutine = null;

        public override void Use(Status status)
        {
            status.IsDefenseUpping = true;
            if (_coroutine != null)
            {
                CoroutineHandler.StopStaticCoroutine(_coroutine);
            }
            _coroutine = CoroutineHandler.StartStaticCoroutine(_DefenseUpOFF(status));
        }

        private IEnumerator _DefenseUpOFF(Status status)
        {
            yield return new WaitForSeconds(effectTime);
            status.IsDefenseUpping = false;
        }
    }
}
