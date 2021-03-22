using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Effect
{
    public class IceSpikeRandom : Effect
    {
        [SerializeField] private GameObject iceSpikePrefab;
        [SerializeField] private float generateInterval = 0.3f;
        [SerializeField] private Transform[] generatePointGroups;

        private readonly int _generateCountPerGroup = 1;
        private int _layerMask;

        protected override void Start()
        {
            _layerMask = LayerMask.GetMask("Ground");
            StartCoroutine(_DelayDestroy());
            StartCoroutine(_GenerateEffects());
        }


        private IEnumerator _GenerateEffects()
        {
            for(int i = 0; i < generatePointGroups.Length; i++)
            {
                int random = Random.Range(0, generatePointGroups[i].childCount);
                var selectedPoint = generatePointGroups[i].GetChild(random);
                Instantiate(iceSpikePrefab, selectedPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(generateInterval);
            }
        }



        //処理が重かったため使わない
        private List<Transform> SelectPoint(Transform pointGroup)
        {
            var indexList = new List<int>();
            for (int j = 0; j < pointGroup.childCount; j++)
            {
                indexList.Add(j);
            }

            var transformList = new List<Transform>();
            for (int k = 0; k < _generateCountPerGroup; k++)
            {
                int index = Random.Range(0, indexList.Count);
                int randomNum = indexList[index];
                indexList.RemoveAt(index);

                transformList.Add(pointGroup.GetChild(randomNum));
            }
            return transformList;
        }

        private bool RayCheck(Transform origin, out RaycastHit hit)
        {
            Ray ray = new Ray(origin.position, Vector3.down);
            return Physics.Raycast(ray, out hit, 5, _layerMask);
        }

    }
}
