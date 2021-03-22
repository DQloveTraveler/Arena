using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class PowerUpping : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Status _playerStatus;

        void Awake()
        {
            _playerStatus = transform.root.GetComponent<Status>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            _particleSystem.gameObject.SetActive(_playerStatus.IsPowerUpping);
        }
    }
}
