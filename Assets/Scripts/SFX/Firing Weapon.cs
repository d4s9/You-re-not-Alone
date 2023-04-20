using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringWeapon : MonoBehaviour
{
    public AudioSource firingWeaponSound;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            firingWeaponSound.enabled = true;
        }
        else
        {
            firingWeaponSound.enabled = false;
        }
    }
}
