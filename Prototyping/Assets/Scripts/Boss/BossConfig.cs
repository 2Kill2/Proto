using UnityEngine;

[CreateAssetMenu(fileName = "BossConfig", menuName = "Bosses/Boss Config")]
public class BossConfig : ScriptableObject
{
    [Header("Health")]
    public int maxHP = 100;

    [Header("Movement (cooldowns select when to attempt a move)")]
    public float minMoveCooldown = 0.8f;
    public float maxMoveCooldown = 2.0f;

    [Header("Attacks")]
    public float attackCooldown = 2f;
    public BossMovement[] movements;
    public BossAttack[] attacks;
}

