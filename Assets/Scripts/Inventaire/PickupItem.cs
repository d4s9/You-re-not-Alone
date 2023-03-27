using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private float pickupRange = 20.6f;
     public PickupBehaviour playerPickupBehaviour;
     [SerializeField] private LayerMask layerMask;

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,transform.forward,out hit,pickupRange, layerMask))
        {
            if(hit.transform.CompareTag("Objet"))
            {
                Debug.Log("Il y a un objet");
                if(Input.GetKeyDown(KeyCode.E))
                {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
            
        }
    }
}
