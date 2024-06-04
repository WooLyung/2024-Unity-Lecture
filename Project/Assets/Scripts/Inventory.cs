using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public delegate void InventoryUpdateCallback(Inventory inventory);

    private List<Item> items = new();
    public event InventoryUpdateCallback OnInventoryUpdate;

    public IEnumerable<Item> AllItems()
    {
        foreach (var item in items)
            yield return item;
    }

    public void AddItem(Item item)
    {
        Item found = items.Find(item2 => item2.ItemData == item.ItemData);
        if (found != null)
            found.amount += item.amount;
        else
            items.Add(item);
        OnInventoryUpdate?.Invoke(this);
    }

    public bool HasItem(Item item)
    {
        Item found = items.Find(item2 => item2.ItemData == item.ItemData);
        if (found == null)
            return false;
        return found.amount >= item.amount;
    }

    public bool RemoveItem(Item item)
    {
        if (!HasItem(item))
            return false;

        Item found = items.Find(item2 => item2.ItemData == item.ItemData);
        found.amount -= item.amount;
        OnInventoryUpdate?.Invoke(this);
        return true;
    }
}