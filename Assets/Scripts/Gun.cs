using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;

    private void Start()
    {
        PlayerShoot.shootInput += shoot;
    }

    public void shoot()
    {
        Debug.Log("Shot fired !");
    }
}
