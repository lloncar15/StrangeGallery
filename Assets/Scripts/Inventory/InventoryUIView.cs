using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public class InventoryUIView : MonoBehaviour {
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [SerializeField] private ColorItemUIView colorItemPrefab;
        [SerializeField] private ImageItemUIView imageItemPrefab;
        
        private readonly List<(ItemData data, ItemUIView view)> items = new();

        private bool _isShown;

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
            if (!_isShown) {
                layoutGroup.gameObject.SetActive(true);
                _isShown = true;
            }
            
            ItemUIView view = CreateItemView(item);
            view.Initialize(item);
            items.Add((item.data, view));
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
            (ItemData data, ItemUIView view) entry = items.FirstOrDefault(e => e.data == item.data);
            if (entry.view == null)
                return;
            
            entry.view.UpdateQuantity(item.quantity);
        }

        private void OnItemRemoved(Item item) {
            int index = items.FindIndex(e => e.data == item.data);
            if (index == -1)
                return;

            items[index].view.AnimateDisappearance();
            items.RemoveAt(index);
        }

        private void SortViews() {
            List<ItemUIView> sorted = items.Select(e => e.view)
                .OrderByDescending(v => v.Data is ColorData)
                .ToList();

            for (int i = 0; i < sorted.Count; i++) {
                sorted[i].transform.SetSiblingIndex(i);
            }
        }
    }
}