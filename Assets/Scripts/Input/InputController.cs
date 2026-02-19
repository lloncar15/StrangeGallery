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
    }

    private void OnDisable() {
        if (_inputActions == null) 
            return;
        
        _inputActions.Player.Test.performed -= OnTest;
        _inputActions.Player.Interact.performed -= OnInteract;
        _inputActions.Player.Exit.performed -= OnExit;
        
        _inputActions.Disable();
    }

    private void Update() {
        HandleMoveInputs();
    }

    private void OnTest(InputAction.CallbackContext ctx) {
        Test?.Invoke();
    }

    private void OnExit(InputAction.CallbackContext ctx) {
        OnExitPressed?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext ctx) {
        OnInteractPressed?.Invoke();
    }

    private void HandleMoveInputs() {
        MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = _inputActions.Player.Look.ReadValue<Vector2>();
    }

    private static void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
