using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : PersistentSingleton<GameStateManager> {
    [Header("State")]
    [SerializeField] private GameState currentState = GameState.FPS;
    public PlayablePaintingArea playablePaintingAreaTest;
    public PaintingObject paintingObject;
    
    public static event Action<GameState> OnStateChange;
    public static event Action<PlayablePaintingArea> OnEnteredPainting;
    public static event Action OnExitedPainting;
    
    public static GameState GetCurrentState() => Instance.currentState;

    private void OnEnable() {
        InputController.Test += OnTest;
    }

    private void OnDisable() {
        InputController.Test -= OnTest;
    }

    public void OnTest() {
        if (currentState == GameState.FPS) {
            ChangeState(GameState.Painting);

            PaintingCameraConfig cameraConfig = paintingObject.cameraConfig;
            PlayerCameraController controller = PlayerCameraController.Instance;
            controller.ZoomIntoPainting(paintingObject.transform.position, cameraConfig);
            
            PlayerMovementController.Instance.MoveTo(cameraConfig.lookingPosition, controller.config.zoomInDuration, controller.config.zoomInEase, playablePaintingAreaTest);
            OnEnteredPainting?.Invoke(playablePaintingAreaTest);
        }
        else {
            PlayerMovementController.Instance.ExitPainting();
            PlayerCameraController.Instance.ZoomOut(() => {
                ChangeState(GameState.FPS);
                OnExitedPainting?.Invoke();
            });
        }
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