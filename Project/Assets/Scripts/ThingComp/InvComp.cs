using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (InputSystem.Instance.GetKeyState(KeyCode.D) == InputSystem.KeyState.KeyDown)
        {
            foreach (RecipeData data in Database<RecipeData>.AllDatas())
            {
                bool makeable = true;

                foreach ((string, int) input in data.Inputs)
                {
                    ItemData itemData = Database<ItemData>.ConditionData(x => x.Id == input.Item1);
                    if (!Inventory.HasItem(new Item(itemData, input.Item2)))
                    {
                        makeable = false;
                        break;
                    }
                }
                if (!makeable)
                    break;

                foreach ((string, int) input in data.Inputs)
                {
                    ItemData itemData = Database<ItemData>.ConditionData(x => x.Id == input.Item1);
                    Inventory.RemoveItem(new Item(itemData, input.Item2));
                }
                foreach ((string, int) output in data.Outputs)
                {
                    ItemData itemData = Database<ItemData>.ConditionData(x => x.Id == output.Item1);
                    Inventory.AddItem(new Item(itemData, output.Item2));
                }
            }
        }
    }

    public override string SavableName => "InvComp";
}
