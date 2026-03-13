using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "Image", menuName = "GimGim/Inventory/Image")]
    public class ImageData : ItemData {
        public override bool IsStackable => false;
    }
}