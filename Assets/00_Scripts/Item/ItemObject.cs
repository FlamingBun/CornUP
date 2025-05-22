
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO data;


    public string GetInteractPrompt()
    {
        string str = $"[E] {data.displayName} 줍기";
        return str;
    }

    public void OnInteract()
    {
        GameManager.Instance.Inventory.AddItem(data);
        Destroy(gameObject);
    }
}
