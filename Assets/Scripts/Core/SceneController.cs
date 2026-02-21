using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene transitions with a circular iris wipe effect.
/// </summary>
public class SceneController : PersistentSingleton<SceneController> {
    [Header("Transition References")]
    [Tooltip("Canvas for the transition overlay. Will be set to highest sort order.")]
    [SerializeField] private Canvas transitionCanvas;
    
    [Tooltip("The mask RectTransform with a circular Image + Mask component.")]
    [SerializeField] private RectTransform irisMask;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 0.4f;
    [SerializeField] private Ease closeEase = Ease.InQuad;
    [SerializeField] private Ease openEase = Ease.OutQuad;
    
    [Tooltip("Scale of the iris when fully open (should cover the full screen).")]
    [SerializeField] private float openScale = 1.5f;

    private bool _isTransitioning;

    public static readonly string MainMenuScene = "MainMenu";
    public static readonly string GameScene = "Game";

    protected override void Awake() {
        base.Awake();

        if (transitionCanvas)
            transitionCanvas.sortingOrder = 999;

        // Start fully open (invisible — circle mask is large, so black bg is outside view)
        if (irisMask) {
            irisMask.localScale = Vector3.one * openScale;
            irisMask.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Loads a scene by name with the iris wipe transition.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    /// <param name="onComplete">Optional callback invoked after the new scene is revealed.</param>
    public void LoadScene(string sceneName, Action onComplete = null) {
        if (_isTransitioning)
            return;

        StartCoroutine(TransitionCoroutine(sceneName, onComplete));
    }

    /// <summary>
    /// Shortcut to load the main game scene.
    /// </summary>
    public void LoadGameScene() => LoadScene(GameScene);

    /// <summary>
    /// Shortcut to load the main menu scene.
    /// </summary>
    public void LoadMainMenuScene() => LoadScene(MainMenuScene);

    /// <summary>
    /// Performs: iris close (mask shrinks to 0) → async scene load → iris open (mask grows back).
    /// Uses SetUpdate(true) on tweens so transitions work even when timeScale is 0.
    /// </summary>
    private IEnumerator TransitionCoroutine(string sceneName, Action onComplete) {
        _isTransitioning = true;

        irisMask.gameObject.SetActive(true);
        irisMask.localScale = Vector3.one * openScale;
        
        Tween closeTween = irisMask
            .DOScale(Vector3.zero, transitionDuration)
            .SetEase(closeEase)
            .SetUpdate(true);

        yield return closeTween.WaitForCompletion();
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (asyncLoad != null && !asyncLoad.isDone)
            yield return null;

        Tween openTween = irisMask
            .DOScale(Vector3.one * openScale, transitionDuration)
            .SetEase(openEase)
            .SetUpdate(true);

        yield return openTween.WaitForCompletion();

        irisMask.gameObject.SetActive(false);
        _isTransitioning = false;
        onComplete?.Invoke();
    }
}