using UnityEngine;

[RequireComponent(typeof(BossVisuals))]
[RequireComponent(typeof(Rigidbody2D))]
public class PaladinBoss : BossBase
{
    [Header("Paladin Settings")]
    [SerializeField] private string playerTag = "Player";

    public int CurrentHP => currentHP;
    public int MaxHP => config.maxHP;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override Transform GetCurrentTarget(string tag = "Player")
    {
        return base.GetCurrentTarget(playerTag);
    }
}
