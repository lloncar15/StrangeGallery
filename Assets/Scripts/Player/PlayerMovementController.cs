using System;
using DG.Tweening;
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
    
    private bool _isMovingToTarget;
    private Tween _moveTween;

    private void OnEnable() {
        GameStateManager.OnEnteredPainting += OnEnteredPainting;
        GameStateManager.OnExitedPainting += OnExitedPainting;
        InputController.Test += OnTest;
    }

    private void OnDisable() {
        GameStateManager.OnEnteredPainting -= OnEnteredPainting;
        GameStateManager.OnExitedPainting -= OnExitedPainting;
        InputController.Test -= OnTest;
    }

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        if (_isMovingToTarget) {
            return;
        }
        
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

    #region 3D movement

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
    
    #endregion

    #region Commanded movement
    
    /// <summary>
    /// Moves the player to a target position using DOTween, bypassing player input.
    /// Movement is state-independent and blocks all player-controlled movement until complete.
    /// Y position is locked to the player's current Y at the time of the call.
    /// Any existing MoveTo tween is killed before starting a new one.
    /// </summary>
    /// <param name="targetPosition">World position to move toward. Y component is replaced with the player's current Y.</param>
    /// <param name="duration">Duration of the movement in seconds.</param>
    /// <param name="ease">DOTween ease type to apply to the movement.</param>
    /// <param name="onComplete">Callback invoked when the player reaches the target position.</param>
    private void MoveTo(Vector3 targetPosition, float duration, Ease ease = Ease.Linear, Action onComplete = null) {
        _moveTween?.Kill();
        
        Vector3 target = new(targetPosition.x, transform.position.y, targetPosition.z);
        
        _isMovingToTarget = true;
        
        _moveTween = DOTween.To(
                () => transform.position,
                MoveToPosition,
                target,
                duration)
            .SetEase(ease)
            .OnComplete(() => {
                _isMovingToTarget = false;
                _moveTween = null;
                onComplete?.Invoke();
            });
    }
    
    /// <summary>
    /// Moves the CharacterController to the given position by computing and applying
    /// the delta from the current position each tween tick.
    /// </summary>
    /// <param name="targetPosition">The next interpolated world position provided by the DOTween setter.</param>
    private void MoveToPosition(Vector3 targetPosition) {
        Vector3 delta = targetPosition - transform.position;
        _characterController.Move(delta);
        Debug.Log("Character is moving");
    }

    private void OnTest() {
        Vector3 pos = transform.position + Vector3.forward * 2;
        MoveTo(pos, 0.5f);
    }

    #endregion

    #region 2D movement
    
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
    
    #endregion

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