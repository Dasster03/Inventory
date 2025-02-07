using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance; // Singleton

    public Item[] equippedItems; // Array de objetos equipados
    public EquipmentSlotUI[] equipmentSlotsUI; // Slots de equipamiento en la UI

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Asignar la instancia
        }
        else
        {
            Debug.LogWarning("Más de una instancia de EquipmentManager encontrada.");
            Destroy(gameObject);
        }

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        equippedItems = new Item[numSlots];
    }

    // Equipar un objeto
    public void Equip(Item newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot;

        // Si ya hay un objeto equipado en ese slot, lo desequipamos
        if (equippedItems[slotIndex] != null)
        {
            Unequip(slotIndex);
        }

        // Equipar el nuevo objeto
        equippedItems[slotIndex] = newItem;

        // Actualizar la UI de equipamiento
        if (equipmentSlotsUI[slotIndex] != null)
        {
            equipmentSlotsUI[slotIndex].EquipItem(newItem);
        }

        Debug.Log(newItem.itemName + " equipado.");
    }

    // Desequipar un objeto
    public void Unequip(int slotIndex)
    {
        if (equippedItems[slotIndex] != null)
        {
            Item oldItem = equippedItems[slotIndex];
            Debug.Log(oldItem.itemName + " desequipado.");

            // Actualizar la UI de equipamiento
            if (equipmentSlotsUI[slotIndex] != null)
            {
                equipmentSlotsUI[slotIndex].UnequipItem();
            }

            equippedItems[slotIndex] = null;
        }
    }
}