using System;
using UnityEngine;

public class NpcHealthComponent : MonoBehaviour {
    [SerializeField] private int maxHealth = 3;

    private int _currentHealth;

    public event Action OnDeath;
    public event Action<int> OnDamageTaken;

    public bool IsDead => _currentHealth <= 0;

    private void Awake() {
        _currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduces health by the given amount. Triggers OnDeath if health reaches zero.
    /// </summary>
    /// <param name="amount">Amount of damage to deal.</param>
    public void TakeDamage(int amount) {
        if (IsDead) return;

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnDamageTaken?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
            OnDeath?.Invoke();
    }

    /// <summary>
    /// Kills the NPC immediately regardless of current health.
    /// </summary>
    public void InstantKill() {
        if (IsDead) return;
        _currentHealth = 0;
        OnDeath?.Invoke();
    }
}