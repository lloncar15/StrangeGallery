using System;
using UnityEngine;

namespace Core {
    public class TimerManager : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private int mainGameTimeInSeconds = 180;
        [SerializeField] private int endGameTimeInSeconds = 20;
        
        public static CountdownTimer Timer;

        private void OnEnable() {
            GameStateManager.MainGameStarted += OnMainGameStarted;
            GameStateManager.EndGameStarted += OnEndGameStarted;
        }

        private void OnDisable() {
            GameStateManager.MainGameStarted -= OnMainGameStarted;
            GameStateManager.EndGameStarted -= OnEndGameStarted;
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

        private void OnMainGameStarted() {
            StartTimer();
        }

        private void OnEndGameStarted() {
            Timer.Reset(endGameTimeInSeconds);
            StartTimer();
        }
    }
}