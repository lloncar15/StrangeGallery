using System;
using UnityEngine;

/// <summary>
/// Manages game state transitions including FPS, Painting, and Paused states.
/// Subscribes to InputController events to handle state changes.
/// </summary>
public class GameStateManager : PersistentSingleton<GameStateManager> {
    [Header("State")]
    [SerializeField] private GameState currentState = GameState.FPS;
    
    [Header("References")]
    [SerializeField] private PlayerMovementController playerMovementController;
    
    public static event Action<GameState> OnStateChange;
    public static event Action<PlayablePaintingArea> OnEnteredPainting;
    public static event Action OnExitedPainting;
    
    private GameState _stateBeforePause;
    
    public static GameState GetCurrentState() => Instance.currentState;

    private void OnEnable() {
        InputController.Test += OnTest;
        InputController.OnExitPressed += ExitPainting;
        InputController.OnPausePressed += TogglePause;
    }

    private void OnDisable() {
        InputController.Test -= OnTest;
        InputController.OnExitPressed -= ExitPainting;
        InputController.OnPausePressed -= TogglePause;
    }

    private void OnTest() {
    }

    public void EnterPainting(PaintingObject obj) {
        ChangeState(GameState.Painting);

        PaintingCameraConfig cameraConfig = obj.CameraConfig;
        
        PlayerCameraController cameraController = PlayerCameraController.Instance;
        cameraController.ZoomIntoPainting(obj.transform.position, cameraConfig);
            
        playerMovementController.MoveTo(cameraConfig.lookingPosition,
            cameraController.config.zoomInDuration,
            cameraController.config.zoomInEase,
            obj.PaintingArea);
        
        OnEnteredPainting?.Invoke(obj.PaintingArea);
    }
    
    private void ExitPainting() {
        if (currentState != GameState.Painting)
            return;
        
        playerMovementController.ExitPainting();
        PlayerCameraController.Instance.ZoomOut(() => {
            ChangeState(GameState.FPS);
            OnExitedPainting?.Invoke();
        });
    }

    /// <summary>
    /// Toggles pause on/off. Stores the pre-pause state to restore on unpause.
    /// Sets Time.timeScale to 0 when paused, 1 when unpaused.
    /// </summary>
    public void TogglePause() {
        if (currentState == GameState.Paused) {
            Time.timeScale = 1f;
            InputController.DisableCursor();
            ChangeState(_stateBeforePause);
        }
        else {
            _stateBeforePause = currentState;
            Time.timeScale = 0f;
            InputController.EnableCursor();
            ChangeState(GameState.Paused);
        }
    }

    /// <summary>
    /// Forces unpause without toggling. Used when transitioning scenes from pause menu.
    /// </summary>
    public void ForceUnpause() {
        if (currentState != GameState.Paused)
            return;
        
        Time.timeScale = 1f;
        ChangeState(_stateBeforePause);
    }

    private void ChangeState(GameState state) {
        currentState = state;
        OnStateChange?.Invoke(currentState);
    }
}

public enum GameState {
    FPS,
    Painting,
    Paused
}