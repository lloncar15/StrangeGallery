using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Config", menuName = "GimGim/Camera/Camera Config")]
public class CameraConfig : ScriptableObject {
    [Header("Defaults")]
    [SerializeField] public float defaultFOV = 80f;
    
    [Header("Zoom in")]
    [SerializeField] public float zoomInDuration = 0.5f;
    [SerializeField] public Ease zoomInEase = Ease.InOutQuad;
    
    [Header("Zoom out")]
    [SerializeField] public float zoomOutDuration = 0.5f;
    [SerializeField] public Ease zoomOutEase = Ease.InOutQuad;
}