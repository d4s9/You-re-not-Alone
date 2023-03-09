using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private float pickupRange = 20000.6f;
    public Inventaire inventaire;

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
                    inventaire.contenu.Add(hit.transform.gameObject.GetComponent<Item>().item);
                    Destroy(hit.transform.gameObject);
                }
            }
            
        }
    }
}
