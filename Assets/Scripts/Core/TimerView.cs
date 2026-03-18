using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core {
    public class TimerView : MonoBehaviour {
        [Header("References")]
        [SerializeField] private GameObject timerNode;
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Timer Settings")]
        [SerializeField] private int hurryUpSeconds;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hurryUpColor;
        
        [Header("Animation Settings")]
        [SerializeField] private float punchScale = 0.2f;
        [SerializeField] private float punchDuration = 0.2f;

        private int _lastSecond = -1;

        private void OnEnable() {
            CountdownTimer.OnTimerStart += Show;
            CountdownTimer.OnTimerEnd += Hide;
        }

        private void OnDisable() {
            CountdownTimer.OnTimerStart -= Show;
            CountdownTimer.OnTimerEnd -= Hide;
        }

        private void Update() {
            if (!TimerManager.Timer.IsRunning)
                return;

            int currentSecond = Mathf.CeilToInt(TimerManager.Timer.CurrentTime);
            if (currentSecond == _lastSecond)
                return;
            
            _lastSecond = currentSecond;
            UpdateText(currentSecond);
            PunchText();
        }
        
        /// <summary>
        /// Formats seconds into m:ss and updates the timer text.
        /// </summary>
        /// <param name="totalSeconds">Total remaining seconds.</param>
        private void UpdateText(int totalSeconds) {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{minutes}:{seconds:00}";
            timerText.color = totalSeconds <= hurryUpSeconds ? hurryUpColor : defaultColor;
        }

        private void Show() {
            timerNode.SetActive(true);
            timerNode.transform.localScale = Vector3.zero;
            timerNode.transform.DOScale(Vector3.one, punchDuration)
                .SetEase(Ease.OutBack);
        }

        private void Hide() {
            timerNode.transform.DOScale(Vector3.zero, punchDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => timerNode.SetActive(false));
        }

        private void PunchText() {
            timerText.transform.DOKill();
            timerText.transform.localScale = Vector3.one;
            timerText.transform.DOPunchScale(Vector3.one * punchScale, punchDuration, 0, 0);
        }
    }
}