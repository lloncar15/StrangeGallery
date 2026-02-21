using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class PaintableSprite : MonoBehaviour {
    [Header("Config")]
    [SerializeField] private List<ColorEffectData> availableEffectData;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _triggerCollider;
    private BoxCollider2D _blockingCollider;
    private ColorEffectFactory _factory;

    private readonly List<ColorType> _assignedColors = new();
    private readonly List<IColorEffect> _activeEffects = new();

    private bool _locked;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _triggerCollider = GetComponent<Collider2D>();
        _factory = new ColorEffectFactory(availableEffectData, transform);
    }

    /// <summary>
    /// Assigns a color to this sprite and rebuilds its active effects.
    /// Has no effect if the sprite is locked or already has this color.
    /// </summary>
    /// <param name="colorType">The color to assign.</param>
    public void AssignColor(ColorType colorType) {
        if (_locked || colorType == ColorType.None) return;
        if (_assignedColors.Contains(colorType)) return;

        _assignedColors.Add(colorType);
        RebuildEffects();
        UpdateVisual();
    }

    /// <summary>
    /// Removes a specific color from this sprite and rebuilds its active effects.
    /// Has no effect if the sprite is locked.
    /// </summary>
    /// <param name="colorType">The color to remove.</param>
    public void RemoveColor(ColorType colorType) {
        if (_locked) return;
        if (!_assignedColors.Remove(colorType)) return;

        RebuildEffects();
        UpdateVisual();
    }

    /// <summary>
    /// Removes all assigned colors from this sprite.
    /// Has no effect if the sprite is locked.
    /// </summary>
    public void ClearColors() {
        if (_locked) return;
        _assignedColors.Clear();
        _activeEffects.Clear();
        UpdateVisual();
    }

    /// <summary>
    /// Locks the sprite, preventing any further color changes.
    /// If the sprite has green assigned, enables a non-trigger blocking collider.
    /// </summary>
    public void Lock() {
        _locked = true;
        if (_assignedColors.Contains(ColorType.Green))
            EnableBlockingCollider();
    }

    /// <summary>
    /// Returns a copy of the currently assigned colors.
    /// </summary>
    public List<ColorType> GetAssignedColors() => new(_assignedColors);

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.TryGetComponent(out IColorAffectable target)) return;
        foreach (IColorEffect effect in _activeEffects)
            effect.OnEnter(target);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.TryGetComponent(out IColorAffectable target)) return;
        foreach (IColorEffect effect in _activeEffects)
            effect.OnExit(target);
    }

    /// <summary>
    /// Rebuilds the active effect list from the current assigned colors via the factory.
    /// </summary>
    private void RebuildEffects() {
        _activeEffects.Clear();
        foreach (ColorType color in _assignedColors) {
            IColorEffect effect = _factory.Create(color);
            if (effect != null)
                _activeEffects.Add(effect);
        }
    }

    /// <summary>
    /// Updates the sprite's tint to reflect its assigned colors.
    /// Averages all assigned colors together. Reverts to white if no colors assigned.
    /// </summary>
    private void UpdateVisual() {
        if (_assignedColors.Count == 0) {
            _spriteRenderer.color = Color.white;
            return;
        }

        Color blended = Color.black;
        foreach (ColorType colorType in _assignedColors) {
            ColorEffectData data = availableEffectData.Find(d => d.colorType == colorType);
            if (data)
                blended += data.displayColor;
        }

        blended /= _assignedColors.Count;
        blended.a = 1f;
        _spriteRenderer.color = blended;
    }

    /// <summary>
    /// Adds a non-trigger BoxCollider2D matching the trigger collider's bounds for green obstacle blocking.
    /// </summary>
    private void EnableBlockingCollider() {
        if (_blockingCollider != null) return;

        _blockingCollider = gameObject.AddComponent<BoxCollider2D>();
        _blockingCollider.isTrigger = false;
        _blockingCollider.size = _triggerCollider.bounds.size;
    }
}