using System;
using UnityEngine;

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemSO : ScriptableObject
{
    [Header("Info")]
    public string id;
    public string displayName;

    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;

    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 이렇게 사용하면 체력과 배고픔 둘 다 해결할 수 있다.

    [Header("Equip")]
    public GameObject equipPrefab;
}
