using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += shoot;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void shoot()
    {
        if (gunData.currentAmmo > 0)
        {

        }
    }
}
