using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class PaintingArea : MonoBehaviour {
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;

    [Header("Bounds")]
    [SerializeField] private PolygonCollider2D boundsCollider;
    
    public Vector3 SpawnPosition => spawnPoint ? spawnPoint.position : transform.position;

    private void Awake() {
        if (!boundsCollider) {
            boundsCollider = GetComponent<PolygonCollider2D>();
        }
    }

    /// <summary>
    /// Checks if a position is within the walkable bounds
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>True if position is within bounds</returns>
    private bool IsWithinBounds(Vector3 position) {
        Vector3 localPoint = boundsCollider.transform.InverseTransformPoint(position);

        return boundsCollider.OverlapPoint(new Vector2(localPoint.x, localPoint.y));
    }

    /// <summary>
    /// Clamps a position to the nearest point within bounds
    /// </summary>
    /// <param name="position">The position to clamp</param>
    /// <returns></returns>
    public Vector3 ClampToBounds(Vector3 position) {
        if (IsWithinBounds(position)) {
            return position;
        }
        
        Vector2 point2D = new(position.x, position.y);
        Vector2 closestPoint = boundsCollider.ClosestPoint(point2D);
        
        return new Vector3(closestPoint.x, closestPoint.y, position.z);
    }
}