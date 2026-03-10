using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "Color", menuName = "GimGim/Inventory/Color")]
    public class ColorData : ItemData {
        public override bool IsStackable => true;
        [Header("Color")] 
        public Color color;
    }
}