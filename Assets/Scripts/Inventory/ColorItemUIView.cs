using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public class ColorItemUIView : ItemUIView {
        [SerializeField] private TextMeshProUGUI quantityLabel;

        protected override void OnInitialize(Item item) {
            base.OnInitialize(item);
            
            if (item.data is not ColorData colorData) 
                return;
            
            image.color = colorData.color;
        }

        public override void UpdateQuantity(int quantity) {
            quantityLabel.text = $"x{quantity}";
        }
    }
}