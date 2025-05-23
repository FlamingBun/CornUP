using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, Item> inventoryDictionary;
    private List<Item> inventoryList;
    public List<Item> InventoryList { get { return inventoryList; } }
    private ItemSO equippedItem;
    
    private event Action OnChangeInventory;
    private event Action<ItemSO> OnChangeEquipment;
    
    private void Awake()
    {
        inventoryDictionary = new Dictionary<string, Item>();
        inventoryList = new List<Item>();
    }

    public void AddItem(ItemSO item)
    {
        if (inventoryDictionary.ContainsKey(item.id))
        {
            inventoryDictionary[item.id].Add();
        }
        else
        {
            Item newItem = new Item(item);
            inventoryDictionary.Add(item.id, newItem);
            inventoryList.Add(newItem);
        }

        OnChangeInventory?.Invoke();
    }

    public void UseItem(string id)
    {
        ItemSO usedItem = inventoryDictionary[id].ItemSO;
        if (usedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < usedItem.consumables.Length; i++)
            {
                switch (usedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        GameManager.Instance.CharacterManager.Player.health.Add(usedItem.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        GameManager.Instance.CharacterManager.Player.stamina.Add(usedItem.consumables[i].value);
                        break;
                }
            }
        }

        if (inventoryDictionary[id].Use() <= 0)
        {
            Item item = inventoryDictionary[id];
            inventoryDictionary.Remove(id);
            inventoryList.Remove(item);
        }
        OnChangeInventory?.Invoke();
    }

    public void EquipItem(ItemSO item)
    {
        equippedItem = item;
        OnChangeInventory?.Invoke();
        OnChangeEquipment?.Invoke(equippedItem);
    }

    public void UnEquipItem()
    {
        equippedItem = null;
        OnChangeInventory?.Invoke();
        OnChangeEquipment?.Invoke(equippedItem);
    }

    public void SubscribeInventory(Action action)
    {
        OnChangeInventory += action;
    }

    public void UnSubscribeInventory(Action action)
    {
        OnChangeInventory -= action;
    }
    
    public void SubscribeEquipment(Action<ItemSO> action)
    {
        OnChangeEquipment += action;
    }
    
    public void UnSubscribeEquipment(Action<ItemSO> action)
    {
        OnChangeEquipment -= action;
    }
}