
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : BaseUI
{
    protected override UIKey uiKey { get; } =  UIKey.ConditionUI;
    
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    
    private void OnDisable()
    {
        GameManager.Instance.CharacterManager.Player.health.UnSubscribeCondition(UpdateHealthUI);
        GameManager.Instance.CharacterManager.Player.stamina.UnSubscribeCondition(UpdateStaminaUI);
    }

    public override void SetUIActive(bool isActive)
    {
        base.SetUIActive(isActive);
        GameManager.Instance.CharacterManager.Player.health.SubscribeCondition(UpdateHealthUI);
        GameManager.Instance.CharacterManager.Player.stamina.SubscribeCondition(UpdateStaminaUI);
    }
    

    private void UpdateHealthUI(float currentValue, float maxValue)
    {
        healthBar.fillAmount = currentValue / maxValue;
    }

    private void UpdateStaminaUI(float currentValue, float maxValue)
    {
        staminaBar.fillAmount = currentValue / maxValue;
    }
}