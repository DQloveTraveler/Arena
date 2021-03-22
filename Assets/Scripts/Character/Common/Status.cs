using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StatusManagement
{
    public class Status : MonoBehaviour
    {
        [SerializeField] private StatusData statusData;

        public HitPoint HP { get; private set; }
        public StaminaPoint SP { get; private set; }
        public EnergyPoint EP { get; private set; }
        public bool IsPowerUpping { get; set; } = false;
        public bool IsDefenseUpping { get; set; } = false;

        void Awake()
        {
            HP = new HitPoint(statusData);
            SP = new StaminaPoint(statusData);
            EP = new EnergyPoint(statusData);
        }

    }

    public class HitPoint
    {
        private StatusData statusData;
        public int Max { get => statusData.MaxHP; }
        public int Value { get; private set; }
        public float Ratio { get => (float)Value / Max; }

        public HitPoint(StatusData statusData)
        {
            this.statusData = statusData;
            Value = Max;
        }

        public void Damage(int damage)
        {
            Value -= damage;
        }

        public void Heal(int healAmount)
        {
            Value += healAmount;
            Value = Mathf.Clamp(Value, 0, Max);
        }
    }

    public class StaminaPoint
    {
        private StatusData statusData;
        public int Max { get => statusData.MaxSP; }
        public float Value { get; private set; }
        public bool IsKeepMax { get; set; } = false;

        public StaminaPoint(StatusData statusData)
        {
            this.statusData = statusData;
            Value = Max;
        }

        public void Use(float amount)
        {
            if (!IsKeepMax) Value -= amount;
            Value = Mathf.Clamp(Value, 0, Max);
        }

        public void Heal(float amount)
        {
            Value += amount;
            Value = Mathf.Clamp(Value, 0, Max);
        }

        public void Reset()
        {
            Value = Max;
        }
    }

    public class EnergyPoint
    {
        private StatusData statusData;
        public int Max { get => statusData.MaxEP; }
        public int Value { get; private set; }

        public EnergyPoint(StatusData statusData)
        {
            this.statusData = statusData;
            Value = Max /2;
        }

        public void Use(int amount)
        {
            Value -= amount;
            if (Value < 0) Value = 0;
        }

        public void Heal(int amount)
        {
            Value += amount;
            if (Value > Max) Value = Max;
        }
    }
}

