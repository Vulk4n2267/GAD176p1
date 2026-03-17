using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    protected override void Fire()
    {
        SpawnProjectile(firePoint.forward, damage);
    }
}