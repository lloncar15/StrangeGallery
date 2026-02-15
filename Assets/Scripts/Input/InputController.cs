using UnityEngine;

public class InputController : PersistentSingleton<InputController> {
    private PlayerInputActions _inputActions;
    
    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}

    protected override void Awake() {
        _inputActions = new PlayerInputActions();
        base.Awake();
    }
    
    private void OnEnable() {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();
    }

    private void OnDisable() {
        _inputActions?.Disable();
    }

    private void Update() {
        HandleMoveInputs();
    }

    private void HandleMoveInputs() {
        MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = _inputActions.Player.Look.ReadValue<Vector2>();
    }
}
