using UnityEngine;

/// <summary>
/// Holds the data for the painting camera movement positioning
/// </summary>
[CreateAssetMenu(fileName = "Painting Camera Config", menuName = "GimGim/Camera/Panting Camera Config")]
public class PaintingCameraConfig : ScriptableObject {
    [Header("Player Movement")]
    [Tooltip("World position the player object will move to. Y is ignored.")]
    public Vector3 lookingPosition;

    [Header("Camera Settings")]
    [Tooltip("Y position of the CameraHolder child object, controlling the height from which the camera looks.")]
    public float cameraHolderPositionY;
    [Tooltip("Field of view when zoomed into the painting.")]
    public float zoomFOV;
}