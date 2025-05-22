using System;
using UnityEngine;

public class Player : MonoBehaviour
{
        [HideInInspector]public PlayerController controller;

        [HideInInspector] public Condition health;
        [HideInInspector] public Condition stamina;
        //public Equipment equipment;

        [SerializeField] private float passiveRecovery = 10f;
        
        [SerializeField]private float addStaminaTimer = 5f;
        private float currentAddStaminaTime = 0f;

        public event Action onDamage;
        
        private void Awake()
        {
                controller = GetComponent<PlayerController>();
                health = new Condition(100f);
                stamina = new Condition(100f);
                currentAddStaminaTime = addStaminaTimer;
        }

        private void Update()
        {
                if (currentAddStaminaTime > 0f)
                {
                        currentAddStaminaTime -= Time.deltaTime;
                        if (currentAddStaminaTime <= 0)
                        {
                                stamina.Add(passiveRecovery);
                                currentAddStaminaTime = addStaminaTimer;
                        }
                }
        }

        public void TakeDamage(float damage)
        {
                health.Subtract(damage);
                onDamage?.Invoke();
                if (health.Value <= 0)
                {
                        GameManager.Instance.GameOver();
                }
        }
}
