using System;
using Input;
using UnityEngine;
using Utils;

namespace Core {
    public class GameStateManager : PersistentSingleton<GameStateManager> {
        [Header("State")]
        [SerializeField] private GameState currentState = GameState.FPS;
        
        public static event Action<GameState, GameState> StateChanged;
    
        /// <summary>
        /// Returns the current state flags.
        /// </summary>
        public static GameState GetCurrentState() => Instance.currentState;
        
        /// <summary>
        /// Checks if the current state contains the given flag.
        /// </summary>
        /// <param name="state">The flag to check for.</param>
        /// <returns>True if the flag is present in the current state.</returns>
        public static bool IsInState(GameState state) => (Instance.currentState & state) == state;

        private void OnEnable() {
            InputController.Test += OnTest;
        }

        private void OnDisable() {
            InputController.Test -= OnTest;
        }

        private void OnTest() {
        }

        /// <summary>
        /// Replaces the current state entirely and fires OnStateChange.
        /// </summary>
        /// <param name="state">The new state to set.</param>
        private void ChangeStateInternal(GameState state) {
            GameState previous = currentState;
            currentState = state;
            StateChanged?.Invoke(previous, currentState);
        }

        /// <summary>
        /// Additively sets a state flag without replacing existing flags.
        /// Does not fire OnStateChange.
        /// </summary>
        /// <param name="state">The flag to add.</param>
        private void AddStateInternal(GameState state) {
            GameState previous = currentState;
            currentState |= state;
            StateChanged?.Invoke(previous, currentState);
        }

        /// <summary>
        /// Removes a state flag without affecting other flags.
        /// Does not fire OnStateChange.
        /// </summary>
        /// <param name="state">The flag to remove.</param>
        private void RemoveStateInternal(GameState state) {
            GameState previous = currentState;
            currentState &= ~state;
            StateChanged?.Invoke(previous, currentState);
        }
        
        public static void ChangeState(GameState state) => Instance.ChangeStateInternal(state);
        public static void AddState(GameState state) => Instance.AddStateInternal(state);
        public static void RemoveState(GameState state) => Instance.RemoveStateInternal(state);
    }

    [Flags]
    public enum GameState {
        Paused = 1,
        MainGame = 2,
        EndGame = 4,
        FPS = 8,
        Painting = 16
    }
}