using UnityEngine;

[CreateAssetMenu(fileName = "Npc State Data", menuName = "GimGim/Painting/Npc State Data")]
public class NpcStateData : ScriptableObject {
    [Header("Movement")]
    public float moveSpeed = 2f;
    
    [Header("Roaming")]
    public float roamRadius = 3f;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 2f;

    [Header("Health")]
    public int maxHealth = 3;
}