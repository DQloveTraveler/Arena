using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class HandGrenade : Effect
    {
        [SerializeField] private GameObject explosionEffect;
        private ParticleSystem _particleSystem;

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
        }

        void OnParticleCollision(GameObject other)
        {
            var events = new List<ParticleCollisionEvent>();
            _particleSystem.GetCollisionEvents(other, events);
            var hitPos = events[0].intersection;
            Instantiate(explosionEffect, hitPos, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
