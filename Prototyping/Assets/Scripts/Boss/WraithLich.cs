using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WraithLich : BossBase
{
    [Header("Lich Teleport Logic")]
    [SerializeField] private int minAttacksBeforeTeleport = 1;
    [SerializeField] private int maxAttacksBeforeTeleport = 3;

    [Tooltip("Movement used for normal wandering.")]
    [SerializeField] private BossMovement wanderMovement;

    [Tooltip("Movement used when lich teleports.")]
    [SerializeField] private BossMovement teleportMovement;

    private int attacksSinceTeleport = 0;
    private int attacksBeforeTeleport = 1;
    private bool shouldTeleport = false;

    protected override void Awake()
    {
        base.Awake();
        RollNextTeleportCount();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        attacksSinceTeleport = 0;
        shouldTeleport = false;
        RollNextTeleportCount();
    }

    private void RollNextTeleportCount()
    {
        attacksBeforeTeleport = Random.Range(minAttacksBeforeTeleport, maxAttacksBeforeTeleport + 1);
    }

    public void Heal(int amount)
    {
        if (!isAlive || amount <= 0) return;

        int maxHp = config != null ? config.maxHP : currentHP;
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHp);
    }

    protected override BossAttack ChooseAttack()
    {
        var attack = base.ChooseAttack();

        //Count attacks and schedule next teleport
        attacksSinceTeleport++;
        if (attacksSinceTeleport >= attacksBeforeTeleport)
        {
            attacksSinceTeleport = 0;
            RollNextTeleportCount();
            shouldTeleport = true;

            //Force movement to be available immediately
            nextMoveAt = Time.time;
        }

        return attack;
    }

    protected override BossMovement ChooseMovement()
    {
        if (shouldTeleport && teleportMovement != null)
        {
            shouldTeleport = false;
            return teleportMovement;
        }

        //Default wandering move
        if (wanderMovement != null) return wanderMovement;

        //Fallback to base behaviour
        return base.ChooseMovement();
    }
}