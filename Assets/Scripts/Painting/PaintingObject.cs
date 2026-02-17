using UnityEngine;

/// <summary>
/// Represents a painting object in the world that displays a render texture.
/// Handles the material and render texture assignment.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class PaintingObject : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RenderTexture paintingRenderTexture;
    [SerializeField] private PaintingCameraController associatedCamera;
    
    private MeshRenderer _meshRenderer;
    private Material _paintingMaterial;

    private void OnEnable() {
        GameStateManager.OnEnteredPainting += OnEnteredPainting;
        PlayerMovementController.OnFinishedExitingPainting += OnExitedPainting;
    }

    private void OnDisable() {
        GameStateManager.OnEnteredPainting -= OnEnteredPainting;
        PlayerMovementController.OnFinishedExitingPainting -= OnExitedPainting;
    }

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
        SetupMaterial();
    }

    /// <summary>
    /// Creates and assigns a material with the render texture to the painting
    /// </summary>
    private void SetupMaterial() {
        _paintingMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit")) {
            mainTexture = paintingRenderTexture
        };
        
        _meshRenderer.material = _paintingMaterial;
    }
    
    /// <summary>
    /// Activates the painting for 2D gameplay mode - starts updating the render texture
    /// </summary>
    private void EnterPaintingMode() {
        if (associatedCamera == null)
            return;
        associatedCamera.StartRendering();
    }

    /// <summary>
    /// Deactivates the painting when exiting 2D gameplay mode - freezes the render texture
    /// </summary>
    private void ExitPaintingMode() {
        if (associatedCamera == null) 
            return;
        
        associatedCamera.RenderOnce();
        associatedCamera.StopRendering();
    }

    private void OnEnteredPainting(PaintingArea _) {
        EnterPaintingMode();
    }

    private void OnExitedPainting() {
        ExitPaintingMode();
    }

    private void OnDestroy() {
        if (_paintingMaterial)
            Destroy(_paintingMaterial);
    }
}
