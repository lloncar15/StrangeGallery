using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : PersistentSingleton<InputController> {
    private PlayerInputActions _inputActions;
    
    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}

    public static Action Test;
    
    protected override void Awake() {
        _inputActions = new PlayerInputActions();
        HideCursor();
        base.Awake();
    }
    
    private void OnEnable() {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();

        _inputActions.Player.Test.performed += OnTest;
    }

    private void OnDisable() {
        _inputActions.Player.Test.performed -= OnTest;
        
        _inputActions?.Disable();
    }

    private void Update() {
        HandleMoveInputs();
    }

    private void OnTest(InputAction.CallbackContext ctx) {
        Test?.Invoke();
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
