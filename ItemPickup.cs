using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public Item Pickup()
    {
        Destroy(gameObject); // Destruir el objeto en el mundo
        return item; // Devolver el objeto recogido
    }
}