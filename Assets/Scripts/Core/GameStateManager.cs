using System;
using Camera;
using Camera.Configs;
using Conversations;
using Input;
using Painting;
using Player;
using UnityEngine;
using Utils;

namespace Core {
    public class GameStateManager : PersistentSingleton<GameStateManager> {
        [Header("State")]
        [SerializeField] private GameState currentState = GameState.FPS;
        
        public static event Action<GameState> OnStateChange;
    
        public static GameState GetCurrentState() => Instance.currentState;

        private void OnEnable() {
            InputController.Test += OnTest;
            YarnConversationController.DialogueStarted += OnDialogueStarted;
            YarnConversationController.DialogueEnded += OnDialogueEnded;
        }

        private void OnDisable() {
            InputController.Test -= OnTest;
            YarnConversationController.DialogueStarted -= OnDialogueStarted;
            YarnConversationController.DialogueEnded -= OnDialogueEnded;
        }

        private void OnTest() {
        }

        public void ChangeState(GameState state) {
            currentState = state;
        
            OnStateChange?.Invoke(currentState);
        }

        private void OnDialogueStarted(YarnDialogueProvider _) {
            ChangeState(GameState.Conversation);
        }

        private void OnDialogueEnded(YarnDialogueProvider _) {
            ChangeState(GameState.Painting);
        }
    }

    public enum GameState {
        FPS,
        Painting,
        Conversation
    }
}