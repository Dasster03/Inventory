using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// IDropHandler
{
    public EquipmentSlot equipmentSlot; // Tipo de slot (por ejemplo, Weapon, Helmet, etc.)
    public Image icon;

    public Item equippedItem;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform parentToReturnTo = null;
    private bool isDragging = false;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = icon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = icon.gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void EquipItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("Se intentó equipar un ítem nulo.");
            return;
        }

        equippedItem = item;
        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
    }

    public void UnequipItem()
    {
        equippedItem = null;
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    // Manejar el inicio del arrastre
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equippedItem != null)
        {
            parentToReturnTo = transform;
            icon.transform.SetParent(canvas.transform, true);
            canvasGroup.blocksRaycasts = false;
            isDragging = true;
        }
    }

    // Manejar el arrastre
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            icon.transform.position = eventData.position;
        }
    }

    // Manejar el final del arrastre
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            canvasGroup.blocksRaycasts = true;
            icon.transform.SetParent(parentToReturnTo, false);
            icon.transform.localPosition = Vector3.zero;
            isDragging = false;
        }
    }

    // Manejar el evento OnDrop
    public void OnDrop(PointerEventData eventData)
    {
        // Verificar si el objeto arrastrado es un InventorySlot
        InventorySlot otherSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (otherSlot != null && otherSlot.inventoryItem.item != null)
        {
            // Verificar si el ítem es compatible con este slot de equipamiento
            if (otherSlot.inventoryItem.item.equipmentSlot == equipmentSlot)
            {
                // Equipar el ítem
                EquipmentManager.instance.Equip(otherSlot.inventoryItem.item);
                otherSlot.ClearSlot();
            }
            else
            {
                Debug.Log("Este ítem no es compatible con este slot de equipamiento.");
            }
        }
    }
}
