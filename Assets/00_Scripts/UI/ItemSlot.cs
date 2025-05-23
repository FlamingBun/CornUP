using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Item item;
    [SerializeField]private Image icon;
    [SerializeField]private TextMeshProUGUI itemNameText;
    [SerializeField]private TextMeshProUGUI quantityText;
    private Button button;
    private Outline outline;
    
    private InventoryUI inventoryUI;
    
    private bool equipped;

    private void Awake()
    {
        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        if (inventoryUI == null)
        {
            inventoryUI=GameManager.Instance.UIManager.UIDictionary[UIKey.InventoryUI] as InventoryUI;
        }

        outline.enabled = equipped;
    }

    public void Set(Item _item, bool _equipped)
    {
        item = _item;
        equipped = _equipped;
        
        
        icon.sprite = item.ItemSO.icon;
        itemNameText.text = item.ItemSO.name;
        quantityText.text = item.Count > 1 ? item.Count.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>OnClickButton(item.ItemSO));
        }
        
        this.gameObject.SetActive(true);
    }

    public void Clear()
    {
        this.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    private void OnClickButton(ItemSO itemSO)
    {
        inventoryUI.SelectItem(itemSO);
    }
}