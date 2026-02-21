using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : GenericSingleton<PlayerMovementController> {
    [Header("References")] [SerializeField]
    private Transform cameraHolder;
    [SerializeField] private PlayerConfig playerConfig;

    [Header("2D Movement")]
    [SerializeField] private PlayerSprite playerSprite;

    private CharacterController _characterController;
    private float _cameraPitch;
    private float _verticalVelocity;

    private PlayablePaintingArea _currentPlayablePaintingArea;
    
    private bool _isMovingToTarget;
    private Tween _moveTween;

    private void OnEnable() {
    }

    private void OnDisable() {
    }

    protected override void Awake() {
        base.Awake();
        
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
    /// <param name="paintingArea">The painting area we are entering.</param>
    public void MoveTo(Vector3 targetPosition, float duration, Ease ease = Ease.Linear, PlayablePaintingArea paintingArea = null) {
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
                EnterPainting(paintingArea);
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
    }

    #endregion

    #region 2D movement
    
    /// <summary>
    /// Handles 2D movement on the XY plane with bounds checking.
    /// Applies the player sprite's speed multiplier (affected by blue slow effect).
    /// </summary>
    /// <param name="moveInput">Move input vector from the InputController.</param>
    private void Handle2DMovement(Vector2 moveInput) {
        if (!_currentPlayablePaintingArea)
            return;
    
        Transform paintingPlayerTransform = playerSprite.transform;
    
        float speed = playerConfig.moveSpeed2D * playerSprite.SpeedMultiplier;
        Vector3 movement = moveInput * (speed * Time.deltaTime);
        Vector3 newPosition = paintingPlayerTransform.position + movement;
    
        paintingPlayerTransform.position = _currentPlayablePaintingArea.ClampToBounds(newPosition, playerSprite.FootOffset);
    }
    
    #endregion

    #region Painting Flow

    private void EnterPainting(PlayablePaintingArea paintingArea) {
        _currentPlayablePaintingArea = paintingArea;
    }

    public void ExitPainting() {
        _currentPlayablePaintingArea = null;
    }

    #endregion
}