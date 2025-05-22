using System;
using UnityEngine;

public class Player : MonoBehaviour
{
        [HideInInspector]public PlayerController controller;

        [HideInInspector] public Condition health;
        //public Equipment equipment;

        private void Start()
        {
                GameManager.Instance.CharacterManager.Player = this;
                controller = GetComponent<PlayerController>();
                health = new Condition(100f);
        }
}
