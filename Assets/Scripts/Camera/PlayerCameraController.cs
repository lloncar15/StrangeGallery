using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles camera zoom effects by adjusting field of view and camera position.
/// Used for focusing on paintings.
/// </summary>
public class PlayerCameraController : GenericSingleton<PlayerCameraController> {
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] public Transform playerTransform;
    [SerializeField] public CameraConfig config;

    private Tweener _fovTween;
    private Tweener _rotationTween;
    private Tweener _holderPositionTween;
    private Tweener _playerRotationTween;
    
    private Quaternion _originalCameraHolderRotation;
    private Vector3 _originalCameraHolderPosition;
    
    protected override void Awake() {
        base.Awake();
        
        if (!playerCamera)
            playerCamera = Camera.main;

        if (playerCamera != null) 
            playerCamera.fieldOfView = config.defaultFOV;
    }

    /// <summary>
    /// Zooms in while rotating the camera holder to look at the painting object and moving it to the configured height.
    /// </summary>
    /// <param name="targetPoint">World position of the PaintingObject to look at.</param>
    /// <param name="cameraConfig">Config asset containing the target FOV and camera holder Y offset.</param>
    /// <param name="onComplete">Callback invoked when the zoom tween completes.</param>
    public void ZoomIntoPainting(Vector3 targetPoint, PaintingCameraConfig cameraConfig, Action onComplete = null) {
    KillTweens();
    
    _originalCameraHolderRotation = cameraHolder.localRotation;
    _originalCameraHolderPosition = cameraHolder.localPosition;

    _fovTween = playerCamera
        .DOFieldOfView(cameraConfig.zoomFOV, config.zoomInDuration)
        .SetEase(config.zoomInEase)
        .OnComplete(() => onComplete?.Invoke());
    
    // Use the final player position (lookingPosition X/Z) rather than current position
    // so the rotation is correct after MoveTo completes
    Vector3 finalPlayerPosition = new(
        cameraConfig.lookingPosition.x,
        cameraHolder.parent.position.y,
        cameraConfig.lookingPosition.z);

    Vector3 finalCameraHolderWorldPosition = new(
        finalPlayerPosition.x,
        finalPlayerPosition.y + cameraConfig.cameraHolderPositionY,
        finalPlayerPosition.z);
    
    Vector3 directionToPainting = targetPoint - finalCameraHolderWorldPosition;

    // Player only rotates on Y axis — flatten direction to XZ plane
    Vector3 flatDirection = new Vector3(directionToPainting.x, 0f, directionToPainting.z);
    float targetYAngle = Quaternion.LookRotation(flatDirection).eulerAngles.y;
    Quaternion targetPlayerRotation = Quaternion.Euler(0f, targetYAngle, 0f);

    // Camera holder only rotates on X axis (pitch)
    float targetXAngle = Quaternion.LookRotation(directionToPainting).eulerAngles.x;
    Quaternion targetHolderRotation = Quaternion.Euler(targetXAngle, 0f, 0f);

    _rotationTween = cameraHolder
        .DOLocalRotateQuaternion(targetHolderRotation, config.zoomInDuration)
        .SetEase(config.zoomInEase);

    _holderPositionTween = cameraHolder
        .DOLocalMoveY(cameraConfig.cameraHolderPositionY, config.zoomInDuration)
        .SetEase(config.zoomInEase);
    
    _playerRotationTween = playerTransform
        .DOLocalRotateQuaternion(targetPlayerRotation, config.zoomInDuration)
        .SetEase(config.zoomInEase);
    }

/// <summary>
/// Zooms back out to default FOV and restores original camera holder rotation and position.
/// </summary>
public void ZoomOut(Action onComplete = null) {
    KillTweens();

    _fovTween = playerCamera
        .DOFieldOfView(config.defaultFOV, config.zoomOutDuration)
        .SetEase(config.zoomOutEase)
        .OnComplete(() => {
            onComplete?.Invoke();
        });
    
    _rotationTween = cameraHolder
        .DOLocalRotateQuaternion(_originalCameraHolderRotation, config.zoomOutDuration)
        .SetEase(config.zoomOutEase);
    
    _holderPositionTween = cameraHolder
        .DOLocalMove(_originalCameraHolderPosition, config.zoomOutDuration)
        .SetEase(config.zoomOutEase);
}

    private void KillTweens() {
        _fovTween?.Kill();
        _rotationTween?.Kill();
        _holderPositionTween?.Kill();
        _playerRotationTween?.Kill();
    }
}
