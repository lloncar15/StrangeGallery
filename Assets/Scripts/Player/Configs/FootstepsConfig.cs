using UnityEngine;

[CreateAssetMenu(fileName = "Footsteps Config", menuName = "GimGim/Player/Footsteps Config")]
public class FootstepsConfig : ScriptableObject {
    [Header("Audio clips")]
    [SerializeField] public AudioClip[] footstepsSounds;
    
    [Header("Sound settings")]
    [SerializeField][Tooltip("The interval at which a footstep sound is played.")] 
    public float stepInterval = 0.4f;
    [SerializeField] public Vector2 pitchRange = new(0.9f, 1.1f);
    [SerializeField] public float volume = 1.1f;
}