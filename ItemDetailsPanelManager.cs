using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsPanelManager : MonoBehaviour
{
    public static ItemDetailsPanelManager instance; // Singleton

    public GameObject itemDetailsPanel; // Referencia al panel de detalles
    public TMP_Text itemDetailsText; // Referencia al texto de detalles
    public Button actionButtonPrefab; // Prefab del botón de acción
    public Transform actionsParent; // Contenedor de los botones de acción
    public Canvas canvas; // Referencia al Canvas

    private Dictionary<string, System.Action<Item>> itemActions; // Diccionario de acciones

    private void Awake()
    {
        // Configurar el singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar el diccionario de acciones
        itemActions = new Dictionary<string, System.Action<Item>>();
        InitializeActions();
    }

    private void Start()
    {
        itemDetailsPanel.SetActive(false);
    }

    // Inicializar las acciones disponibles
    private void InitializeActions()
    {
        itemActions.Add(
            "Comer",
            item =>
            {
                if (item.itemType is ItemType.Food)
                {
                    Debug.Log($"Comiendo {item.itemName}. Restaura {30} de salud.");
                    // Aquí puedes agregar la lógica para comer el ítem
                    Inventory.instance.Remove(item, 1); // Eliminar un ítem del inventario
                }
            }
        );

        itemActions.Add(
            "Equipar",
            item =>
            {
                if (item.itemType is ItemType.Weapon)
                {
                    Debug.Log($"Usando {item.itemName}...");
                    // Aquí puedes agregar la lógica para comer el ítem
                    Inventory.instance.Remove(item, 1); // Eliminar un ítem del inventario
                }
            }
        );

        // itemActions.Add(
        //     "Usar",
        //     item =>
        //     {
        //         Debug.Log($"Usando {item.itemName}...");
        //         // Aquí puedes agregar la lógica para usar el ítem
        //     }
        // );

        // itemActions.Add(
        //     "Tirar",
        //     item =>
        //     {
        //         Debug.Log($"Tirando {item.itemName}...");
        //         // Aquí puedes agregar la lógica para tirar el ítem
        //         Inventory.instance.Remove(item, 1); // Eliminar un ítem del inventario
        //     }
        // );
    }

    // Mostrar el panel de detalles
    public void ShowItemDetails(Item item, Vector2 cursorPosition)
    {
        // Actualizar el texto de detalles
        itemDetailsText.text = $"Nombre: {item.itemName}\nDescripción: {item.description}";

        // Limpiar los botones de acciones anteriores
        foreach (Transform child in actionsParent)
        {
            Destroy(child.gameObject);
        }

        // Crear botones para las acciones disponibles
        foreach (var action in itemActions)
        {
            Button actionButton = Instantiate(actionButtonPrefab, actionsParent);
            actionButton.GetComponentInChildren<TMP_Text>().text = action.Key;
            actionButton.onClick.AddListener(() => action.Value(item));
        }

        // Convertir las coordenadas de la pantalla a coordenadas del mundo en el espacio del Canvas
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            cursorPosition,
            null,
            out Vector3 worldPoint
        );

        // Asignar la posición al panel
        itemDetailsPanel.transform.position = worldPoint;

        // Ajustar la posición para que el panel no se salga de la pantalla
        RectTransform panelRect = itemDetailsPanel.GetComponent<RectTransform>();
        Vector2 panelSize = panelRect.sizeDelta;

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (screenPosition.x + panelSize.x > screenWidth)
        {
            screenPosition.x = screenWidth - panelSize.x;
        }
        if (screenPosition.y - panelSize.y < 0)
        {
            screenPosition.y = panelSize.y;
        }

        // Convertir las coordenadas ajustadas de nuevo a coordenadas del mundo
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            null,
            out worldPoint
        );

        // Asignar la posición ajustada al panel
        itemDetailsPanel.transform.position = worldPoint;

        // Mostrar el panel con animación
        itemDetailsPanel.transform.localScale = Vector3.zero;
        itemDetailsPanel.SetActive(true);
        itemDetailsPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    // Obtener la posición del panel ajustada a los límites de la pantalla
    private Vector2 GetPanelPosition(Vector2 slotPosition)
    {
        RectTransform panelRect = itemDetailsPanel.GetComponent<RectTransform>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Convertir la posición del slot a coordenadas de la pantalla
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, slotPosition);

        // Convertir las coordenadas de la pantalla a coordenadas del Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            null,
            out Vector2 localPoint
        );

        // Ajustar la posición para que el panel no se salga de la pantalla
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;

        if (localPoint.x + panelWidth > canvasRect.rect.width / 2)
        {
            localPoint.x = canvasRect.rect.width / 2 - panelWidth;
        }
        if (localPoint.y - panelHeight < -canvasRect.rect.height / 2)
        {
            localPoint.y = -canvasRect.rect.height / 2 + panelHeight;
        }

        return localPoint;
    }

    // Ocultar el panel de detalles con animación
    public void HideItemDetails()
    {
        itemDetailsPanel
            .transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => itemDetailsPanel.SetActive(false));
    }
}
