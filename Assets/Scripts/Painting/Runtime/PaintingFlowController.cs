using System;
using Camera;
using Camera.Configs;
using Core;
using Input;
using Player;
using UnityEngine;

namespace Painting {
    public class PaintingFlowController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private PlayerMovementController playerMovementController;
        
        public static event Action<PlayablePaintingArea> OnEnteredPainting;
        public static event Action OnExitedPainting;

        private void OnEnable() {
            InputController.OnExitPressed += ExitPainting;
        }

        private void OnDisable() {
            InputController.OnExitPressed -= ExitPainting;
        }
        
        public void EnterPainting(PaintingObject obj) {
            GameStateManager.ChangeState(GameState.Painting);

            PaintingCameraConfig cameraConfig = obj.CameraConfig;
        
            PlayerCameraController cameraController = PlayerCameraController.Instance;
            cameraController.ZoomIntoPainting(obj.transform.position, cameraConfig);
            
            playerMovementController.MoveTo(cameraConfig.lookingPosition,
                cameraController.config.zoomInDuration,
                cameraController.config.zoomInEase,
                obj.PaintingArea);
        
            OnEnteredPainting?.Invoke(obj.PaintingArea);
        }
        
        public void ExitPainting() {
            if (!GameStateManager.IsInState(GameState.Painting))
                return;
        
            playerMovementController.ExitPainting();
            PlayerCameraController.Instance.ZoomOut(() => {
                GameStateManager.ChangeState(GameState.FPS);
                OnExitedPainting?.Invoke();
            });
        }
    }
}