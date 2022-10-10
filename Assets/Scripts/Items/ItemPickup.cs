using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private int amountToAdd;

    //################ #################
    //------------UNITY F--------------
    //################ #################

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupItem();
        }
    }

    //################ #################
    //----------CLASS METHODS-----------
    //################ #################

    private void PickupItem()
    {
        InventoryManager.instance.AddCoinsToInventory(amountToAdd);
        AudioManager.instance.PlaySFX("ItemPickupSFX");
        Destroy(gameObject);
    }
}
