using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;



    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;

    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                var bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.Euler(0,90,0));
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);

                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {

                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.damage(gunData.damage);

                }
                gunData.currentAmmo--;
                timeSinceLastShot = 0f;
                OnGunShot();
            }
            
        }
    }

    private void Update()
    {
        Debug.DrawRay(muzzle.position, muzzle.forward, Color.red);
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot()
    {
        
    }
}
