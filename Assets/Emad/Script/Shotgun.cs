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

            GameObject projectile = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.LookRotation(dir)
            );

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(dir * projectileForce, ForceMode.Impulse);

            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null)
                proj.damage = damagePerPellet;
        }
    }

    private Vector3 GetSpreadDirection()
    {
        Vector3 forward = firePoint.forward;

        float x = Random.Range(-spreadAngle, spreadAngle);
        float y = Random.Range(-spreadAngle, spreadAngle);

        return Quaternion.Euler(y, x, 0) * forward;
    }
}
