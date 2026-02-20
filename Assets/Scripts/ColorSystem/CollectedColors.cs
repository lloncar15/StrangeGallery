using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Collected Colors", menuName = "GimGim/Painting/Collected Colors")]
public class CollectedColors : ScriptableObject {
    [SerializeField] private List<ColorEntry> collectedColors = new();

    /// <summary>
    /// Adds a quantity of a color to the player's collection.
    /// If the color already exists, its count is incremented.
    /// </summary>
    /// <param name="colorType">The color to collect.</param>
    /// <param name="quantity">The amount to add.</param>
    public void CollectColor(ColorType colorType, int quantity = 1) {
        if (colorType == ColorType.None || quantity <= 0)
            return;

        ColorEntry existing = FindEntry(colorType);
        if (existing != null) {
            existing.count += quantity;
            return;
        }

        collectedColors.Add(new ColorEntry(colorType, quantity));
    }

    /// <summary>
    /// Attempts to spend one unit of a color. Used when assigning a color to a sprite.
    /// </summary>
    /// <param name="colorType">The color to spend.</param>
    /// <returns>True if the color was available and decremented.</returns>
    public bool SpendColor(ColorType colorType) {
        ColorEntry entry = FindEntry(colorType);
        if (entry == null || entry.count <= 0)
            return false;

        entry.count--;
        return true;
    }

    /// <summary>
    /// Refunds one unit of a color. Used when removing a color from a sprite.
    /// </summary>
    /// <param name="colorType">The color to refund.</param>
    public void RefundColor(ColorType colorType) {
        if (colorType == ColorType.None)
            return;

        ColorEntry entry = FindEntry(colorType);
        if (entry != null) {
            entry.count++;
            return;
        }

        collectedColors.Add(new ColorEntry(colorType, 1));
    }

    /// <summary>
    /// Returns whether the player has at least one unit of a given color.
    /// </summary>
    /// <param name="colorType">The color to check.</param>
    /// <returns>True if the player has one or more of this color.</returns>
    public bool HasColor(ColorType colorType) {
        ColorEntry entry = FindEntry(colorType);
        return entry != null && entry.count > 0;
    }

    /// <summary>
    /// Returns how many units the player has of a given color.
    /// </summary>
    /// <param name="colorType">The color to check.</param>
    /// <returns>The count of that color, or 0 if not collected.</returns>
    public int GetCount(ColorType colorType) {
        ColorEntry entry = FindEntry(colorType);
        return entry?.count ?? 0;
    }

    /// <summary>
    /// Returns a copy of all collected colors and their counts.
    /// </summary>
    public List<ColorEntry> GetCollectedColors() => new(collectedColors);

    /// <summary>
    /// Clears all collected colors. Intended for testing/reset purposes.
    /// </summary>
    public void Clear() => collectedColors.Clear();

    /// <summary>
    /// Finds a color entry by type, or null if not present.
    /// </summary>
    /// <param name="colorType">The color type to search for.</param>
    /// <returns>The matching ColorEntry, or null.</returns>
    private ColorEntry FindEntry(ColorType colorType) {
        return collectedColors.FirstOrDefault(t => t.colorType == colorType);
    }
}

/// <summary>
/// Represents a color type and its collected quantity.
/// Uses a class instead of a struct so list references remain mutable.
/// </summary>
[Serializable]
public class ColorEntry {
    [SerializeField] public ColorType colorType;
    [SerializeField] public int count;

    public ColorEntry(ColorType colorType, int count) {
        this.colorType = colorType;
        this.count = count;
    }
}