using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Effect
{
    public class DecalEffect : Effect
    {
        [SerializeField] private Gradient color = new Gradient();
        private Renderer render;
        private Color baseColor;
        private float time = 0;


        private void Awake()
        {
            render = GetComponent<Renderer>();
            baseColor = render.material.GetColor("_TintColor");
        }

        protected override void Start()
        {
            StartCoroutine(_DelayDestroy());
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            var multiplyColor = color.Evaluate(time / lifeTime);
            render.material.SetColor("_TintColor", baseColor * multiplyColor);
        }
    }
}
