using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private float pickupRange = 2.6f;
   [SerializeField] public PickupBehaviour playerPickupBehaviour;

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,transform.forward,out hit,pickupRange))
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
