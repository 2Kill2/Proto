using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "FireNovaAttack", menuName = "Bosses/Attacks/Fire Nova")]
public class FireNovaAttack : BossAttack
{
    [Header("Rings")]
    public int ringCount = 3;
    public float ringInterval = 0.36f;
    public float startRadius = 0.8f;
    public float radiusStep = 1.2f;
    public int projectilesPerRing = 16;
    public float angleOffsetPerRing = 11.25f;

    //Usable for other variations
    [Header("Ground Fire (optional)")]
    public bool spawnGroundFire = false;
    public GameObject firePatchPrefab;
    public float firePatchLifetime = 2.5f;

    [Header("Projectile Ring (optional)")]
    public bool shootProjectiles = false;
    public float projectileAngleOffset = 0f;

    [Header("Visuals (optional)")]
    public bool useAnimatorCues = true;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        if (useAnimatorCues) vis?.SetTelegraph(true);

        //get center and clamp to arena
        Vector2 center = boss.ClampInsideArena(boss.Rb.position);

        //Checks
        if (spawnGroundFire && !firePatchPrefab)
            Debug.LogWarning("[Fire Nova] spawnGroundFire enabled but firePatchPrefab is null.");
        if (shootProjectiles && (boss.ProjectilePrefab == null) || ProjectileManager.Instance == null)
            Debug.LogWarning("[FireNova] shootProjectiles enabled but projectile system not set.");

        float angleAccum = 0f;

        for(int r = 0; r < ringCount;  r++)
        {
            float radius = startRadius + r * radiusStep;
            float stepAngle = 360f / Mathf.Max(1, projectilesPerRing);
            float ringAngleOffset = angleAccum;

            //for fire patches
            if (spawnGroundFire && firePatchPrefab)
            {
                for(int i = 0; i < projectilesPerRing; i++)
                {
                    float angle = ringAngleOffset + i * stepAngle;
                    float rad = angle * Mathf.Deg2Rad;
                    Vector2 pos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
                    pos = boss.ClampInsideArena(pos);
                    var patch = Object.Instantiate(firePatchPrefab, pos, Quaternion.identity);

                    if (firePatchLifetime > 0f)
                    {
                        Object.Destroy(patch, firePatchLifetime);
                    }
                }
            }

            //For Fire Projectiles
            if (shootProjectiles && boss.ProjectilePrefab && ProjectileManager.Instance)
            {
                ProjectileManager.Instance.ShootProjectilesInArc(
                    boss.ProjectilePrefab,
                    center,
                    projectilesPerRing,
                    radius,
                    ringAngleOffset + projectileAngleOffset
                    );
                if (useAnimatorCues) vis?.TriggerCast();
            }

            if (r < ringCount - 1)
            {
                yield return new WaitForSeconds(ringInterval);
            }

            angleAccum += angleOffsetPerRing;
        }
        if (useAnimatorCues) vis?.SetTelegraph(false);

        if (postAttack > 0f)
            yield return new WaitForSeconds(postAttack);
    }
}
