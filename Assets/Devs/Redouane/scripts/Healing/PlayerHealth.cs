using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : HealthSystem
{
    [Header("health bar display")]
    [SerializeField] private float lerpTimer;
    [SerializeField] private float chipSpeed = 2f;

    [SerializeField] private bool damaged;
    [SerializeField] private bool healing;

    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;

    [SerializeField] private UnityEvent damageEvent;
    [SerializeField] private UnityEvent healEvent;

    private void Update()
    {
        UpdateHealthUI();
        if (damaged)
        {
            TakeDamage(10);
            damaged = false;
        }
        if (healing)
        {
            HealDamage(10);
            healing = false;
        }
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("Player took damage: " + damage);
        lerpTimer = 0f;
        base.TakeDamage(damage);
        UpdateHealthUI();
        if (healEvent != null)
            damageEvent.Invoke();

    }

    public override void HealDamage(int healing)
    {
        base.HealDamage(healing);
        lerpTimer = 0f;
        UpdateHealthUI();
        if (healEvent != null)
            healEvent.Invoke();
    }
    private void UpdateHealthUI()
    {
        //putting the front and back fill amount in a variable
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        //calculating the fraction of the hitpoints
        float hFraction = currentHealth / maxHealth;
        //lerping the fill amount of the front and back health bar
        lerpTimer += Time.unscaledDeltaTime;

        //squaring the percentage complete to make the lerp more smooth
        float percentageComplete = Mathf.Clamp01(lerpTimer / chipSpeed);
        percentageComplete *= percentageComplete;

        //if the fill amount of the front health bar is bigger than the fraction of the hitpoints so you take damage
        if (fillBack > hFraction)
        {
            //set the fill amount of the front health bar to the fraction of the hitpoints
            frontHealthBar.fillAmount = hFraction;
            //lerp the fill amount of the back health bar to the fraction of the hitpoints to make sure it goes smooth
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentageComplete);
            //set the color of the back health bar to red so you are able to see that you are taking damage
            backHealthBar.color = Color.red;
        }
        //if the fill amount of the front health bar is smaller than the fraction of the hitpoints so you heal
        else if (fillFront < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentageComplete);
            backHealthBar.color = Color.green;
        }
    }
}
