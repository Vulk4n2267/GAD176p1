using UnityEngine;

public abstract class ProjectileWeapon : IWeapon
{
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 20f;

    [Header("Damage")]
    public float damage = 10f;

    protected void SpawnProjectile(Vector3 direction, float dmg)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(direction * projectileForce, ForceMode.Impulse);

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
            proj.damage = dmg;
    }
}