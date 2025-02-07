using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inventory inventory;
    public EquipmentManager equipmentManager;
      public float interactionRange = 3f; // Rango de interacción
    public KeyCode interactKey = KeyCode.E; // Tecla para interactuar
    public KeyCode closeKey = KeyCode.Escape; // Tecla para cerrar la UI del cofre

    private Chest nearbyChest; // Referencia al cofre cercano


    private void Update()
    {
        // Ejemplo: Equipar un objeto al presionar la tecla 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.items.Count > 0)
            {
                // Equipar el primer objeto del inventario
             //   equipmentManager.Equip(inventory.items[0]);
            }
        }

        // Ejemplo: Equipar un objeto específico (por ejemplo, un arma)
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     foreach (Item item in inventory.items)
        //     {
        //         if (item.equipmentSlot == EquipmentSlot.Weapon)
        //         {
        //             equipmentManager.Equip(item);
        //             break;
        //         }
        //     }
        // }

   if (Input.GetKeyDown(interactKey) && nearbyChest != null)
        {
            if (!ChestUI.instance.chestPanel.activeSelf) // Si la UI del cofre no está activa
            {
                nearbyChest.OpenChest(); // Abrir el cofre
            }
        }

        if (Input.GetKeyDown(closeKey) && ChestUI.instance.chestPanel.activeSelf) // Si la UI del cofre está activa
        {
            ChestUI.instance.HideChest(); // Cerrar la UI del cofre
        }

        DetectNearbyChest();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto tiene el script ItemPickup
        // ItemPickup itemPickup = other.GetComponent<ItemPickup>();
        // Debug.Log("aaaaa: " + itemPickup.name);
        // if (itemPickup != null)
        // {
        //     // Recoger el objeto
        //     Item newItem = itemPickup.Pickup();
        //     if (newItem != null)
        //     {
        //         inventory.Add(newItem);
        //     }
        // }
    }

 private void DetectNearbyChest()
    {
        // Lanzar un rayo para detectar cofres cerca del jugador
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
        {
            Chest chest = hit.collider.GetComponent<Chest>();
            if (chest != null)
            {
                if (nearbyChest != chest)
                {
                    if (nearbyChest != null)
                    {
                        nearbyChest.HideInteractionIcon(); // Ocultar el icono del cofre anterior
                        if (ChestUI.instance.chestPanel.activeSelf) // Si la UI del cofre está abierta
                        {
                            ChestUI.instance.HideChest(); // Cerrar la UI del cofre
                        }
                    }
                    nearbyChest = chest;
                    nearbyChest.ShowInteractionIcon(); // Mostrar el icono del nuevo cofre
                }
            }
            else
            {
                if (nearbyChest != null)
                {
                    nearbyChest.HideInteractionIcon(); // Ocultar el icono si no hay cofre cerca
                    if (ChestUI.instance.chestPanel.activeSelf) // Si la UI del cofre está abierta
                    {
                        ChestUI.instance.HideChest(); // Cerrar la UI del cofre
                    }
                    nearbyChest = null;
                }
            }
        }
        else
        {
            if (nearbyChest != null)
            {
                nearbyChest.HideInteractionIcon(); // Ocultar el icono si no hay cofre cerca
                if (ChestUI.instance.chestPanel.activeSelf) // Si la UI del cofre está abierta
                {
                    ChestUI.instance.HideChest(); // Cerrar la UI del cofre
                }
                nearbyChest = null;
            }
        }
    }
}