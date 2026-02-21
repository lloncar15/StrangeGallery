using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the main menu UI with animated transitions between panels.
/// Panels slide in/out horizontally. Main panel starts visible, others start offscreen right.
/// </summary>
public class MainMenuView : MonoBehaviour {
    [Header("Panels")]
    [SerializeField] private RectTransform mainPanel;
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private RectTransform creditsPanel;

    [Header("Main Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Back Buttons")]
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button creditsBackButton;

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.35f;
    [SerializeField] private Ease slideInEase = Ease.OutQuad;
    [SerializeField] private Ease slideOutEase = Ease.InQuad;

    private float _offscreenX;
    private RectTransform _currentPanel;
    
    private void Awake() {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas) {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            _offscreenX = canvasRect.rect.width;
        }
        else {
            _offscreenX = 1920f;
        }
    }

    private void Start() {
        mainPanel.anchoredPosition = Vector2.zero;
        settingsPanel.anchoredPosition = new Vector2(_offscreenX, 0);
        creditsPanel.anchoredPosition = new Vector2(_offscreenX, 0);

        settingsPanel.gameObject.SetActive(true);
        creditsPanel.gameObject.SetActive(true);

        _currentPanel = mainPanel;
        
        InputController.EnableCursor();
    }

    private void OnEnable() {
        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        settingsBackButton.onClick.AddListener(OnSettingsBackClicked);
        creditsBackButton.onClick.AddListener(OnCreditsBackClicked);
    }

    private void OnDisable() {
        playButton.onClick.RemoveListener(OnPlayClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
        creditsButton.onClick.RemoveListener(OnCreditsClicked);
        exitButton.onClick.RemoveListener(OnExitClicked);
        settingsBackButton.onClick.RemoveListener(OnSettingsBackClicked);
        creditsBackButton.onClick.RemoveListener(OnCreditsBackClicked);
    }

    /// <summary>
    /// Starts the game by loading the game scene via SceneController.
    /// </summary>
    private void OnPlayClicked() {
        SceneController.Instance.LoadGameScene();
    }

    private void OnSettingsClicked() => TransitionTo(settingsPanel);
    private void OnCreditsClicked() => TransitionTo(creditsPanel);
    private void OnSettingsBackClicked() => TransitionTo(mainPanel);
    private void OnCreditsBackClicked() => TransitionTo(mainPanel);

    /// <summary>
    /// Exits the application. Stops play mode if in the Unity editor.
    /// </summary>
    private void OnExitClicked() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Slides the current panel offscreen left and the target panel in from the right.
    /// If going back to main, the current slides out right and main slides in from left.
    /// </summary>
    /// <param name="target">The panel RectTransform to transition to.</param>
    private void TransitionTo(RectTransform target) {
        if (_currentPanel == target)
            return;

        RectTransform from = _currentPanel;
        _currentPanel = target;
        
        bool goingToMain = target == mainPanel;
        float fromTargetX = goingToMain ? _offscreenX : -_offscreenX;
        float toStartX = goingToMain ? -_offscreenX : _offscreenX;
        
        target.anchoredPosition = new Vector2(toStartX, 0);
        
        from.DOAnchorPosX(fromTargetX, transitionDuration)
            .SetEase(slideOutEase)
            .SetUpdate(true);
        
        target.DOAnchorPosX(0, transitionDuration)
            .SetEase(slideInEase)
            .SetUpdate(true);
    }
}