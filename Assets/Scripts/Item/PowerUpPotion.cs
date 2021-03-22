using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "PowerUpPotion", menuName = "ScriptableObjects/IItem/PowerUpPotion", order = 1)]
    public class PowerUpPotion : BaseItem
    {
        [SerializeField] private float effectTime = 30;

        private Coroutine _coroutine = null;

        public override void Use(Status status)
        {
            status.IsPowerUpping = true;
            if (_coroutine != null)
            {
                CoroutineHandler.StopStaticCoroutine(_coroutine);
            }
            _coroutine = CoroutineHandler.StartStaticCoroutine(_PowerUpOFF(status));
        }

        private IEnumerator _PowerUpOFF(Status status)
        {
            yield return new WaitForSeconds(effectTime);
            status.IsPowerUpping = false;
        }
    }
}