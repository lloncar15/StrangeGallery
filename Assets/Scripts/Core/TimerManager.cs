using System;
using UnityEngine;

namespace Core {
    public class TimerManager : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private int mainGameTimeInSeconds = 180;
        [SerializeField] private int endGameTimeInSeconds = 20;
        
        public static CountdownTimer Timer;

        private void OnEnable() {
            GameStateManager.StateChanged += OnStateChanged;
        }

        private void OnDisable() {
            GameStateManager.StateChanged -= OnStateChanged;
        }

        private void Awake() {
            Timer = new CountdownTimer(mainGameTimeInSeconds);
        }

        private void Update() {
            Timer.Tick();
        }

        private void StartTimer() {
            Timer.Start();
        }

        private void OnStateChanged(GameState previous, GameState current) {
            if (current.HasFlag(GameState.MainGame) && !previous.HasFlag(GameState.MainGame)) {
                StartTimer();
            }
            else if (current.HasFlag(GameState.EndGame) && !previous.HasFlag(GameState.EndGame)) {
                Timer.Reset(endGameTimeInSeconds);
                StartTimer();
            }
        }
    }
}