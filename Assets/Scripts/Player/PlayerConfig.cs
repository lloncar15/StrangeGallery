using UnityEngine;

[CreateAssetMenu(fileName = "Player Config", menuName = "GimGim/Player/Player Config")]
public class PlayerConfig : ScriptableObject {
    [Header("3D Movement")]
    [SerializeField] public float moveSpeed = 4f;
    [SerializeField] public float gravity = -9.81f;
    
    [Header("Look")]
    [SerializeField] public float lookSensitivity = 0.15f;
    [SerializeField] public float maxLookAngle = 85f;

    [Header("2D Movement")]
    [SerializeField] public float moveSpeed2D = 3f;
}
