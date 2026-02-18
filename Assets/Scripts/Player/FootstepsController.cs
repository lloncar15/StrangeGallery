using UnityEngine;

public class FootstepsController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private FootstepsConfig config;
    [SerializeField] private CharacterController characterController;

    private float _stepTimer;
    private int _footstepCount;
    
    private const float MOVEMENT_THRESHOLD = 0.01f;

    private void Start() {
        _footstepCount = config.footstepsSounds.Length;
    }

    private void Update() {
        HandleFootsteps();
    }
    
    /// <summary>
    /// Checks wether it should play footsteps depending on the CharacterController movement and step timer
    /// </summary>
    private void HandleFootsteps() {
        bool isMoving = characterController.velocity.magnitude > MOVEMENT_THRESHOLD;
        if (!isMoving) {
            _stepTimer = 0f;
            return;
        }
        
        _stepTimer += Time.deltaTime;
        if (_stepTimer >= config.stepInterval) {
            _stepTimer = 0f;
            PlayFootStep();
        }
    }

    /// <summary>
    /// Plays footstep sounds in different pitches
    /// </summary>
    private void PlayFootStep() {
        footstepsSource.pitch = Random.Range(config.pitchRange.x, config.pitchRange.y);
        int footstepIndex = Random.Range(0, _footstepCount);
        SoundController.Instance.PlaySound(footstepsSource, config.footstepsSounds[footstepIndex]);
    }
}