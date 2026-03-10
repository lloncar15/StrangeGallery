using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "Image", menuName = "GimGim/Inventory/Image")]
    public class ImageData : ItemData {
        public override bool IsStackable => false;
        [Tooltip("The scaling factor applied to this item on the painting prefab.")]
        public float paintingScaleFactor;
    }
}