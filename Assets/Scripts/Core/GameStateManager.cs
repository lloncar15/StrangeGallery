using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : PersistentSingleton<GameStateManager> {
    [Header("State")]
    [SerializeField] private GameState currentState = GameState.FPS;
    public PaintingArea paintingAreaTEST;
    
    public static event Action<GameState> OnStateChange;
    public static event Action<PaintingArea> OnEnteredPainting;
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
            OnEnteredPainting?.Invoke(paintingAreaTEST);
        }
        else {
            ChangeState(GameState.FPS);
            OnExitedPainting?.Invoke();
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