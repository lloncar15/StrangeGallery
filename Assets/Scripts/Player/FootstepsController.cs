using UnityEngine;

public class FootstepsController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private FootstepsConfig config;

    private float _stepTimer;
    private int _currentFootstepIndex;
    private int _footstepCount;
    
    private const float MOVEMENT_THRESHOLD = 0.01f;

    private void Start() {
        _footstepCount = config.footstepsSounds.Length;
    }

    private void Update() {
        HandleFootsteps();
    }
    
    private void HandleFootsteps() {
        Vector2 moveInput = InputController.Instance.MoveInput;
        
        bool isMoving = moveInput.sqrMagnitude > MOVEMENT_THRESHOLD;
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

    private void PlayFootStep() {
        footstepsSource.pitch = Random.Range(config.pitchRange.x, config.pitchRange.y);
        SoundController.Instance.PlaySound(footstepsSource, config.footstepsSounds[_currentFootstepIndex]);
        _currentFootstepIndex = (_currentFootstepIndex + 1) % _footstepCount;
    }
}