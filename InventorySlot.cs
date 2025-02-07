using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot
    : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IPointerClickHandler
{
    public Image icon; // Referencia al ícono del ítem
    public Button removeButton; // Botón para eliminar el ítem
    public InventoryItem inventoryItem; // 🔹 Cambiamos de Item a InventoryItem

    private Transform parentToReturnTo = null;
    private bool isDragging = false;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform iconRectTransform;
    public TMP_Text quantityText; // 🔹 Nuevo: Mostrar cantidad

    public bool IsEmpty => inventoryItem == null;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = icon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = icon.gameObject.AddComponent<CanvasGroup>();
        }
        iconRectTransform = icon.GetComponent<RectTransform>();
    }

    public void AddItem(InventoryItem newItem)
    {
        if (newItem == null)
        {
            Debug.LogWarning("Se intentó añadir un ítem nulo al inventario.");
            return;
        }

        inventoryItem = newItem;
        icon.sprite = inventoryItem.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;

        UpdateQuantity();
    }

    public void ClearSlot()
    {
        inventoryItem = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        quantityText.text = "";
    }

    private void UpdateQuantity()
    {
        if (inventoryItem != null && inventoryItem.item.isStackable)
        {
            quantityText.text = inventoryItem.quantity.ToString();
        }
        else
        {
            quantityText.text = "";
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            parentToReturnTo = transform;
            icon.transform.SetParent(canvas.transform, true);
            canvasGroup.blocksRaycasts = false;
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out Vector2 localPoint
            );
            iconRectTransform.localPosition = localPoint;
        }
    }

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

    public void OnDrop(PointerEventData eventData)
    {
        // Verificar si el objeto arrastrado es un InventorySlot
        InventorySlot otherSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (otherSlot != null && otherSlot.inventoryItem != null)
        {
            if (IsEmpty)
            {
                // Si el slot de destino está vacío, mover el ítem aquí
                AddItem(otherSlot.inventoryItem);
                otherSlot.ClearSlot();
            }
            else
            {
                // Si el slot de destino está ocupado, intercambiar los ítems
                InventoryItem tempItem = inventoryItem;
                AddItem(otherSlot.inventoryItem);
                otherSlot.AddItem(tempItem);
            }
        }

        // Verificar si el objeto arrastrado es un EquipmentSlotUI
        // EquipmentSlotUI equipmentSlot = eventData.pointerDrag.GetComponent<EquipmentSlotUI>();
        // if (equipmentSlot != null && equipmentSlot.equippedItem != null)
        // {
        //     AddItem(equipmentSlot.equippedItem);
        //     // Desequipar el ítem y moverlo al inventario
        //     EquipmentManager.instance.Unequip((int)equipmentSlot.equipmentSlot);
        //     equipmentSlot.UnequipItem();
        // }
    }

    // Detectar clic derecho
    public void OnPointerClick(PointerEventData eventData)
    {
        if (
            eventData.button == PointerEventData.InputButton.Right
            && inventoryItem != null
            && inventoryItem.item != null
        )
        {
            // Obtener la posición del cursor en coordenadas de pantalla
            Vector2 cursorPosition = eventData.position;
            // Mostrar detalles del ítem usando el singleton
            ItemDetailsPanelManager.instance.ShowItemDetails(inventoryItem.item, cursorPosition);
        }
    }
}
