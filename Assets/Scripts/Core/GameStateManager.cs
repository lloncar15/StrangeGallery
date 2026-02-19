using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : PersistentSingleton<GameStateManager> {
    [Header("State")]
    [SerializeField] private GameState currentState = GameState.FPS;
    
    public static event Action<GameState> OnStateChange;
    public static event Action<PlayablePaintingArea> OnEnteredPainting;
    public static event Action OnExitedPainting;
    
    public static GameState GetCurrentState() => Instance.currentState;

    private void OnEnable() {
        InputController.Test += OnTest;
        InputController.OnExitPressed += ExitPainting;
    }

    private void OnDisable() {
        InputController.Test -= OnTest;
        InputController.OnExitPressed -= ExitPainting;
    }

    private void OnTest() {
    }

    public void EnterPainting(PaintingObject obj) {
        ChangeState(GameState.Painting);

        PaintingCameraConfig cameraConfig = obj.CameraConfig;
        
        PlayerCameraController cameraController = PlayerCameraController.Instance;
        cameraController.ZoomIntoPainting(obj.transform.position, cameraConfig);
            
        PlayerMovementController.Instance.MoveTo(cameraConfig.lookingPosition,
            cameraController.config.zoomInDuration,
            cameraController.config.zoomInEase,
            obj.PaintingArea);
        
        OnEnteredPainting?.Invoke(obj.PaintingArea);
    }
    
    private void ExitPainting() {
        if (currentState != GameState.Painting)
            return;
        
        PlayerMovementController.Instance.ExitPainting();
        PlayerCameraController.Instance.ZoomOut(() => {
            ChangeState(GameState.FPS);
            OnExitedPainting?.Invoke();
        });
    }

    private void ChangeState(GameState state) {
        currentState = state;
        
        OnStateChange?.Invoke(currentState);
    }
}

public enum GameState {
    FPS,
    Painting
}