using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class ChargingEffect : MonoBehaviour
    {
        [SerializeField] private Color colorOfSecondLevel;
        [SerializeField] private Color colorOfThirdLevel;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private AudioSource[] audioSource;
        private ParticleSystem.MainModule _particleMain;
        private Color _startColor;

        public int Level { get; private set; } = 0;

        void Awake()
        {
            _particleMain = particle.main;
            _startColor = _particleMain.startColor.color;
        }

        void OnEnable()
        {
            Level = 0;
        }

        public void LevelUp()
        {
            Level++;
            switch (Level)
            {
                case 1:
                    _particleMain.startColor = _startColor;
                    particle.Play();
                    audioSource[0].volume = 0.6f;
                    audioSource[0].pitch = 1f;
                    audioSource[1].Play();
                    break;
                case 2:
                    _particleMain.startColor = colorOfSecondLevel;
                    audioSource[0].volume = 0.8f;
                    audioSource[0].pitch = 1.1f;
                    break;
                case 3:
                    _particleMain.startColor = colorOfThirdLevel;
                    audioSource[0].volume = 1;
                    audioSource[0].pitch = 1.2f;
                    break;

            }
            audioSource[0].Play();
        }

    }
}
