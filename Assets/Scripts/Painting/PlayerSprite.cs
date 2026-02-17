using System;
using UnityEngine;

public class PlayerSprite : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform footTransform;

    private float _footOffset;
    
    public float FootOffset => _footOffset;

    private void Awake() {
        _footOffset = footTransform.localPosition.y;
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    /// <summary>
    /// Sets the transform position so that the foot transform is at the given position
    /// </summary>
    /// <param name="position">The position the foot transform should be at</param>
    private void SetPositionWithOffset(Vector3 position) {
        SetPosition(new Vector3(position.x, position.y - _footOffset, position.z));
    }
    
    public void SetInitialPlayerPositionInPainting(Vector3 spawnPoint) {
        SetPositionWithOffset(spawnPoint);
        gameObject.SetActive(true);
    }

    public void OnExitedPainting() {
        gameObject.SetActive(false);
    }
}
