using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton

    public List<InventoryItem> items = new List<InventoryItem>(); // Cambiamos de List<Item> a List<InventoryItem>
    public int space = 20; // Capacidad máxima del inventario

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback; // Evento para notificar cambios

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Asignar la instancia
        }
        else
        {
            Debug.LogWarning("Más de una instancia de Inventory encontrada.");
            Destroy(gameObject);
        }
    }

    // Añadir un objeto al inventario
    // 🔹 Nuevo método: Añadir ítems con stacks
    public bool Add(InventoryItem newItem, int amount = 1)
    {
        if (newItem.item.isStackable)
        {
            // Buscar si ya existe un stack de este ítem
            foreach (InventoryItem inventoryItem in items)
            {
                if (inventoryItem.item == newItem.item && !inventoryItem.IsFull())
                {
                    int spaceLeft = newItem.item.maxStackSize - inventoryItem.quantity;
                    int amountToAdd = Mathf.Min(spaceLeft, amount);

                    inventoryItem.quantity += amountToAdd;
                    amount -= amountToAdd;

                    if (amount <= 0)
                    {
                        onItemChangedCallback?.Invoke();
                        return true;
                    }
                }
            }
        }

        // Si no es apilable o si no había stack disponible, agregar uno nuevo
        if (items.Count < space)
        {
            items.Add(new InventoryItem(newItem.item, amount));
            onItemChangedCallback?.Invoke();
            return true;
        }

        Debug.Log("No hay espacio en el inventario.");
        return false;
    }

    // 🔹 Modificar la eliminación de ítems
    public void Remove(Item itemToRemove, int amount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == itemToRemove)
            {
                if (items[i].quantity > amount)
                {
                    items[i].quantity -= amount;
                    onItemChangedCallback?.Invoke();
                    return;
                }
                else
                {
                    amount -= items[i].quantity;
                    items.RemoveAt(i);
                    i--;
                }

                if (amount <= 0)
                    break;
            }
        }
        onItemChangedCallback?.Invoke();
    }

    // Guardar el inventario
    public void SaveInventory()
    {
        ES3.Save("inventoryItems", items); // Guardar la lista de ítems
        Debug.Log("Inventario guardado.");
    }

    // Cargar el inventario
    public void LoadInventory()
    {
        if (ES3.KeyExists("inventoryItems"))
        {
            items = ES3.Load<List<InventoryItem>>("inventoryItems"); // Cargar la lista de ítems
            onItemChangedCallback?.Invoke(); // Notificar cambios en la UI
            Debug.Log("Inventario cargado.");
        }
        else
        {
            Debug.Log("No se encontró un inventario guardado.");
        }
    }

    private void Update()
    {
        // Ejemplo: Guardar el inventario al presionar una tecla
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveInventory();
        }

        // Ejemplo: Cargar el inventario al presionar una tecla
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadInventory();
        }
    }
}
