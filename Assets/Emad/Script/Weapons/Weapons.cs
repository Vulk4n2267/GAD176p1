using UnityEngine;
using System.Collections;

public abstract class Weapons : MonoBehaviour
{
    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    protected bool isReloading;

    [Header("Firing")]
    public float fireRate = 0.2f;
    protected float nextFireTime;

    protected virtual void Start()
    {
        currentAmmo = magazineSize;
    }

    public virtual void TryFire()
    {
        if (isReloading) return;
        if (Time.time < nextFireTime) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        Fire();
        currentAmmo--;
        nextFireTime = Time.time + fireRate;
    }

    protected abstract void Fire();

    protected virtual IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
    }
}
