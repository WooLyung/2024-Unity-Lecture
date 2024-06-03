using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvComp : ThingComp
{
    public Inventory Inventory { get; private set; } = new();
    private ItemData itemA, itemB;

    public InvComp(Thing thing) : base(thing)
    {
    }

    public override void OnStart()
    {
        base.OnStart();
        itemA = Database<ItemData>.ConditionData(data => data.Id == "item_a");
        itemB = Database<ItemData>.ConditionData(data => data.Id == "item_b");
    }

    public override void Tick()
    {
        base.Tick();
        if (InputSystem.Instance.GetKeyState(KeyCode.A) == InputSystem.KeyState.KeyDown)
            Inventory.AddItem(new Item(itemA, 10));
        if (InputSystem.Instance.GetKeyState(KeyCode.S) == InputSystem.KeyState.KeyDown)
            Inventory.AddItem(new Item(itemB, 10));
    }
}
