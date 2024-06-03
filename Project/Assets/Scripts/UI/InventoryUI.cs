using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Text text;
    private bool isOpened = false;
    private Inventory inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOpened)
            {
                isOpened = false;
                inventory.OnInventoryUpdate -= OnInventoryUpdate;
                inventory = null;
                text.text = "";
            }
            else
            {
                isOpened = true;
                Thing thing = ThingSystem.Instance.FindThingsWithComp(typeof(InvComp))[0];
                InvComp invComp = (InvComp)thing.GetComp(typeof(InvComp));
                inventory = invComp.Inventory;
                inventory.OnInventoryUpdate += OnInventoryUpdate;
                OnInventoryUpdate(inventory);
            }
        }
    }

    private void OnInventoryUpdate(Inventory inventory)
    {
        text.text = "";
        foreach (Item item in inventory.AllItems())
            text.text += item.ItemData.Name + " " + item.amount + "°³\n";
    }
}