using UnityEngine;
using System.Collections;



[CreateAssetMenu(fileName = "WanderMovement", menuName = "Bosses/Movement/Wander")]
public class WanderMovement : BossMovement
{
    [Header("Wander Settings")]
    public float moveSpeed = 3f;
    public float moveDuration = 1.5f;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f)
            yield return new WaitForSeconds(windUp);

        var rb = boss.Rb;
        var vis = boss.GetComponent<BossVisuals>();

        Vector2 targetPos = boss.RandomPointInsideArena();
        float elapsed = 0f;

        vis?.SetMoving(true);

        while (elapsed < moveDuration)
        {
            Vector2 current = rb.position;
            Vector2 next = Vector2.MoveTowards(current, targetPos, moveSpeed * Time.deltaTime);
            rb.MovePosition(next);

            elapsed += Time.deltaTime;
            yield return null;
        }

        vis?.SetMoving(false);

        if (postAttack > 0f)
            yield return new WaitForSeconds(postAttack);
    }
}


