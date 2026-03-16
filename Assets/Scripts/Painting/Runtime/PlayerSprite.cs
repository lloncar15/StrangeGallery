using Core;
using DG.Tweening;
using Interactables;
using Player;
using UnityEngine;

namespace Painting {
    /// <summary>
    /// Controls the player's 2D sprite representation inside paintings.
    /// Subscribes to painting enter/exit events and manages its own visibility and positioning.
    /// </summary>
    public class PlayerSprite : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Transform footTransform;
        [SerializeField] private PlayerInteractionController controller;
        

        private float _footOffset;
        public float FootOffset => _footOffset;
        
        private const float OVERSHOOT = 4f;
        private const float SHOW_HIDE_DURATION = 0.25f;

        private void Awake() {
            _footOffset = footTransform.localPosition.y;
        }

        /// <summary>
        /// Sets the transform position directly.
        /// </summary>
        /// <param name="position">World position to set</param>
        private void SetPosition(Vector3 position) {
            transform.position = position;
        }

        /// <summary>
        /// Sets the transform position so that the foot transform aligns with the given position.
        /// </summary>
        /// <param name="position">The position the foot transform should be at</param>
        private void SetPositionAtFoot(Vector3 position) {
            SetPosition(new Vector3(position.x, position.y - _footOffset, position.z));
        }
    
        /// <summary>
        /// Positions the sprite at the painting's spawn point and makes it visible.
        /// </summary>
        /// <param name="playablePaintingArea">The painting area being entered</param>
        public void OnEnteredPainting(PlayablePaintingArea playablePaintingArea) {
            SetPositionAtFoot(playablePaintingArea.SpawnPosition);

            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            
            transform.DOScale(Vector3.one, SHOW_HIDE_DURATION)
                .SetEase(Ease.OutBack, OVERSHOOT);
        }

        /// <summary>
        /// Hides the sprite when exiting a painting.
        /// </summary>
        public void OnExitedPainting() {
            transform.DOScale(Vector3.zero, SHOW_HIDE_DURATION)
                .SetEase(Ease.InBack, OVERSHOOT)
                .OnComplete(() => {
                    gameObject.SetActive(false);
                });
        }

        public void OnInteractionZoneEnter(IInteractable interactable) {
            controller.OnInteractionZoneEnter(interactable);
        }

        public void OnInteractionZoneExit() {
            controller.OnInteractionZoneExit();
        }
    }
}
