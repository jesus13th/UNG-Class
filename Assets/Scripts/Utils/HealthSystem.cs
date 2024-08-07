using UnityEngine;

public abstract class HealthSystem : MonoBehaviour {

    protected static HealthSystem instance;
    public static HealthSystem Instance => instance ?? (instance = FindObjectOfType<HealthSystem>());

    public virtual int CurrentHealth { set; get; }
}
