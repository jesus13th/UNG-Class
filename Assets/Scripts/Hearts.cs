using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class Hearts : HealthSystem {
    [SerializeField] private int currentHealth = 6;
    public override int CurrentHealth {
        get => currentHealth;
        set => SetHearts(currentHealth = value);
    }
    [SerializeField] private Sprite heart, emptyHeart;
    [SerializeField] private Transform heartsParent;
    [SerializeField] private Image[] hearts;

    private void Awake() {
        instance = this;
        hearts = heartsParent.GetComponentsInChildren<Image>().Where(t => t.transform != heartsParent).ToArray();
    }
    private void SetHearts(int n) {
        hearts.ToList().ForEach(t => t.sprite = emptyHeart);
        hearts.Take(n).ToList().ForEach(x => x.sprite = heart);
    }
}