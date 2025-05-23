using System;
using UnityEngine;

public class Item
{
    private ItemSO itemSO;
    public ItemSO ItemSO { get { return itemSO; } }
    private int count;
    public int Count { get { return count; } }

    public Item(ItemSO _itemSO, int _count=1)
    {
        itemSO = _itemSO;
        count = _count;
    }

    public int Add()
    {
        return ++count;
    }

    public int Use()
    {
        return --count;
    }
}
