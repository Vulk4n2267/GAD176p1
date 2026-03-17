using UnityEngine;

using System;

public class Health : MonoBehaviour, IDamageable
{
    
    [SerializeField] private float maxHealth = 100f;
    public float CurrentHealth { get; private set; }

    [SerializeField] private bool destroyOnDeath = true;

    //Events
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    private bool isDead;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        CurrentHealth -= amount;
        OnDamageTaken?.Invoke(amount);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
