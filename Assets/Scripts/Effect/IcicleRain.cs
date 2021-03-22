using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Effect
{
    public class IcicleRain : Effect
    {
        [SerializeField] private GameObject iciclePrefab;
        [SerializeField] private float interval = 0.2f;

        private List<Transform> _generatePosiList = new List<Transform>();
        private AudioSource _audioSource;
        protected override void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            StartCoroutine(_DelayDestroy());
            StartCoroutine(_Generate());
            StartCoroutine(_PlayAudio(1.5f, 0.1f, 22));
        }

        private IEnumerator _Generate()
        {
            var meshObjects = GetComponentsInChildren<MeshFilter>();
            foreach(var mo in meshObjects)
            {
                _generatePosiList.Add(mo.transform);
            }

            while (_generatePosiList.Count > 80)
            {
                int random = Random.Range(0, _generatePosiList.Count);
                Instantiate(iciclePrefab, _generatePosiList[random].position, _generatePosiList[random].rotation);
                _generatePosiList.RemoveAt(random);
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator _PlayAudio(float delay, float interval, int playNum)
        {
            yield return new WaitForSeconds(delay);
            for(int i = 0; i < playNum; i++)
            {
                _audioSource.PlayOneShot(_audioSource.clip);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
