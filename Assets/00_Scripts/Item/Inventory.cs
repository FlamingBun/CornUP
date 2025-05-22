using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, Item> inventoryDictionary;
    private List<Item> itemList;

    private void Awake()
    {
        inventoryDictionary = new Dictionary<string, Item>();
        itemList = new List<Item>();
    }

    public void AddItem(ItemSO item)
    {
        if (inventoryDictionary.ContainsKey(item.id))
        {
            // TODO :: item 추가
        }
    }
}