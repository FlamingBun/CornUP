
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO data;


    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        GameManager.Instance.Inventory.AddItem(data);
        Destroy(gameObject);
    }
}
