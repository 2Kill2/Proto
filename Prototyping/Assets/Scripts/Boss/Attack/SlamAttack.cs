using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetedSlamAttack", menuName = "Bosses/Attacks/Targeted Slam")]
public class SlamAttack : BossAttack
{
    [Header("Targeting")]
    public string playerTag = "Player";

    [Header("Tracking")]
    public float trackingDuration = 0.6f;
    public float trackLerpSpeed = 15f;

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
    [Tooltip("If true, temporarily hide GFX using BossVisuals while vanished (jumped up).")]
    public bool hideGfxWhileVanished = true;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();

        // Pick a target point (nearest player or random) and clamp to arena
        Transform t = boss.GetCurrentTarget(playerTag);
        Vector2 aim = t ? (Vector2)t.position : boss.RandomPointInsideArena();
        aim = boss.ClampInsideArena(aim);

        vis?.SetTelegraph(true);

        // Spawn shadow
        GameObject shadow = null;
        if (shadowPrefab != null)
        {
            shadow = Object.Instantiate(shadowPrefab, aim, Quaternion.identity);
            shadow.transform.localScale = Vector3.one * shadowScaleRange.x;
        }

        Renderer[] hiddenRenders = null;
        if (hideGfxWhileVanished)
        {
            if (vis != null)
            {
                vis.SetHidden(true);
            }
            else
            {
                hiddenRenders = boss.GetComponentsInChildren<Renderer>(true);
                foreach (var r in hiddenRenders) r.enabled = false;
            }
        }

        boss.Rb.position = new Vector2(aim.x, aim.y + vanishYOffset);

        // TRACK phase (follow target while “in air”)
        vis?.SetTracking(true);
        float trackEnd = Time.time + trackingDuration;

        while (Time.time < trackEnd)
        {
            // update aim toward current player position, clamped to arena
            t = boss.GetCurrentTarget(playerTag);
            if (t)
            {
                Vector2 desired = boss.ClampInsideArena((Vector2)t.position);
                aim = Vector2.Lerp(aim, desired, 1f - Mathf.Exp(-trackLerpSpeed * Time.deltaTime));
            }

            if (shadow) shadow.transform.position = aim;
            yield return null;
        }
        vis?.SetTracking(false);

        Vector2 lockedPoint = aim;

        float teleT = 0f;
        while (teleT < fallDelay)
        {
            teleT += Time.deltaTime;
            // grow shadow slightly
            if (shadow)
            {
                float k = Mathf.InverseLerp(0f, fallDelay, teleT);
                float s = Mathf.Lerp(shadowScaleRange.x, shadowScaleRange.y, k);
                shadow.transform.localScale = new Vector3(s, s, 1f);
                shadow.transform.position = lockedPoint;
            }
            yield return null;
        }

        // Impact reappear exactly at aim
        boss.Rb.position = lockedPoint;

        if (hideGfxWhileVanished)
        {
            if (vis != null)
            {
                vis.SetHidden(false);
            }
            else if (hiddenRenders != null)
            {
                foreach (var r in hiddenRenders) r.enabled = true;
            }
        }

        vis?.TriggerImpact();
        vis?.SetTelegraph(false);

        // Remove shadow
        if (shadow) Object.Destroy(shadow);

        // Hit players in radius (simple AoE)
        if (slamRadius > 0f)
        {
            var hits = Physics2D.OverlapCircleAll(aim, slamRadius, playerMask);
            foreach (var h in hits)
            {
                h.gameObject.SendMessage("Damage", slamDamage, SendMessageOptions.DontRequireReceiver);
            }
        }

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}