using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{

  [SerializeField] private Inventaire inventaire;
    [SerializeField] private GameObject inventairePleinTxt;



    public void Start()
    {
        inventairePleinTxt.SetActive(false);
    }
    public void DoPickup(Item item)  
    {
        if(inventaire.IsFull())
        {
            inventairePleinTxt.SetActive(true);
        }
        else
        {
            inventaire.AddItem(item.itemData);
            Destroy(item.gameObject);
            inventairePleinTxt.SetActive(false);
        }
    }


}
