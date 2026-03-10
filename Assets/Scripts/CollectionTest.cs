using System;
using Inventory;
using UnityEngine;

public class CollectionTest : MonoBehaviour {
    [SerializeField] private ItemData data;

    private void OnTriggerEnter2D(Collider2D other) {
        InventoryController.Instance.AddItem(data);
        Destroy(gameObject);
    }
}
