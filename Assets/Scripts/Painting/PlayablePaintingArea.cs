using UnityEngine;

/// <summary>
/// Represents and monitors the playable area for the 2d gameplay. 
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class PlayablePaintingArea : MonoBehaviour {
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
        return boundsCollider.OverlapPoint(new Vector2(position.x, position.y));
    }

    /// <summary>
    /// Clamps a position to the nearest point within bounds
    /// </summary>
    /// <param name="position">The position to clamp</param>
    /// <param name="footOffsetY"> The offset of players feet on the Y axis</param>
    /// <returns>The clamped sprite pivot position</returns>
    public Vector3 ClampToBounds(Vector3 position, float footOffsetY) {
        Vector3 footPosition = new(position.x, position.y + footOffsetY, position.z);
        
        if (IsWithinBounds(footPosition)) {
            return position;
        }
        
        Vector2 footPoint = new(footPosition.x, footPosition.y);
        Vector2 closestPoint = boundsCollider.ClosestPoint(footPoint);
        
        return new Vector3(closestPoint.x, closestPoint.y - footOffsetY, position.z);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (boundsCollider == null) {
            boundsCollider = GetComponent<PolygonCollider2D>();
        }
        
        if (boundsCollider != null) {
            Gizmos.color = Color.green;
            
            // Draw the polygon bounds in 3D space
            for (int pathIndex = 0; pathIndex < boundsCollider.pathCount; pathIndex++) {
                Vector2[] path = boundsCollider.GetPath(pathIndex);
                
                for (int i = 0; i < path.Length; i++) {
                    Vector2 current = path[i];
                    Vector2 next = path[(i + 1) % path.Length];
                    
                    Vector3 worldCurrent = boundsCollider.transform.TransformPoint(new Vector3(current.x, current.y, 0f));
                    Vector3 worldNext = boundsCollider.transform.TransformPoint(new Vector3(next.x, next.y, 0f));
                    
                    Gizmos.DrawLine(worldCurrent, worldNext);
                }
            }
        }
        
        if (spawnPoint != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.05f);
        }
    }
#endif
}