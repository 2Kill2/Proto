using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkMovement", menuName = "Bosses/Movement/Walk")]
public class WalkMovement : BossMovement
{
    [Header("Walk Settings")]
    public float walkSpeed = 5f;
    // seconds to walk before stopping
    public float maxDuration = 1.25f;
    // stopping distance
    public float targetTolerance = 0.1f;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        vis?.SetMoving(true);

        float endAt = Time.time + Mathf.Max(0.4f, maxDuration);

        while (Time.time < endAt)
        {
            Transform t = boss.GetCurrentTarget("Player");
            Vector2 target = t ? (Vector2)t.position : boss.RandomPointInsideArena();

            Vector2 pos = boss.Rb.position;
            Vector2 to = target - pos;
            float dist = to.magnitude;

            if (dist > 0.01f)
            {
                Vector2 dir = to / dist;
                Vector2 next = pos + dir * walkSpeed * Time.fixedDeltaTime;   // ← use walkSpeed
                next = boss.ClampInsideArena(next);
                boss.Rb.MovePosition(next);
            }

            yield return new WaitForFixedUpdate();
        }

        vis?.SetMoving(false);
        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}
