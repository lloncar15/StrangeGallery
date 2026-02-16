using System;
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
    public void EnterPaintingMode() {
        if (associatedCamera != null) {
            associatedCamera.StartRendering();
        }
    }

    /// <summary>
    /// Deactivates the painting when exiting 2D gameplay mode - freezes the render texture
    /// </summary>
    public void ExitPaintingMode() {
        if (associatedCamera != null) {
            associatedCamera.StopRendering();
        }
    }

    private void OnDestroy() {
        if (_paintingMaterial)
            Destroy(_paintingMaterial);
    }
}
