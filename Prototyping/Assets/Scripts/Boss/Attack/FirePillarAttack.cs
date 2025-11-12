using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirePillarAttack", menuName = "Bosses/Attacks/Fire Pillar")]
public class FirePillarAttack : BossAttack
{
    [Header("Spawn")]
    public int pillarCount = 6;
    public float minSeparation = 1.5f;
    public float wallPadding = 0.6f;
    public int maxGlobalAttempts = 2000;

    [Header("Telegraph & Pillar")]
    public GameObject telegraphPrefab;
    public float telegraphTime = 1.8f;

    [Tooltip("Pillar Projectile prefab (must have Projectile + FirePillarAoE)")]
    public Projectile pillarProjectilePrefab;

    [Header("Visuals (optional)")]
    public bool useAnimatorCues = true;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        if (useAnimatorCues) vis?.SetTelegraph(true);

        var spots = GenerateWellSpacedPoints(boss, pillarCount, minSeparation, wallPadding, maxGlobalAttempts);

        var telegraphs = new List<GameObject>(spots.Count);
        if (telegraphPrefab)
            foreach (var s in spots)
                telegraphs.Add(Object.Instantiate(telegraphPrefab, s, Quaternion.identity));

        if (telegraphTime > 0f) yield return new WaitForSeconds(telegraphTime);

        if (pillarProjectilePrefab && ProjectileManager.Instance)
        {
            foreach (var s in spots)
            {
                ProjectileManager.Instance.ShootProjectileFromPosition(pillarProjectilePrefab, s, 0f);
                if (useAnimatorCues) vis?.TriggerCast();
            }
        }
        else
        {
            Debug.LogWarning("[FirePillarAttack] Missing pillarProjectilePrefab or ProjectileManager.Instance.");
        }

        foreach (var tg in telegraphs) if (tg) Object.Destroy(tg);

        if (useAnimatorCues) vis?.SetTelegraph(false);

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }

    private List<Vector2> GenerateWellSpacedPoints(BossBase boss, int count, float minDist, float pad, int maxAttempts)
    {
        var b = boss.GetArenaBounds();
        b.Expand(new Vector3(-2f * pad, -2f * pad, 0f));
        if (b.size.x <= 0f || b.size.y <= 0f) return new List<Vector2>();

        float cell = minDist / 1.41421356f;
        int cols = Mathf.Max(1, Mathf.CeilToInt(b.size.x / cell));
        int rows = Mathf.Max(1, Mathf.CeilToInt(b.size.y / cell));
        int[,] grid = new int[cols, rows];
        for (int x = 0; x < cols; x++) for (int y = 0; y < rows; y++) grid[x, y] = -1;

        var pts = new List<Vector2>(count);
        int attempts = 0;

        while (pts.Count < count && attempts < maxAttempts)
        {
            attempts++;

            Vector2 cand = new Vector2(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y));
            int cx = Mathf.Clamp(Mathf.FloorToInt((cand.x - b.min.x) / cell), 0, cols - 1);
            int cy = Mathf.Clamp(Mathf.FloorToInt((cand.y - b.min.y) / cell), 0, rows - 1);

            bool ok = true;
            for (int dx = -1; dx <= 1 && ok; dx++)
            {
                for (int dy = -1; dy <= 1 && ok; dy++)
                {
                    int nx = cx + dx, ny = cy + dy;
                    if (nx < 0 || ny < 0 || nx >= cols || ny >= rows) continue;
                    int idx = grid[nx, ny];
                    if (idx < 0) continue;
                    if ((pts[idx] - cand).sqrMagnitude < (minDist * minDist)) ok = false;
                }
            }
            if (!ok) continue;

            grid[cx, cy] = pts.Count;
            pts.Add(cand);
        }

        return pts;
    }
}