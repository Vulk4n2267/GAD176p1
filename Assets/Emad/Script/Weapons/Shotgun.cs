using UnityEngine;

public class Shotgun : ProjectileWeapon
{
    public int pelletCount = 8;
    public float spreadAngle = 10f;

    protected override void Fire()
    {
        float damagePerPellet = damage / pelletCount;

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 dir = GetSpreadDirection();
            SpawnProjectile(dir, damagePerPellet);
        }
    }

    private Vector3 GetSpreadDirection()
    {
        float x = Random.Range(-spreadAngle, spreadAngle);
        float y = Random.Range(-spreadAngle, spreadAngle);

        return Quaternion.Euler(y, x, 0) * firePoint.forward;
    }
}
