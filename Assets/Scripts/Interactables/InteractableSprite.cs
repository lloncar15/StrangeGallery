using Core;
using DG.Tweening;
using Painting;
using Player;
using UnityEngine;

namespace Interactables {
    public abstract class InteractableSprite : MonoBehaviour, IInteractable {
        private bool _isInRange;

        private const float SHAKE_SCALE_STRENGTH = 0.3f;
        private const float SHAKE_ROTATION_STRENGTH = 10f;
        private const float SHAKE_DURATION = 0.2f;
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player"))
                return;
        
            _isInRange = true;
        
            if (!CanBeInteracted())
                return;

            ShakeOnPlayerEnter();
        
            PlayerSprite sprite = other.GetComponent<PlayerSprite>();
            sprite.OnInteractionZoneEnter(this);
        }
    
        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player"))
                return;

            _isInRange = false;
            
            PlayerSprite sprite = other.GetComponent<PlayerSprite>();
            sprite.OnInteractionZoneExit();
        }

        private void ShakeOnPlayerEnter() {
            transform.DOShakeScale(SHAKE_DURATION, SHAKE_SCALE_STRENGTH * transform.localScale.x);
            transform.DOShakeRotation(SHAKE_DURATION, SHAKE_ROTATION_STRENGTH);
        }

        public abstract void Interact();
    
        /// <summary>
        /// Only interactable in Painting (2D) mode.
        /// </summary>
        /// <returns>True if in Painting state and within trigger range</returns>
        public virtual bool CanBeInteracted() {
            return _isInRange && GameStateManager.GetCurrentState() == GameState.Painting;
        }

    }
}