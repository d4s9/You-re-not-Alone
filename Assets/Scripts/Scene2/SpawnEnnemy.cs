using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnnemy : MonoBehaviour
{
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private GameObject zombieGroup;

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCC)
        {
            zombieGroup.SetActive(true);
        }
    }
}