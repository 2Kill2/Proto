using UnityEngine;

/// <summary>
/// Boss Controller for King Slime
/// Plug the sciptable objects in the Inspector:
/// - Movement: BounceMovement
/// - Attacks: TargetedSlamAttack, RingAttack, EngulfAttack
/// </summary>
public class KingSlime : BossBase
{
    [Header("Targeting")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform player; //Autofills at runtime

    [Header("Range")]
    [SerializeField] private float ringMinRange = 6f;
    [SerializeField] private float slamRange = 5f;
    [SerializeField] private float engulfRange = 2f;

    [Header("Attack Chance")]
    [SerializeField] private float chanceRing = 1.0f;
    [SerializeField] private float chanceSlam = 1.5f;
    [SerializeField] private float chanceEngulf = 0.5f;

    [Header("Behavior")]
    [SerializeField] private float preferBounceChance = 0.6f; //bias to move instead of attacking
    [SerializeField] private float minTimeBetweenEngulf = 6f; // cooldown for engulf attacks

    [SerializeField] private float retargetInterval = 3f;
    private float nextRetarget = 0f;

    private float lastEngulfAt = -999f;

    protected override void OnEnable()
    {
        if (player == null)
        {
            player = FindNearestPlayer();
        }
        base.OnEnable();
    }

    void Update()
    {
        if (Time.time >= nextRetarget)
        {
            player = FindNearestPlayer();
            nextRetarget = Time.time + retargetInterval;
        }
    }

    protected override BossMovement ChooseMovement()
    {
        //do a movement instead of attack 60% of the time if both are available
        if (Random.value < preferBounceChance)
        {
            // try to pick a "BounceMovement" if present
            for (int i = 0; i < config.movements.Length; i++)
            {
                if (config.movements[i] != null &&
                    config.movements[i].name.ToLower().Contains("bounce"))
                {
                    return config.movements[i];
                }
            }
        }
        return base.ChooseMovement();
    }

    protected override BossAttack ChooseAttack()
    {
        if (config == null || config.attacks == null || config.attacks.Length == 0)
            return null;

        if (player == null)
            return base.ChooseAttack();

        float d = Vector2.Distance(player.position, transform.position);
        float now = Time.time;

        // if player is close at the start trigger engulf
        BossAttack engulf = FindAttackByNameContains("engulf");
        bool canEngulf = (engulf != null) && (d <= engulfRange) && (now - lastEngulfAt >= minTimeBetweenEngulf);

        // if player is close = Engulf attack
        BossAttack slam = FindAttackByNameContains("slam");
        bool canSlam = (slam != null) && (d <= slamRange);

        // if player is far/medium = Ring Attack
        BossAttack ring = FindAttackByNameContains("ring");
        bool canRing = (ring != null) && (d >= ringMinRange);

        // Chance of choices among valid candidates
        float wSum = 0f;
        float wEngulf = (canEngulf ? chanceEngulf : 0f);
        float wSlam = (canSlam ? chanceSlam : 0f);
        float wRing = (canRing ? chanceRing : 0f);
        wSum = wEngulf + wSlam + wRing;

        if (wSum > 0f)
        {
            float r = Random.Range(0f, wSum);
            if (r < wEngulf)
            {
                lastEngulfAt = now;
                return engulf;
            }
            r -= wEngulf;
            if (r < wSlam) return slam;
            return ring;
        }

        if (ring != null) return ring;
        if (slam != null) return slam;
        if (engulf != null) return engulf;

        return base.ChooseAttack();
    }

    private BossAttack FindAttackByNameContains(string key)
    {
        if (config == null || config.attacks == null) return null;
        key = key.ToLower();
        for (int i = 0; i < config.attacks.Length; i++)
        {
            var a = config.attacks[i];
            if (a != null && a.name.ToLower().Contains(key))
                return a;
        }
        return null;
    }

    private Transform FindNearestPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag(playerTag);
        if (players == null || players.Length == 0) return null;

        float bestDist = float.MaxValue;
        Transform best = null;
        Vector2 pos = transform.position;

        foreach (var p in players)
        {
            if (!p.activeInHierarchy) continue;
            float d = Vector2.Distance(pos, p.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = p.transform;
            }
        }
        return best;
    }
}
