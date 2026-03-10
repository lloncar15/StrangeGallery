using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public class ImageItemUIView : ItemUIView {
        [SerializeField] private Image imageSprite;

        protected override void OnInitialize(Item item) {
            if (item.data is not ImageData imageData)
                return;
            
            imageSprite.sprite = imageData.sprite;
        }

        protected override void OnQuantityUpdated(int quantity) {
            // no-op
        }
    }
}