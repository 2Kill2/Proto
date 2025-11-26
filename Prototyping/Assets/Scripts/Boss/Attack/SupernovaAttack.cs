using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SupernovaAttack", menuName = "Bosses/Attacks/Supernova")]
public class SupernovaAttack : BossAttack
{
    [Header("HP Condition")]
    public bool requireHpThreshold = false;
    public float hpThresholdPercent = 0.5f;

    [Header("Jump")]
    public float preJumpWindup = 0.4f;
    public float jumpHangTime = 0.4f;

    [Header("Rocks")]
    public GameObject rockPrefab;
    public int rockCount = 8;
    public float rockRadius = 3.5f;

    [Header("Blast Telegraph")]
    public GameObject blastTelegraphPrefab;
    public float blastTelegraphTime = 1.0f;
    public float blastRadius = 5f;

    [Header("Blast VFX")]
    public GameObject explosionPrefab;
    public float explosionVfxDuration = 1.5f;

    [Header("Damage")]
    public int blastDamage = 15;
    public LayerMask playerLayers;
    public LayerMask obstacleLayers;

    public override IEnumerator Execute(BossBase boss)
    {
        var paladin = boss as PaladinBoss;
        if (requireHpThreshold && paladin != null)
        {
            float hpPercent = (float)paladin.CurrentHP / paladin.MaxHP;
            if (hpPercent > hpThresholdPercent)
            {
                yield break;
            }
        }

        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        var rb = boss.Rb;

        var arena = boss.GetArenaBounds();
        Vector2 center = arena.center;

        var spawnedRocks = new System.Collections.Generic.List<GameObject>();

        if (rockPrefab != null && rockCount > 0)
        {
            float step = 360f / rockCount;
            for (int i = 0; i < rockCount; i++)
            {
                float a = (step * i) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                Vector2 pos = center + dir * rockRadius;

                GameObject r = Object.Instantiate(rockPrefab, pos, Quaternion.identity);
                spawnedRocks.Add(r);
            }
        }

        vis?.SetJump(true);
        if (preJumpWindup > 0f) yield return new WaitForSeconds(preJumpWindup);
        vis?.SetJump(false);

        vis?.SetHidden(true);

        if (jumpHangTime > 0f) yield return new WaitForSeconds(jumpHangTime);

        rb.position = center;
        boss.transform.position = center;

        vis?.SetHidden(false);
        vis?.TriggerImpact();

        GameObject blastTelegraph = null;

        if (blastTelegraphPrefab != null)
        {
            blastTelegraph = Object.Instantiate(blastTelegraphPrefab, center, Quaternion.identity);
            float d = blastRadius * 2f;
            blastTelegraph.transform.localScale = new Vector3(d, d, 1f);
        }

        vis?.SetJump(true);
        yield return new WaitForSeconds(blastTelegraphTime);
        vis?.SetJump(false);

        if (blastTelegraph != null) Object.Destroy(blastTelegraph);

        if (explosionPrefab != null)
        {
            GameObject exp = Object.Instantiate(
                explosionPrefab,
                center,
                Quaternion.identity
            );

            float d = blastRadius * 2f;
            exp.transform.localScale = new Vector3(d, d, 1f);

            if (explosionVfxDuration > 0f)
                Object.Destroy(exp, explosionVfxDuration);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, blastRadius, playerLayers);

        foreach (var h in hits)
        {
            if (h == null) continue;

            Vector2 targetPos = h.transform.position;
            Vector2 dir = (targetPos - center);
            float dist = dir.magnitude;
            if (dist <= 0.01f) continue;

            RaycastHit2D block = Physics2D.Raycast(center, dir.normalized, dist, obstacleLayers);
            if (block.collider != null)
            {
                continue;
            }

            h.SendMessage("TakeDamage", blastDamage, SendMessageOptions.DontRequireReceiver);
        }

        foreach (var r in spawnedRocks)
        {
            if (r != null)
                Object.Destroy(r);
        }

        if (explosionVfxDuration > 0f)
            yield return new WaitForSeconds(explosionVfxDuration);

        if (postAttack > 0f)
            yield return new WaitForSeconds(postAttack);
    }
}
