using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public abstract class ItemUIView : MonoBehaviour {
        [SerializeField] protected Image image;
        public ItemData Data { get; private set; }

        private const float OVERSHOOT = 4f;
        private const float SCALE_DURATION = 0.2f;
        
        private RectTransform _rectTransform;

        public void Initialize(Item item) {
            OnInitialize(item);
            AnimateAppearance();
            UpdateQuantity(item.quantity);
        }

        private void AnimateAppearance() {
            _rectTransform.localScale = Vector3.zero;

            _rectTransform.DOScale(Vector3.one, SCALE_DURATION)
                .SetEase(Ease.OutBack, OVERSHOOT);
        }

        public void AnimateDisappearance() {
            _rectTransform.DOScale(Vector3.zero, SCALE_DURATION)
                .SetEase(Ease.InBack, OVERSHOOT)
                .OnComplete(() => {
                    Destroy(gameObject);
                });
        }

        public virtual void UpdateQuantity(int quantity) {}

        protected virtual void OnInitialize(Item item) {
            Data = item.data;
            image.sprite = item.data?.sprite;
            image.rectTransform.localScale *= Data.iconScaleFactor;
            
            _rectTransform  = GetComponent<RectTransform>();
        }
    }
}