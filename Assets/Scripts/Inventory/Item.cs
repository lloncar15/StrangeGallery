using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "Item", menuName = "GimGim/Inventory/Item")]
    public class Item : ScriptableObject {
        public string itemName;
        public string description;
        
        [Header("Sprite")]
        public Sprite sprite;
        [Tooltip("The scaling factor applied to this item on the painting prefab.")]
        public float paintingScaleFactor;
        [Tooltip("The scaling factor applied to this item in the inventory UI.")]
        public float iconScaleFactor;
    }
}