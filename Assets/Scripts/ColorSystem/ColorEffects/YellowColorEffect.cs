using UnityEngine;

/// <summary>
/// Applies a knockback impulse pushing the target away from the sprite's center on enter.
/// </summary>
public class YellowColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Yellow;

    private readonly float _knockbackForce;
    private readonly Transform _spriteTransform;

    /// <summary>
    /// Creates a yellow knockback effect.
    /// </summary>
    /// <param name="data">Color effect data containing knockback force.</param>
    /// <param name="spriteTransform">Transform of the PaintableSprite, used as knockback origin.</param>
    public YellowColorEffect(ColorEffectData data, Transform spriteTransform) {
        _knockbackForce = data.knockbackForce;
        _spriteTransform = spriteTransform;
    }

    /// <inheritdoc/>
    public void OnEnter(IColorAffectable target) {
        target.ApplyKnockback(_spriteTransform.position, _knockbackForce);
    }

    /// <inheritdoc/>
    public void OnExit(IColorAffectable target) { }
}