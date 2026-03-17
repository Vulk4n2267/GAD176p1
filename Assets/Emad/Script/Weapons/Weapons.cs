using UnityEngine;
using System.Collections;

public abstract class IWeapon : MonoBehaviour
{
    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;

    protected bool isReloading;

    [Header("Firing")]
    public float fireRate = 0.2f;
    protected float nextFireTime;

    public enum FireMode { SemiAuto, FullAuto }
    public FireMode fireMode = FireMode.FullAuto;

    protected bool isFiringHeld;
    protected bool firePressedThisFrame;

    protected virtual void Start()
    {
        currentAmmo = magazineSize;
    }

    protected virtual void Update()
    {
        HandleFiring();
        firePressedThisFrame = false; // reset each frame
    }

    private void HandleFiring()
    {
        if (isReloading) return;

        if (fireMode == FireMode.FullAuto && isFiringHeld)
            TryFire();

        if (fireMode == FireMode.SemiAuto && firePressedThisFrame)
            TryFire();
    }

    protected void TryFire()
    {
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

    public virtual void OnFireStarted()
    {
        isFiringHeld = true;
        firePressedThisFrame = true;
    }

    public virtual void OnFireCanceled()
    {
        isFiringHeld = false;
    }

    public virtual void OnReload()
    {
        if (!isReloading)
            StartCoroutine(Reload());
    }

    public virtual IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
    }
}

