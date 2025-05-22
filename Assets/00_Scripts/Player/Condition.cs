using System;
using UnityEngine;

public class Condition
{
        private float currentValue;
        private float maxValue;
        public float Value { get { return currentValue; } }
        private event Action<float, float> OnChangeCondition;
        
        public Condition(float _maxValue)
        {
                maxValue = _maxValue;
                currentValue = maxValue;
        }

        public void UpgradeMaxValue(float value)
        {
                maxValue += value;
                OnChangeCondition?.Invoke(currentValue, maxValue);
        }

        public void DowngradeMaxValue(float value)
        {
                maxValue -= value;
                OnChangeCondition?.Invoke(currentValue, maxValue);
        }

        public void Add(float value)
        {
                currentValue += Mathf.Min(currentValue + value, maxValue);
                OnChangeCondition?.Invoke(currentValue, maxValue);
        }

        public void Subtract(float value)
        {
                currentValue = Mathf.Max(currentValue - value, 0);
                OnChangeCondition?.Invoke(currentValue, maxValue);
        }

        public void SubscribeCondition( Action<float, float> action)
        {
                OnChangeCondition += action;
        }

        public void UnSubscribeCondition(Action<float, float> action)
        {
                OnChangeCondition -= action;
        }
}
