using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorAssignmentController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera paintingCamera;
    [SerializeField] private ColorPaletteUI colorPaletteUI;

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

    private void Update() {
        if (!_isActive) return;
        if (_selectedColor == ColorType.None) return;

        if (Input.GetMouseButtonDown(0))
            TryPaintAtMousePosition();
    }

    /// <summary>
    /// Raycasts from the painting camera at the mouse position to find a PaintableSprite.
    /// If found, assigns the currently selected color to it.
    /// </summary>
    private void TryPaintAtMousePosition() {
        if (!_isActive || _selectedColor == ColorType.None) return;

        Vector2 worldPoint = paintingCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (!hit.collider) return;
        if (!hit.collider.TryGetComponent(out PaintableSprite sprite)) return;
        if (!_paintableSprites.Contains(sprite)) return;

        sprite.AssignColor(_selectedColor);
    }

    /// <summary>
    /// Raycasts from the painting camera at the mouse position to find a PaintableSprite.
    /// If found, clears all assigned colors from it immediately.
    /// </summary>
    private void TryRemoveColorsAtMousePosition() {
        if (!_isActive) return;

        Vector2 worldPoint = paintingCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (!hit.collider) return;
        if (!hit.collider.TryGetComponent(out PaintableSprite sprite)) return;
        if (!_paintableSprites.Contains(sprite)) return;

        sprite.ClearColors();
    }

    private void OnColorSelected(ColorType colorType) {
        _selectedColor = colorType;
    }

    private void OnLockInPressed() {
        OnLockIn?.Invoke();
    }
}