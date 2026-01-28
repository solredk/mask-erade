using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : HealthSystem
{
    [SerializeField] private UnityEvent damageEvent;
    [SerializeField] private UnityEvent healEvent;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (healEvent != null)
            damageEvent.Invoke();
    }

    public override void HealDamage(int healing)
    {
        base.TakeDamage(healing);

        if (healEvent != null)
            healEvent.Invoke();
    }
}
