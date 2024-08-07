using UnityEngine;
using UnityEngine.UI;

public class HealthBar : HealthSystem {
    [SerializeField] private int currentHealth;
    public override int CurrentHealth {
        get => currentHealth;
        set {
            if (assignMaxHealth) {
                maxHealth = value;
                healthBar.maxValue = maxHealth;
                assignMaxHealth = false;
            }
            SetHealth(currentHealth = value);
        }
    }
    private bool assignMaxHealth = true;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;

    public void SetHealth(int value) => healthBar.value = value;
}