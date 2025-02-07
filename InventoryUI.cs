using DG.Tweening; // Importar DOTween
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IPointerClickHandler
{
    public StarterAssetsInputs starterAssetsInputs;
    public Transform itemsParent; // Contenedor de los slots del inventario
    public GameObject inventoryUI; // UI del inventario (el panel principal)
    public float animationDuration = 0.5f; // Duración de la animación
    public Ease easeType = Ease.OutBack; // Tipo de easing para la animación

    private Inventory inventory;
    private InventorySlot[] slots;
    private bool isInventoryVisible = false; // Estado del inventario (visible/oculto)
    public GameObject itemDetailsPanel; // Referencia al panel de detalles

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; // Suscribirse al evento de cambios

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Ocultar el inventario al inicio (opcional)
        if (inventoryUI != null)
        {
            inventoryUI.transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        // Detectar si se presiona la tecla I
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // Actualizar la UI del inventario
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]); // Añadir ítem al slot
            }
            else
            {
                slots[i].ClearSlot(); // Limpiar el slot si no hay ítem
            }
        }
    }

    // Mostrar u ocultar el inventario
    private void ToggleInventory()
    {
        // Cambiar el estado del inventario
        isInventoryVisible = !isInventoryVisible;

        // Animación usando DOTween
        if (inventoryUI != null)
        {
            if (isInventoryVisible)
            {
                // Mostrar el inventario
                inventoryUI.transform.DOScale(Vector3.one, animationDuration).SetEase(easeType);
                //Mostrar el cursor
                starterAssetsInputs.SetCursorState(false);
            }
            else
            {
                // Ocultar el inventario
                inventoryUI.transform.DOScale(Vector3.zero, animationDuration).SetEase(easeType);
                //Ocultar el cursor
                starterAssetsInputs.SetCursorState(true);
            }
        }
    }

    // Detectar clic fuera del panel de detalles
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            itemDetailsPanel.SetActive(false);
        }
    }
}
