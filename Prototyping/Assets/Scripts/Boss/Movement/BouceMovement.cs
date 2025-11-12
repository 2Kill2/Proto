using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BounceMovement", menuName = "Bosses/Movement/Bounce")]
public class BounceMovement : BossMovement
{
    [Header("Bounce")]
    public float maxHorizontal = 5f;
    public float duration = 2.0f;
    public float planeSpeed = 4f;
    public float segmentTime = 1.8f;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        vis?.SetMoving(true);

        float endAt = Time.time + Mathf.Max(0.1f, duration);
        Vector2 target = boss.RandomPointInsideArena();

        while (Time.time < endAt)
        {
            Vector2 pos = boss.Rb.position;
            Vector2 to = target - pos;
            float dist = to.magnitude;

            if (dist < 0.1f)
            {
                target = boss.RandomPointInsideArena();
            }
            else
            {
                Vector2 dir = to / Mathf.Max(dist, 0.0001f);
                // Reuse maxHorizontal as a "speed" to avoid new fields; rename later if you want.
                Vector2 next = pos + dir * maxHorizontal * Time.fixedDeltaTime;
                next = boss.ClampInsideArena(next);
                boss.Rb.MovePosition(next);
            }

            yield return new WaitForFixedUpdate();
        }

        vis?.SetMoving(false);
        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}
