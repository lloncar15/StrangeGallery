using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public abstract class ItemUIView : MonoBehaviour {
        [SerializeField] protected Image image;
        public ItemData Data { get; private set; }

        public void Initialize(Item item) {
            OnInitialize(item);
            UpdateQuantity(item.quantity);
        }

        public virtual void UpdateQuantity(int quantity) {}

        protected virtual void OnInitialize(Item item) {
            Data = item.data;
            image.sprite = item.data?.sprite;
            image.rectTransform.localScale *= Data.iconScaleFactor;
        }
    }
}