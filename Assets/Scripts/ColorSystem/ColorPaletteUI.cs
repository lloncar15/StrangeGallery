using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPaletteUI : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform colorButtonContainer;
    [SerializeField] private ColorButtonUI colorButtonPrefab;
    [SerializeField] private Button lockInButton;

    [Header("Selection")]
    [SerializeField] private Color selectedOutlineColor = Color.white;
    [SerializeField] private Color deselectedOutlineColor = Color.clear;

    private readonly List<ColorButtonUI> _spawnedButtons = new();
    private ColorButtonUI _selectedButton;

    public static event Action<ColorType> OnColorSelected;
    public static event Action OnLockInPressed;

    private void Awake() {
        lockInButton.onClick.AddListener(OnLockInClicked);
    }

    private void OnDestroy() {
        lockInButton.onClick.RemoveListener(OnLockInClicked);
    }

    /// <summary>
    /// Populates the palette with buttons for each collected color.
    /// Clears any previously spawned buttons before building.
    /// </summary>
    /// <param name="collectedColors">The color entries available to the player.</param>
    public void Initialize(List<ColorEntry> collectedColors) {
        ClearButtons();

        foreach (ColorEntry entry in collectedColors) {
            ColorButtonUI button = Instantiate(colorButtonPrefab, colorButtonContainer);
            button.Initialize(entry.colorType, entry.count);
            button.OnClicked += OnColorButtonClicked;
            _spawnedButtons.Add(button);
        }
    }

    /// <summary>
    /// Destroys all spawned color buttons and clears the internal list.
    /// </summary>
    private void ClearButtons() {
        foreach (ColorButtonUI button in _spawnedButtons) {
            button.OnClicked -= OnColorButtonClicked;
            Destroy(button.gameObject);
        }
        _spawnedButtons.Clear();
        _selectedButton = null;
    }

    private void OnColorButtonClicked(ColorButtonUI button) {
        if (_selectedButton == button) {
            DeselectButton(button);
            OnColorSelected?.Invoke(ColorType.None);
            return;
        }

        if (_selectedButton != null)
            DeselectButton(_selectedButton);

        SelectButton(button);
        OnColorSelected?.Invoke(button.ColorType);
    }

    private void SelectButton(ColorButtonUI button) {
        _selectedButton = button;
        button.SetOutlineColor(selectedOutlineColor);
    }

    private void DeselectButton(ColorButtonUI button) {
        _selectedButton = null;
        button.SetOutlineColor(deselectedOutlineColor);
    }

    private void OnLockInClicked() {
        OnLockInPressed?.Invoke();
    }
}