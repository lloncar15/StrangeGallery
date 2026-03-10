using UnityEngine;

namespace Inventory {
    public abstract class ItemUIView : MonoBehaviour {
        public ItemData Data { get; private set; }

        public void Initialize(Item item) {
            Data = item.data;
            UpdateQuantity(item.quantity);
            OnInitialize(item);
        }

        public void UpdateQuantity(int quantity) => OnQuantityUpdated(quantity);
        
        protected abstract void OnQuantityUpdated(int quantity);
        protected abstract void OnInitialize(Item item);
    }
}