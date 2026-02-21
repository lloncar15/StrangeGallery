using UnityEngine;

/// <summary>
/// Interface for any entity that can be affected by color effects.
/// </summary>
public interface IColorAffectable {
    /// <summary>
    /// Deals damage to the target.
    /// </summary>
    /// <param name="amount">Amount of damage to deal.</param>
    void TakeDamage(int amount);

    /// <summary>
    /// Instantly kills the target.
    /// </summary>
    void InstantKill();

    /// <summary>
    /// Applies a speed multiplier to the target.
    /// </summary>
    /// <param name="factor">Multiplier between 0 and 1.</param>
    void ApplySlow(float factor);

    /// <summary>
    /// Removes one slow stack from the target.
    /// </summary>
    void RemoveSlow();

    /// <summary>
    /// Applies a knockback impulse away from a source position.
    /// </summary>
    /// <param name="sourcePosition">World position of the knockback source.</param>
    /// <param name="force">Strength of the knockback.</param>
    void ApplyKnockback(Vector2 sourcePosition, float force);

    Transform Transform { get; }
}