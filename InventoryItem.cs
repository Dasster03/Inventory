using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsFull()
    {
        return quantity >= item.maxStackSize;
    }
}
