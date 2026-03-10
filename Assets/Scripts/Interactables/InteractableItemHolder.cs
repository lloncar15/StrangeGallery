using DG.Tweening;
using Inventory;
using UnityEngine;

namespace Interactables {
    public class InteractableItemHolder : InteractableSprite {
        [SerializeField] private ItemData item;
        
        private bool _hasBeenInteracted;
        
        private const float OVERSHOOT = 4f;
        private const float SCALE_DURATION = 0.2f;
        
        public override void Interact() {
            _hasBeenInteracted = true;
            InventoryController.Instance.AddItem(item);
            OnInteracted();
        }

        public override bool CanBeInteracted() {
            return base.CanBeInteracted() && !_hasBeenInteracted;
        }

        private void OnInteracted() {
            transform.DOScale(Vector3.zero, SCALE_DURATION)
                .SetEase(Ease.InBack, OVERSHOOT)
                .OnComplete(() => {
                    Destroy(gameObject);
                });
        }
    }
}