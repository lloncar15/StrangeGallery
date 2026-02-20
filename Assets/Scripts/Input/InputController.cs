using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : PersistentSingleton<InputController> {
    private PlayerInputActions _inputActions;
    
    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}

    public static Action Test;
    public static Action OnInteractPressed;
    public static Action OnExitPressed;
    
    public static event Action OnPaintInteract;
    public static event Action OnPaintRemove;
    
    protected override void Awake() {
        _inputActions = new PlayerInputActions();
        HideCursor();
        base.Awake();
    }
    
    private void OnEnable() {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();

        _inputActions.Player.Interact.performed += OnInteract;
        _inputActions.Player.Exit.performed += OnExit;
        _inputActions.Player.Test.performed += OnTest;

        _inputActions.Player.Interact.performed += OnInteractPerformed;
        _inputActions.Player.PaintRemove.performed += OnPaintRemovePerformed;
    }

    private void OnDisable() {
        if (_inputActions == null) 
            return;
        
        _inputActions.Player.Test.performed -= OnTest;
        _inputActions.Player.Interact.performed -= OnInteract;
        _inputActions.Player.Exit.performed -= OnExit;
        
        _inputActions.Player.Interact.performed -= OnInteractPerformed;
        _inputActions.Player.PaintRemove.performed -= OnPaintRemovePerformed;
        
        _inputActions.Disable();
    }

    private void Update() {
        ReadMoveInputs();
    }

    /// <summary>
    /// Forwards the test action to subscribers.
    /// </summary>
    private void OnTest(InputAction.CallbackContext ctx) {
        Test?.Invoke();
    }

    private void OnExit(InputAction.CallbackContext ctx) {
        OnExitPressed?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext ctx) {
        OnInteractPressed?.Invoke();
    }

    /// <summary>
    /// Reads current move and look input values from the input action asset.
    /// </summary>
    private void ReadMoveInputs() {
        MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = _inputActions.Player.Look.ReadValue<Vector2>();
    }

    /// <summary>
    /// Locks and hides the cursor for FPS gameplay.
    /// </summary>
    private static void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Unlocks and shows the cursor.
    /// </summary>
    private static void ShowCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void OnInteractPerformed(InputAction.CallbackContext ctx) {
        OnPaintInteract?.Invoke();
    }

    private void OnPaintRemovePerformed(InputAction.CallbackContext ctx) {
        OnPaintRemove?.Invoke();
    }
}
