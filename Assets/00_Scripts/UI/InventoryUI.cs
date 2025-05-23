using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseUI
{
    protected override UIKey uiKey { get; } = UIKey.InventoryUI;
    private Inventory inventory;
    List<Item> itemList;

    [SerializeField] private Transform content;
    private List<ItemSlot> slots;
    [SerializeField]private GameObject slotPrefab;
    
    private ItemSO selectedItem;
    private ItemSO equipedItem;
    
    #region Item Info

    [Header("Item Info")]
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private TextMeshProUGUI selectedStatName;
    [SerializeField] private TextMeshProUGUI selectedStatValue;
    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    #endregion
    
    protected override void Initialize()
    {
        base.Initialize();
        inventory = GameManager.Instance.Inventory;
        itemList = inventory.InventoryList;
        slots = new List<ItemSlot>();
        CheckSlots();
    }

    private void OnDisable()
    {
        if (inventory == null) return;
        
        inventory.UnSubscribeInventory(UpdateSlot);
        inventory.UnSubscribeEquipment((x)=>equipedItem =x);
    }

    public override void SetUIActive(bool isActive)
    {
        base.SetUIActive(isActive);
        
        inventory.SubscribeInventory(UpdateSlot);
        inventory.SubscribeEquipment((x)=>equipedItem =x);
        
        UpdateSlot();
        ClearSelectedItemWindow();
    }

    private void UpdateSlot()
    {
        CheckSlots();
        
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Set(itemList[i], equipedItem!=null&&itemList[i].ItemSO.id== equipedItem.id);
        }

        if (selectedItem != null)
        {
            SetItemInfo();
        }
    }
    
    private void CheckSlots()
    {
        while(slots.Count != itemList.Count)
        {
            if (slots.Count < itemList.Count)
            {
                slots.Add(Instantiate(slotPrefab, content).GetComponent<ItemSlot>());
            }
            else
            {
                GameObject slot = slots[slots.Count-1].gameObject;
                slots.RemoveAt(slots.Count-1);
                Destroy(slot);
            }
        }
    }
    
    public void SelectItem(ItemSO item)
    {
        selectedItem = item;
        ClearSelectedItemWindow();
        SetItemInfo();
    }
    
    private void SetItemInfo()
    {
        selectedItemName.text = selectedItem.name;
        selectedItemDescription.text = selectedItem.description;
        
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.gameObject.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.gameObject.SetActive(selectedItem.type == ItemType.Equipable && selectedItem.id != equipedItem.id);
        unequipButton.gameObject.SetActive(selectedItem.type == ItemType.Equipable && selectedItem.id == equipedItem.id);
     
        useButton.onClick.RemoveAllListeners();
        equipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.RemoveAllListeners();
        
        useButton.onClick.AddListener(() =>
        {
            inventory.UseItem(selectedItem.id);
            selectedItem = null;
            ClearSelectedItemWindow();
        });
        equipButton.onClick.AddListener(()=>inventory.EquipItem(selectedItem));
        unequipButton.onClick.AddListener(()=>inventory.UnEquipItem());
    }
    
    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
    }
}