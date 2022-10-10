using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int coinsHeld;

    public static InventoryManager instance; 

    //################ #################
    //------------UNITY F--------------
    //################ #################

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    //################ #################
    //----------CLASS METHODS-----------
    //################ #################

    public void AddCoinsToInventory(int amountToAdd)
    {
        coinsHeld += amountToAdd;
    }

    //################ #################
    //--------------GETTERS-----------
    //################ #################
    public int GetCoinsHeld()
    {
        return coinsHeld;
    }
}
