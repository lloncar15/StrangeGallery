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
    [SerializeField] private PaintingCameraConfig cameraConfig;
    [SerializeField] private PlayablePaintingArea paintingArea;
    
    public PaintingCameraConfig CameraConfig => cameraConfig;
    public PlayablePaintingArea PaintingArea => paintingArea;
    
    private MeshRenderer _meshRenderer;
    private Material _paintingMaterial;

    private void OnEnable() {
        GameStateManager.OnEnteredPainting += OnEnteredPainting;
        GameStateManager.OnExitedPainting += OnExitedPainting;
    }

    private void OnDisable() {
        GameStateManager.OnEnteredPainting -= OnEnteredPainting;
        GameStateManager.OnExitedPainting -= OnExitedPainting;
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
        _meshRenderer.material.mainTextureScale = 
            new Vector2(associatedCamera.viewportRect.width, associatedCamera.viewportRect.height);
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

    private void OnEnteredPainting(PlayablePaintingArea _) {
        EnterPaintingMode();
    }

    private void OnExitedPainting() {
        ExitPaintingMode();
    }

    private void OnDestroy() {
        if (_paintingMaterial)
            Destroy(_paintingMaterial);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!cameraConfig)
            return;

        const float playerPostionY = 1f;
        // The world point from which the camera will look at the painting:
        // X and Z from lookingPosition, Y from cameraHolderPosition (the camera child offset)
        Vector3 cameraWorldPoint = new(
            cameraConfig.lookingPosition.x,
            cameraConfig.cameraHolderPositionY + playerPostionY,
            cameraConfig.lookingPosition.z);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(cameraWorldPoint, 0.1f);

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        float aspect = mainCamera.aspect;
        float drawDistance = Vector3.Distance(cameraWorldPoint, transform.position);
        
        // Draw the FOV frustum looking from the camera world point toward the painting
        Vector3 directionToPainting = (transform.position - cameraWorldPoint).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPainting);
        
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(cameraWorldPoint, lookRotation, Vector3.one);
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawFrustum(Vector3.zero, cameraConfig.zoomFOV, drawDistance, 0.01f, aspect);
        Gizmos.matrix = originalMatrix;
    }
#endif
}
