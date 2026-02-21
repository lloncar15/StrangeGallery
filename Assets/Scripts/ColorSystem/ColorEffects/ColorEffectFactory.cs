using System.Collections.Generic;
using UnityEngine;

public class ColorEffectFactory {
    private readonly Dictionary<ColorType, ColorEffectData> _dataMap = new();
    private readonly Transform _spriteTransform;

    /// <summary>
    /// Initializes the factory with all available color effect data assets.
    /// </summary>
    /// <param name="effectDataAssets">All ColorEffectData scriptable objects.</param>
    /// <param name="spriteTransform">Transform of the owning PaintableSprite, used by directional effects.</param>
    public ColorEffectFactory(IEnumerable<ColorEffectData> effectDataAssets, Transform spriteTransform) {
        _spriteTransform = spriteTransform;
        foreach (ColorEffectData data in effectDataAssets) {
            _dataMap[data.colorType] = data;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Creates an IColorEffect instance for the given color type.
    /// Returns null if the color type is unrecognized or None.
    /// </summary>
    /// <param name="colorType">The color type to create an effect for.</param>
    /// <returns>The corresponding IColorEffect, or null.</returns>
    public IColorEffect Create(ColorType colorType) {
        if (!_dataMap.TryGetValue(colorType, out ColorEffectData data)) {
            Debug.LogWarning($"[ColorEffectFactory] No data found for color: {colorType}");
            return null;
        }

        return colorType switch {
            ColorType.Red => new RedColorEffect(data),
            ColorType.Green => new GreenColorEffect(),
            ColorType.Yellow => new YellowColorEffect(data, _spriteTransform),
            ColorType.Blue => new BlueColorEffect(data),
            ColorType.Black => new BlackColorEffect(),
            _ => null
        };
    }
}