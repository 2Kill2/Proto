using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "FireBreathAttack", menuName = "Bosses/Attacks/Fire Breath")]
public class FireBreathAttack : BossAttack
{
    [Header("Cone")]
    public float coneAngle = 60f;
    public int projectilesPerBurst = 8;
    public float burstInterval = 0.08f;
    public float duration = 1.4f;
    public float sweepDegrees = 90f;

    [Header("Targeting")]
    public string playerTag = "Player";
    public bool lockOnAtStart = true;

    [Header("Visuals (optional)")]
    public bool useAnimatorCues = true;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        if (useAnimatorCues) vis?.SetBreath(true);

        //Flame Origin
        Vector2 origin = boss.ProjectileOrigin ? (Vector2)boss.ProjectileOrigin.position: boss.Rb.position;

        Transform t = boss.GetCurrentTarget(playerTag);
        Vector2 to = (t ? (Vector2)t.position : (origin + Vector2.right)) - origin;
        float baseFacing = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg;

        //Sweeping action and angle
        float halfSweep = Mathf.Abs(sweepDegrees) * 0.5f;
        float startAngle = baseFacing - halfSweep;
        float endAngle = baseFacing + halfSweep;

        float startTime = Time.time;
        float nextBurst = 0f;

        while (Time.time - startTime < duration)
        {
            origin = boss.ProjectileOrigin ? (Vector2)boss.ProjectileOrigin.position : boss.Rb.position;

            // If not locked, continuously aim at the player and sweeps that location
            if (!lockOnAtStart)
            {
                t = boss.GetCurrentTarget(playerTag);
                if (t)
                {
                    to = (Vector2)t.position - origin;
                    baseFacing = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg;
                    startAngle = baseFacing - halfSweep;
                    endAngle = baseFacing + halfSweep;
                }
            }

            float k = Mathf.InverseLerp(0f, duration, Time.time - startTime);
            float facing = Mathf.Lerp(startAngle, endAngle, k);

            // Fire a burst
            if (Time.time >= nextBurst && ProjectileManager.Instance && boss.ProjectilePrefab)
            {
                float rotationOffset = facing - (coneAngle * 0.5f);
                ProjectileManager.Instance.ShootProjectilesInArc(
                    boss.ProjectilePrefab,
                    origin,
                    projectilesPerBurst,
                    coneAngle,
                    rotationOffset
                );
                if (useAnimatorCues) vis?.TriggerCast();
                nextBurst = Time.time + burstInterval;
            }

            yield return null;
        }
        if (useAnimatorCues) vis?.SetBreath(false);
        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}
