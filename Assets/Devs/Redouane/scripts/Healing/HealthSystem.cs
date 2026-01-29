using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
    protected float maxHealth = 100;
    protected float currentHealth;
    protected bool isDead = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
        }
    }

    public virtual void HealDamage(int Healing)
    {
        currentHealth += Healing;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
