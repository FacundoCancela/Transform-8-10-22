using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsHeldTextValue;

    //################ #################
    //------------UNITY F--------------
    //################ #################

    private void Update()
    {
        coinsHeldTextValue.text = "x " + InventoryManager.instance.GetCoinsHeld().ToString();
    }
}
