using UnityEngine;

//listneers yeeay
public class PlayerHealthUI : MonoBehaviour
{
    public Health health;

    private void OnEnable()
    {
        health.OnDamageTaken += UpdateUI;
        health.OnDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        health.OnDamageTaken -= UpdateUI;
        health.OnDeath -= OnPlayerDeath;
    }

    void UpdateUI(float damage)
    {
        Debug.Log("Update UI");
    }

    void OnPlayerDeath()
    {
        Debug.Log("Ye you died lol");
    }
}
