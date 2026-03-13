using UnityEngine;

namespace Inventory {
    public abstract class ItemData : ScriptableObject {
        public string itemName;
        public string tooltipDescription;
        public abstract bool IsStackable { get; }
        
        [Header("Sprite")]
        public Sprite sprite;
        [Tooltip("The scaling factor applied to this item in the inventory UI.")]
        public float iconScaleFactor;
        [Tooltip("The scaling factor applied to this item on the painting prefab.")]
        public float paintingScaleFactor;
    }
}