using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour {
    [Header("References")] [SerializeField]
    private Transform cameraHolder;
    [SerializeField] private PlayerConfig playerConfig;

    [Header("2D Movement")]
    [SerializeField] private PlayerSprite playerSprite;

    private CharacterController _characterController;
    private float _cameraPitch;
    private float _verticalVelocity;

    private PlayablePaintingArea _currentPlayablePaintingArea;
    public static event Action OnFinishedExitingPainting;

    private void OnEnable() {
        GameStateManager.OnEnteredPainting += OnEnteredPainting;
        GameStateManager.OnExitedPainting += OnExitedPainting;
    }

    private void OnDisable() {
        GameStateManager.OnEnteredPainting -= OnEnteredPainting;
        GameStateManager.OnExitedPainting += OnExitedPainting;
    }

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        Vector2 moveInput = InputController.Instance.MoveInput;

        GameState currentState = GameStateManager.GetCurrentState();
        if (currentState == GameState.FPS) {
            HandleLook();
            Handle3DMovement(moveInput);
        }
        else if (currentState == GameState.Painting) {
            Handle2DMovement(moveInput);
        }
    }

    private void HandleLook() {
        Vector2 lookInput = InputController.Instance.LookInput;

        transform.Rotate(Vector3.up, lookInput.x * playerConfig.lookSensitivity);

        _cameraPitch -= lookInput.y * playerConfig.lookSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -playerConfig.maxLookAngle, playerConfig.maxLookAngle);
        cameraHolder.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
    }

    private void Handle3DMovement(Vector2 moveInput) {
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (_characterController.isGrounded && _verticalVelocity < 0f) {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += playerConfig.gravity * Time.deltaTime;
        direction.y = _verticalVelocity;

        _characterController.Move(direction * (playerConfig.moveSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Handles 2D movement on the XY plane with bounds checking
    /// </summary>
    /// <param name="moveInput">Move input vector from the InputController</param>
    private void Handle2DMovement(Vector2 moveInput) {
        if (!_currentPlayablePaintingArea)
            return;
        
        Transform paintingPlayerTransform = playerSprite.transform;
        
        Vector3 movement = moveInput * (playerConfig.moveSpeed2D  * Time.deltaTime);
        Vector3 newPosition = paintingPlayerTransform.position + movement;
        
        paintingPlayerTransform.position = _currentPlayablePaintingArea.ClampToBounds(newPosition, playerSprite.FootOffset);
    }

    //TODO: this flow should probably be in GameStateManager
    private void OnEnteredPainting(PlayablePaintingArea playablePaintingArea) {
        _currentPlayablePaintingArea = playablePaintingArea;
        playerSprite.SetInitialPlayerPositionInPainting(playablePaintingArea.SpawnPosition);
    }
    
    //TODO: this flow should probably be in GameStateManager
    private void OnExitedPainting() {
        _currentPlayablePaintingArea = null;
        playerSprite.OnExitedPainting();
        OnFinishedExitingPainting?.Invoke();
    }
}