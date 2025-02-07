using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public static ChestUI instance; // Singleton

    public GameObject chestPanel; // Panel que contiene la UI del cofre
    public Transform itemsParent; // Contenedor de los slots de ítems del cofre
    public Button closeButton; // Botón para cerrar la UI del cofre
    public Chest currentChest; // Referencia al cofre actual
    public float animationDuration = 0.5f; // Duración de la animación
    public Ease easeType = Ease.OutBack; // Tipo de easing para la animación

    public InventorySlot[] slots; // Slots de la UI del cofre (ahora es público)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Asignar la instancia
        }
        else
        {
            Debug.LogWarning("Más de una instancia de ChestUI encontrada.");
            Destroy(gameObject);
        }

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Configurar el botón de cierre
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideChest);
        }
    }

    private void Start()
    {
        chestPanel.SetActive(false);
    }

    // Mostrar la UI del cofre con animación
    public void ShowChest(Chest chest, bool isOpen)
    {
        currentChest = chest;
        chestPanel.SetActive(true);
        chestPanel.transform.localScale = Vector3.zero; // Escala inicial (cero)
        chestPanel.transform.DOScale(Vector3.one, animationDuration).SetEase(easeType); // Animación de escala
        UpdateUI(isOpen);
    }

    // Ocultar la UI del cofre con animación
    public void HideChest()
    {
        chestPanel
            .transform.DOScale(Vector3.zero, animationDuration)
            .SetEase(easeType)
            .OnComplete(() => chestPanel.SetActive(false)); // Desactivar después de la animación
    }

    // Actualizar la UI del cofre
    public void UpdateUI(bool isOpen)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < currentChest.itemsInChest.Count)
            {
                slots[i].AddItem(currentChest.itemsInChest[i]); // Mostrar el ítem en el slot
            }
            else
            {
                slots[i].ClearSlot(); // Limpiar el slot si no hay ítem
            }
        }
    }

    // Método para tomar un ítem del cofre
    public void TakeItem(InventoryItem item)
    {
        if (currentChest != null)
        {
            currentChest.TakeItem(item);
            Inventory.instance.Add(item); // Añadir el ítem al inventario
            UpdateUI(false); // Actualizar la UI del cofre
        }
    }

    // Método para tomar todos los ítems del cofre
    public void TakeAll()
    {
        if (currentChest != null)
        {
            // Recorrer la lista de ítems del cofre
            for (int i = currentChest.itemsInChest.Count - 1; i >= 0; i--)
            {
                InventoryItem item = currentChest.itemsInChest[i];

                // Intentar agregar el ítem al inventario
                if (Inventory.instance.Add(item, item.quantity))
                {
                    // Si el ítem se agregó correctamente, eliminarlo del cofre
                    currentChest.TakeItem(item);
                }
                else
                {
                    // Si el inventario está lleno, detener el proceso
                    Debug.Log("Inventario lleno. No se pueden agregar más ítems.");
                    break;
                }
            }

            // Actualizar la UI del cofre
            UpdateUI(false);
        }
    }
}
