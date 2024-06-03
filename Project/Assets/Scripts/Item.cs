using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData ItemData { get; private set; }
    public int amount;

    public Item(ItemData itemData, int amount)
    {
        ItemData = itemData;
        this.amount = amount;
    }
}