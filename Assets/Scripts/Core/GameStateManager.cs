
using System;
using UnityEngine;

public class GameStateManager : PersistentSingleton<GameStateManager> {
    [Header("State")]
    [SerializeField] private GameState currentState = GameState.FPS;
    
    public static event Action<GameState> OnStateChange;
    public static event Action<PaintingArea> OnEnteredPainting;
    public static event Action OnExitedPainting;
    
    public static GameState GetCurrentState() => Instance.currentState;
}

public enum GameState {
    FPS,
    Painting
}