using System;
using UnityEngine;

namespace Core {
    public class CountdownTimer {
        public float CurrentTime { get; private set; }
        public bool IsRunning { get;  private set; }

        private float _initialTime;

        public static event Action OnTimerStart;
        public static event Action OnTimerEnd;

        public CountdownTimer(float time) {
            _initialTime = time;
        }

        public void Start() {
            CurrentTime = _initialTime;
            if (IsRunning) 
                return;
            
            IsRunning = true;
            OnTimerStart?.Invoke();
        }

        public void Stop() {
            if (!IsRunning) 
                return;
            
            IsRunning = false;
            OnTimerEnd?.Invoke();
        }

        public void Tick() {
            if (IsRunning && CurrentTime > 0) {
                CurrentTime -= Time.deltaTime;
            }

            if (IsRunning && CurrentTime <= 0) {
                Stop();
            }
        }
        
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        public void Reset() => CurrentTime = _initialTime;

        public void Reset(float newTime) {
            _initialTime = newTime;
            Reset();
        }
        
        public bool IsFinished => CurrentTime <= 0;
        
        public float Progress => Mathf.Clamp(CurrentTime / _initialTime, 0, 1);
    }
}