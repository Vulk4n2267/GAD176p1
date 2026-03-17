using UnityEngine;

public abstract class ProjectileWeapon : Weapons
{
    
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 20f;

    public float damage = 10f;

    protected void SpawnProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * projectileForce, ForceMode.Impulse);
        }

        // Assign damage dynamically
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.damage = damage;
        }
    }
}