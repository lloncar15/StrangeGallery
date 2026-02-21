using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the pause menu UI in the game scene.
/// Listens to GameStateManager state changes to show/hide.
/// Contains settings controls and a back-to-menu button.
/// </summary>
public class PauseMenuView : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RectTransform panelRoot;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button resumeButton;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;

    private void OnEnable() {
        GameStateManager.OnStateChange += OnGameStateChanged;
        backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        resumeButton.onClick.AddListener(OnResumeClicked);
    }

    private void OnDisable() {
        GameStateManager.OnStateChange -= OnGameStateChanged;
        backToMenuButton.onClick.RemoveListener(OnBackToMenuClicked);
        resumeButton.onClick.RemoveListener(OnResumeClicked);
    }

    private void Start() {
        panelRoot.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows or hides the pause panel based on the new game state.
    /// </summary>
    /// <param name="state">The new game state.</param>
    private void OnGameStateChanged(GameState state) {
        if (state == GameState.Paused)
            Show();
        else if (panelRoot.gameObject.activeSelf)
            Hide();
    }

    /// <summary>
    /// Activates the panel and plays a bouncy scale-in animation.
    /// Uses SetUpdate(true) because Time.timeScale is 0 while paused.
    /// </summary>
    private void Show() {
        panelRoot.gameObject.SetActive(true);
        panelRoot.localScale = Vector3.zero;
        panelRoot.DOScale(Vector3.one, animationDuration)
            .SetEase(showEase)
            .SetUpdate(true);
    }

    /// <summary>
    /// Plays a scale-out animation then deactivates the panel.
    /// </summary>
    private void Hide() {
        panelRoot.DOScale(Vector3.zero, animationDuration)
            .SetEase(hideEase)
            .SetUpdate(true)
            .OnComplete(() => panelRoot.gameObject.SetActive(false));
    }

    /// <summary>
    /// Unpauses, then triggers scene transition to main menu.
    /// </summary>
    private void OnBackToMenuClicked() {
        GameStateManager.Instance.ForceUnpause();
        InputController.EnableCursor();
        SceneController.Instance.LoadMainMenuScene();
    }

    /// <summary>
    /// Unpauses the game by toggling pause state back via GameStateManager.
    /// </summary>
    private void OnResumeClicked() {
        GameStateManager.Instance.TogglePause();
    }
}