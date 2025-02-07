using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public List<InventoryItem> itemsInChest = new List<InventoryItem>(); // Lista de ítems en el cofre (ya definidos desde el inicio)
    public bool isOpen = false; // Estado del cofre (abierto/cerrado)
    public GameObject interactionIcon; // Icono de interacción (por ejemplo, un botón "E")
    public Material chestMaterial; // Material del cofre
    public Color outlineColor = Color.white; // Color del outline cuando el jugador está cerca
    public float animationDuration = 0.3f; // Duración de la animación

    private Color originalOutlineColor; // Color original del outline
    private CanvasGroup iconCanvasGroup; // Componente CanvasGroup del icono

    private void Start()
    {
        if (interactionIcon != null)
        {
            // Obtener o añadir un CanvasGroup al icono
            iconCanvasGroup = interactionIcon.GetComponent<CanvasGroup>();
            if (iconCanvasGroup == null)
            {
                iconCanvasGroup = interactionIcon.AddComponent<CanvasGroup>();
            }

            // Configurar el icono como invisible al inicio
            iconCanvasGroup.alpha = 0;
            interactionIcon.transform.localScale = Vector3.zero;
            interactionIcon.SetActive(false);
        }

        if (chestMaterial != null)
        {
            // Guardar el color original del outline
            originalOutlineColor = chestMaterial.GetColor("_OutlineColor");
        }
    }

    // Método para abrir el cofre
    public void OpenChest()
    {
        isOpen = true;
        Debug.Log("Cofre abierto.");

        // Mostrar la UI del cofre con los ítems
        ChestUI.instance.ShowChest(this, isOpen);
    }

    // Método para tomar un ítem del cofre
    public void TakeItem(InventoryItem item)
    {
        if (itemsInChest.Contains(item))
        {
            itemsInChest.Remove(item);
            Debug.Log("Ítem tomado: " + item.item.name);
        }
    }

    // Mostrar el icono de interacción y cambiar el color del outline
    public void ShowInteractionIcon()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(true);

            // Animación de escala y opacidad del icono
            interactionIcon
                .transform.DOScale(Vector3.one, animationDuration)
                .SetEase(Ease.OutBack);
            iconCanvasGroup.DOFade(1, animationDuration).SetEase(Ease.OutQuad);
        }
    }

    // Ocultar el icono de interacción y restaurar el color original del outline
    public void HideInteractionIcon()
    {
        if (interactionIcon != null)
        {
            // Animación de escala y opacidad del icono
            interactionIcon
                .transform.DOScale(Vector3.zero, animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => interactionIcon.SetActive(false)); // Desactivar después de la animación
            iconCanvasGroup.DOFade(0, animationDuration).SetEase(Ease.InQuad);
        }
    }
}
