using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BossBase : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] protected BossConfig config;
    // optional bullet origin point
    [SerializeField] private Transform projectileOrigin;

    [Header("Arena")]
    [SerializeField] private Collider2D arenaCollider;

    [Header("Top-Down Settings")]
    [SerializeField] private float arenaPaddingX = 0.8f;
    [SerializeField] private float arenaPaddingY = 0.8f;

    [Header("Projectiles")]
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private bool autoStart = true;

    protected Rigidbody2D rb;
    protected int currentHP;
    protected bool isAlive = true;

    protected float nextMoveAt = 0f;
    protected float nextAttackAt = 0f;

    public Rigidbody2D Rb => rb;
    public BossConfig Config => config;
    public Transform ProjectileOrigin => projectileOrigin;
    public Projectile ProjectilePrefab => projectilePrefab;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = config != null ? config.maxHP : 100;
    }

    protected virtual void OnEnable()
    {
        rb = rb ? rb : GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (autoStart) StartCoroutine(MainLoop());
    }

    public virtual void TakeDamage(int dmg)
    {
        if (!isAlive) return;
        currentHP -= Mathf.Max(0, dmg);

        var vis = GetComponent<BossVisuals>();
        vis?.TriggerHurt();

        if (currentHP <= 0) Die();
    }

    protected virtual void Die()
    {
        isAlive = false;
        StopAllCoroutines();

        var vis = GetComponent<BossVisuals>();
        vis?.TriggerDeath();

        gameObject.SetActive(false);
    }


    protected IEnumerator MainLoop()
    {
        if (config == null)
        {
            Debug.LogError($"[{name}] Missing BossConfig.");
            yield break;
        }

        yield return null;

        isAlive = true;
        nextMoveAt = Time.time + Random.Range(config.minMoveCooldown, config.maxMoveCooldown);
        nextAttackAt = Time.time + config.attackCooldown;

        while (isAlive)
        {
            // movement timing
            if (Time.time >= nextMoveAt && config.movements != null && config.movements.Length > 0)
            {
                var move = ChooseMovement();
                if (move != null) { yield return StartCoroutine(move.Execute(this)); }
                nextMoveAt = Time.time + Random.Range(config.minMoveCooldown, config.maxMoveCooldown);
            }

            if (Time.time >= nextAttackAt && config.attacks != null && config.attacks.Length > 0)
            {
                var attack = ChooseAttack();
                if (attack != null) { yield return StartCoroutine(attack.Execute(this)); }
                nextAttackAt = Time.time + config.attackCooldown;
            }

            yield return null;
        }
    }

    //--------------------------------------------------------------- Movement Selection ------------------------------------------------------------
    protected virtual BossMovement ChooseMovement()
    {
        if (config == null || config.movements == null || config.movements.Length == 0) return null;
        int i = Random.Range(0, config.movements.Length);
        return config.movements[i];
    }

    //------------------------------------------------------------------- Movement -------------------------------------------------------------------
    public Bounds GetArenaBounds()
    {
        if (arenaCollider != null) return arenaCollider.bounds;
        return new Bounds(Vector3.zero, new Vector3(20, 10, 0));
    }
    public void SetArena(Collider2D arena)
    {
        arenaCollider = arena;
    }

    public Vector2 ClampInsideArena(Vector2 p)
    {
        var b = GetArenaBounds();
        p.x = Mathf.Clamp(p.x, b.min.x + arenaPaddingX, b.max.x - arenaPaddingX);
        p.y = Mathf.Clamp(p.y, b.min.y + arenaPaddingY, b.max.y - arenaPaddingY);
        return p;
    }
    public Vector2 RandomPointInsideArena()
    {
        var b = GetArenaBounds();
        float x = Random.Range(b.min.x + arenaPaddingX, b.max.x - arenaPaddingX);
        float y = Random.Range(b.min.y + arenaPaddingY, b.max.y - arenaPaddingY);
        return new Vector2(x, y);
    }

    //------------------------------------------------------------------- Attacks  -------------------------------------------------------------------
    protected virtual BossAttack ChooseAttack()
    {
        // Default: pick a random one
        if (config.attacks == null || config.attacks.Length == 0) return null;
        int i = Random.Range(0, config.attacks.Length);
        return config.attacks[i];
    }

    //------------------------------------------------------------------ Projectiles ------------------------------------------------------------------
    public virtual void SpawnProjectile(Vector2 position, Vector2 direction, float speed)
    {
        if (ProjectileManager.Instance == null || projectilePrefab == null)
        {
            Debug.LogWarning($"[{name}] Missing ProjectileManager or projectilePrefab.");
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ProjectileManager.Instance.ShootProjectileFromPosition(projectilePrefab,position,angle);
    }

    //------------------------------------------------------------------ Targeting ------------------------------------------------------------------
    protected Transform currentTarget;
    /// <summary>
    /// Returns the currently tracked player. If none, finds the nearest by tag.
    /// </summary>
    public virtual Transform GetCurrentTarget(string tag = "Player")
    {
        //Check if already tracking current target and target is valid
        if (currentTarget != null && currentTarget.gameObject.activeInHierarchy)
            return currentTarget;

        //else find nearest target
        var players = GameObject.FindGameObjectsWithTag(tag);
        if (players == null || players.Length == 0)
            return null;

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

        currentTarget = best;
        return currentTarget;
    }

    /// <summary>
    /// Manually set the target from outside
    /// </summary>
    public virtual void SetTarget(Transform t)
    {
        currentTarget = t;
    }
}