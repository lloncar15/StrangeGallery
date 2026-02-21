using System;
using UnityEngine;

public class NpcHealthComponent : MonoBehaviour {
    private int _maxHealth;
    private int _currentHealth;

    public event Action OnDeath;
    public event Action<int> OnDamageTaken;

    public bool IsDead => _currentHealth <= 0;

    /// <summary>
    /// Initializes health with the given max value.
    /// Must be called before any damage is dealt.
    /// </summary>
    /// <param name="maxHealth">Maximum and starting health.</param>
    public void Initialize(int maxHealth) {
        _maxHealth = maxHealth;
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