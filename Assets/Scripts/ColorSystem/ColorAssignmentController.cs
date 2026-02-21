using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorAssignmentController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera paintingCamera;
    [SerializeField] private ColorPaletteUI colorPaletteUI;
    [SerializeField] private CollectedColors collectedColors;

    private List<PaintableSprite> _paintableSprites;
    private ColorType _selectedColor = ColorType.None;
    private bool _isActive;

    public static event Action OnLockIn;

    private void OnEnable() {
        ColorPaletteUI.OnColorSelected += OnColorSelected;
        ColorPaletteUI.OnLockInPressed += OnLockInPressed;

        InputController.OnPaintInteract += TryPaintAtMousePosition;
        InputController.OnPaintRemove += TryRemoveColorsAtMousePosition;
    }

    private void OnDisable() {
        ColorPaletteUI.OnColorSelected -= OnColorSelected;
        ColorPaletteUI.OnLockInPressed -= OnLockInPressed;

        InputController.OnPaintInteract -= TryPaintAtMousePosition;
        InputController.OnPaintRemove -= TryRemoveColorsAtMousePosition;
    }

    /// <summary>
    /// Prepares the controller with the list of sprites that can be painted this session.
    /// </summary>
    /// <param name="paintableSprites">All paintable sprites in the final painting.</param>
    public void Initialize(List<PaintableSprite> paintableSprites) {
        _paintableSprites = paintableSprites;
        _selectedColor = ColorType.None;
    }

    /// <summary>
    /// Enables click-to-paint interaction.
    /// </summary>
    public void Enable() => _isActive = true;

    /// <summary>
    /// Disables click-to-paint interaction.
    /// </summary>
    public void Disable() {
        _isActive = false;
        _selectedColor = ColorType.None;
    }

    /// <summary>
    /// Raycasts from the painting camera at the mouse position to find a PaintableSprite.
    /// If found and the player has the selected color available, assigns it and spends the color.
    /// </summary>
    private void TryPaintAtMousePosition() {
        if (!_isActive || _selectedColor == ColorType.None) return;

        PaintableSprite sprite = RaycastForPaintableSprite();
        if (sprite == null) return;

        if (!collectedColors.SpendColor(_selectedColor)) return;

        sprite.AssignColor(_selectedColor);
    }

    /// <summary>
    /// Raycasts from the painting camera at the mouse position to find a PaintableSprite.
    /// If found, refunds each assigned color back to inventory and clears the sprite.
    /// </summary>
    private void TryRemoveColorsAtMousePosition() {
        if (!_isActive) return;

        PaintableSprite sprite = RaycastForPaintableSprite();
        if (sprite == null) return;

        List<ColorType> colors = sprite.GetAssignedColors();
        foreach (ColorType color in colors)
            collectedColors.RefundColor(color);

        sprite.ClearColors();
    }

    /// <summary>
    /// Raycasts from the painting camera at the mouse position and returns the hit PaintableSprite,
    /// or null if none was found or it isn't in the allowed list.
    /// </summary>
    /// <returns>The hit PaintableSprite, or null.</returns>
    private PaintableSprite RaycastForPaintableSprite() {
        Vector2 worldPoint = paintingCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (!hit.collider) return null;
        if (!hit.collider.TryGetComponent(out PaintableSprite sprite)) return null;
        if (!_paintableSprites.Contains(sprite)) return null;

        return sprite;
    }

    private void OnColorSelected(ColorType colorType) {
        _selectedColor = colorType;
    }

    private void OnLockInPressed() {
        OnLockIn?.Invoke();
    }
}