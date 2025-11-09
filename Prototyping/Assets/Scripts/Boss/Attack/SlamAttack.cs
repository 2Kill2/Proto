using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetedSlamAttack", menuName = "Bosses/Attacks/Targeted Slam")]
public class SlamAttack : BossAttack
{
    [Header("Targeting")]
    public string playerTag = "Player";

    [Header("Telegraph")]
    public GameObject shadowPrefab;       // dark circle sprite prefab
    public float fallDelay = 0.8f;        // time between vanish and impact
    public Vector2 shadowScaleRange = new Vector2(0.6f, 1.2f);

    [Header("Impact")]
    public float slamRadius = 1.6f;
    public int slamDamage = 1;
    public LayerMask playerMask;

    [Header("Vanish")]
    public float vanishYOffset = 6f;      // visual “jump up” height
    public bool makeInvulnerableWhileHidden = true;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        // Pick a target point (nearest player or random) and clamp to arena
        Transform t = boss.GetCurrentTarget(playerTag);
        Vector2 aim = t ? (Vector2)t.position : boss.RandomPointInsideArena();
        aim = boss.ClampInsideArena(aim);

        // Spawn shadow
        GameObject shadow = null;
        if (shadowPrefab != null)
        {
            shadow = Object.Instantiate(shadowPrefab, aim, Quaternion.identity);
            shadow.transform.localScale = Vector3.one * shadowScaleRange.x;
        }

        // Hide boss visuals & collision, move it “up”
        var renderers = boss.GetComponentsInChildren<Renderer>(true);
        var col = boss.GetComponent<Collider2D>();
        var prevColEnabled = col ? col.enabled : false;
        if (col) col.enabled = false;

        foreach (var r in renderers) r.enabled = false;

        Vector2 offscreen = new Vector2(aim.x, aim.y + vanishYOffset);
        boss.Rb.position = offscreen; // teleport up

        float timer = 0f;
        while (timer < fallDelay)
        {
            timer += Time.deltaTime;
            // grow shadow slightly
            if (shadow)
            {
                float k = Mathf.InverseLerp(0f, fallDelay, timer);
                float s = Mathf.Lerp(shadowScaleRange.x, shadowScaleRange.y, k);
                shadow.transform.localScale = new Vector3(s, s, 1f);
            }
            yield return null;
        }

        // Impact reappear exactly at aim
        boss.Rb.position = aim;

        // Re-enable visuals and collision
        foreach (var r in renderers) r.enabled = true;
        if (col) col.enabled = prevColEnabled;

        // Remove shadow
        if (shadow) Object.Destroy(shadow);

        // Hit players in radius (simple AoE)
        if (slamRadius > 0f)
        {
            var hits = Physics2D.OverlapCircleAll(aim, slamRadius, playerMask);
            foreach (var h in hits)
            {
                var dmg = h.GetComponent<IDamageable>();
                if (dmg != null) dmg.TakeDamage(slamDamage);
                // Player Damage here
            }
        }

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}
