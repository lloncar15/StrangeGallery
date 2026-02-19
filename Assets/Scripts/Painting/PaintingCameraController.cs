using UnityEngine;

/// <summary>
/// Controls the camera that renders to a render texture for the painting.
/// During 3D gameplay, the camera doesn't update the render texture (frozen).
/// During 2D gameplay, the camera updates normally every frame.
/// </summary>
public class PaintingCameraController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera renderCamera;
    
    [Header("Camera Settings")]
    [SerializeField] public Rect viewportRect;
    
    private bool _isActive;

    private void Start() {
        renderCamera.rect = viewportRect;
        RenderOnce();
        _isActive = false;
        DisableCamera();
    }

    private void LateUpdate() {
        if (!_isActive)
            return;
        
        renderCamera?.Render();
    }

    /// <summary>
    /// Renders a single frame to the render texture
    /// </summary>
    public void RenderOnce() {
        renderCamera?.Render();
    }

    /// <summary>
    /// Starts continuous rendering to the render texture (2D gameplay mode)
    /// </summary>
    public void StartRendering() {
        renderCamera.rect = viewportRect;
        _isActive = true;
        EnableCamera();
    }

    /// <summary>
    /// Stops rendering to the render texture (3D gameplay mode - freezes the image)
    /// </summary>
    public void StopRendering() {
        _isActive = false;
        DisableCamera();
    }

    private void EnableCamera() {
        renderCamera.enabled = true;
    }

    private void DisableCamera() {
        renderCamera.enabled = false;
    }
}
