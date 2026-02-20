using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ColorButtonUI : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Image colorImage;
    [SerializeField] private Outline outline;

    [Header("Color Effect Data")]
    [SerializeField] private List<ColorEffectData> allColorEffectData;

    private Button _button;

    public int Count { get; private set; }
    public ColorType ColorType { get; private set; }
    public event Action<ColorButtonUI> OnClicked;

    private void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy() {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    /// <summary>
    /// Sets up the button's color type and visual based on the matching ColorEffectData.
    /// </summary>
    /// <param name="colorType">The color this button represents.</param>
    /// <param name="count">Amount of the color type.</param>
    public void Initialize(ColorType colorType, int count) {
        ColorType = colorType;
        Count = count;

        ColorEffectData data = allColorEffectData.Find(d => d.colorType == colorType);
        if (data != null)
            colorImage.color = data.displayColor;
    }

    /// <summary>
    /// Sets the outline color to visually indicate selection state.
    /// </summary>
    /// <param name="color">The color to apply to the outline.</param>
    public void SetOutlineColor(Color color) {
        if (outline)
            outline.effectColor = color;
    }

    private void OnButtonClicked() {
        OnClicked?.Invoke(this);
    }
}