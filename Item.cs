using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item"; // Nombre del objeto
    public string description = "";
    public Sprite icon = null; // Icono del objeto
    public bool isEquippable = false; // ¿Se puede equipar?
    public EquipmentSlot equipmentSlot; // Slot de equipo (si es equipable)
    public ItemType itemType;
    public GameObject equipmentPrefab; // Prefab del objeto equipable (opcional)

    // 🔹 Nueva Propiedad: Soporte para stacks
    public bool isStackable = false;
    public int maxStackSize = 1; // Máximo de ítems en un stack

    // Método para usar el objeto
    public virtual void Use()
    {
        Debug.Log("Usando " + itemName);
    }
}
