using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private float pickupRange = 2.6f;
     public PickupBehaviour playerPickupBehaviour;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Player player;
    [SerializeField] private GameObject pickupText;
    [SerializeField] private Inventaire inventaire;
    [SerializeField] private GameObject inventairePleinTxt;

    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position,player.transform.forward,out hit,pickupRange, layerMask))
        {
            if(hit.transform.CompareTag("Objet"))
            {
                if(inventaire.IsFull() == true)
                {
                    inventairePleinTxt.SetActive(true);
                }
                else
                {
                    pickupText.SetActive(true);
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
            
        }
        else
        {
            pickupText.SetActive(false);
            inventairePleinTxt.SetActive(false);
        }
    }
}
