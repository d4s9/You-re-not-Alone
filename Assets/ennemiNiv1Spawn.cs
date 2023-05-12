using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemiNiv1Spawn : MonoBehaviour
{
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private GameObject zombieGroup;
 
    private void OnTriggerEnter(Collider other)
    {
        if(other == playerCC)
        {
            zombieGroup.SetActive(true);
        }
    }
}
