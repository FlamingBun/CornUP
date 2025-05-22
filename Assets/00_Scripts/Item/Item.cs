using System;
using UnityEngine;

public class Item
{
    private ItemSO itemSO;
    private int count;
    public int Count { get { return count; } }

    private event Action OnCount;

    public Item(ItemSO _itemSO, int _count=1)
    {
        itemSO = _itemSO;
        count = _count;
    }
}
