using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{

  [SerializeField] private Inventaire inventaire;


    public void DoPickup(Item item)
    {
        inventaire.AddItem(item.itemData);
        Destroy(item.gameObject);
    }


}
