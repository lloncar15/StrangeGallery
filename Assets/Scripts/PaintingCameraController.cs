using UnityEngine;

/// <summary>
/// Controls a camera that renders to a render texture for a painting.
/// During 3D gameplay, the camera doesn't update the render texture (frozen).
/// During 2D gameplay, the camera updates normally every frame.
/// </summary>
public class PaintingCameraController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera renderCamera;
    
    private bool _isActive;

    private void Start() {
        RenderOnce();
        // _isActive = false;
        // DisableCamera();
    }

    private void LateUpdate() {
        if (!_isActive)
            return;
        
        renderCamera?.Render();
    }

    /// <summary>
    /// Renders a single frame to the render texture
    /// </summary>
    private void RenderOnce() {
        renderCamera?.Render();
    }

    /// <summary>
    /// Starts continuous rendering to the render texture (2D gameplay mode)
    /// </summary>
    public void StartRendering() {
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
