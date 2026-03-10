using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Utils;

namespace Inventory {
    public class InventoryController : GenericSingleton<InventoryController> {
        /// <summary>
        /// A serialized list to be shown in the inspector without the need for custom drawers.
        /// </summary>
        [SerializeField] private List<Item> serializedItems = new();
        private Dictionary<ItemData, Item> _items = new();

        public static event Action<Item> ItemAdded;
        public static event Action<Item> ItemQuantityChanged;
        public static event Action<Item> ItemRemoved;

        public IReadOnlyDictionary<ItemData, Item> GetItems() 
            => new ReadOnlyDictionary<ItemData, Item>(_items);

        private void OnEnable() {
            _items = serializedItems.ToDictionary(item => item.data);
        }

        /// <summary>
        /// Adds an ItemData to the inventory. Increase the quantity of the item if it is stackable (ColorData) and is
        /// in inventory already, otherwise adds a new entry.
        /// </summary>
        /// <param name="data">ItemData to add</param>
        public void AddItem(ItemData data) {
            if (data.IsStackable && _items.TryGetValue(data, out Item existingItem)) {
                existingItem.quantity++;
                ItemQuantityChanged?.Invoke(existingItem);
            }
            else {
                Item newItem = new Item(data);
                _items[data] = newItem;
                serializedItems.Add(newItem);
                ItemAdded?.Invoke(newItem);
            }
        }

        /// <summary>
        /// Removes an ItemData from the inventory. Decrease the quantity of the item if it is stackable (ColorData)
        /// and it is in the inventory already, otherwise removes the entry.
        /// </summary>
        /// <param name="data">ItemData to remove</param>
        public void RemoveItem(ItemData data) {
            if (!_items.TryGetValue(data, out Item existingItem))
                return;

            if (data.IsStackable && existingItem.quantity > 1) {
                existingItem.quantity--;
                ItemQuantityChanged?.Invoke(existingItem);
            }
            else {
                _items.Remove(data);
                serializedItems.Remove(existingItem);
                ItemRemoved?.Invoke(existingItem);
            }
        }
    }

    [Serializable]
    public struct Item : IEquatable<Item> {
        public readonly ItemData data;
        public int quantity;

        public Item(ItemData data, int quantity = 1) {
            this.data = data;
            this.quantity = quantity;
        }

        public bool Equals(Item other) => Equals(data, other.data);
        public override bool Equals(object obj) => obj is Item other && Equals(other);
        public override int GetHashCode() => data.GetHashCode();
    }
}