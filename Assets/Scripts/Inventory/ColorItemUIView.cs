using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public class ColorItemUIView : ItemUIView {
        [SerializeField] private Image colorSprite;
        [SerializeField] private TextMeshProUGUI quantityLabel;

        protected override void OnInitialize(Item item) {
            if (item.data is not ColorData colorData) 
                return;
            
            colorSprite.sprite = colorData.sprite;
            colorSprite.color = colorData.color;
        }

        protected override void OnQuantityUpdated(int quantity) {
            quantityLabel.text = $"x{quantity}";
        }
    }
}