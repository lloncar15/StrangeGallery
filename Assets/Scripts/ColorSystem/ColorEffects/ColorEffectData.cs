using UnityEngine;

[CreateAssetMenu(fileName = "Color Effect Data", menuName = "GimGim/Painting/Color Effect Data")]
public class ColorEffectData : ScriptableObject {
    [Header("Color")]
    public ColorType colorType;
    public Color displayColor = Color.white;

    [Header("Red Settings")]
    public int damageAmount = 1;

    [Header("Blue Settings")]
    [Range(0f, 1f)] public float slowFactor = 0.5f;

    [Header("Black Settings")]
    public bool instantKill = true;
}