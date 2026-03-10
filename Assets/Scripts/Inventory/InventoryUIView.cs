using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public class InventoryUIView : MonoBehaviour {
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [SerializeField] private ColorItemUIView colorItemPrefab;
        [SerializeField] private ImageItemUIView imageItemPrefab;

        private readonly Dictionary<ItemData, ItemUIView> items = new();

        private void OnEnable() {
            InventoryController.ItemAdded += OnItemAdded;
            InventoryController.ItemQuantityChanged += OnItemQuantityChanged;
            InventoryController.ItemRemoved += OnItemRemoved;
        }

        private void OnDisable() {
            InventoryController.ItemAdded -= OnItemAdded;
            InventoryController.ItemQuantityChanged -= OnItemQuantityChanged;
            InventoryController.ItemRemoved -= OnItemRemoved;
        }

        private void OnItemAdded(Item item) {
            ItemUIView view = CreateItemView(item);
            view.Initialize(item);
            items[item.data] = view;
            SortViews();
        }

        private ItemUIView CreateItemView(Item item) {
            return item.data switch {
                ColorData => Instantiate(colorItemPrefab, layoutGroup.transform),
                ImageData => Instantiate(imageItemPrefab, layoutGroup.transform),
                _ => null
            };
        }

        private void OnItemQuantityChanged(Item item) {
            if (!items.TryGetValue(item.data, out ItemUIView view))
                return;
            
            view.UpdateQuantity(item.quantity);
        }

        private void OnItemRemoved(Item item) {
            if (!items.Remove(item.data, out ItemUIView view))
                return;

            Destroy(view.gameObject);
        }

        private void SortViews() {
            List<ItemUIView> sorted = items.Values
                .OrderByDescending(v => v.Data is ColorData)
                .ToList();

            for (int i = 0; i < sorted.Count; i++) {
                sorted[i].transform.SetSiblingIndex(i);
            }
        }
    }
}