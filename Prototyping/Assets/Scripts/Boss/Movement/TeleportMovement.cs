using System.Collections;
using UnityEngine;

namespace Dungeon
{
    [CreateAssetMenu(fileName = "TeleportMovement", menuName = "Bosses/Movement/Teleport")]
    public class TeleportMovement : BossMovement
    {
        [Tooltip("Delay while lich 'winds up' the teleport (on top of BossMovement.windUp).")]
        public float preTeleportDelay = 0.3f;

        [Tooltip("Delay after teleport before lich can move again.")]
        public float postTeleportDelay = 0.2f;

        [Tooltip("Try to stay at least this far from the player.")]
        public float minDistanceFromPlayer = 2.5f;

        public override IEnumerator Execute(BossBase boss)
        {
            if (windUp > 0f)
                yield return new WaitForSeconds(windUp);

            var vis = boss.GetComponent<BossVisuals>();
            vis?.SetMoving(false);
            vis?.SetTelegraph(true);

            yield return new WaitForSeconds(preTeleportDelay);

            vis?.SetTelegraph(false);
            vis?.SetHidden(true);

            //Pick a new position inside arena, not too close to the player if possible
            Vector2 newPos = boss.RandomPointInsideArena();
            Transform target = boss.GetCurrentTarget();
            int safety = 16;

            while (target != null && safety-- > 0 &&
                   Vector2.Distance(newPos, target.position) < minDistanceFromPlayer)
            {
                newPos = boss.RandomPointInsideArena();
            }

            boss.Rb.position = newPos;
            boss.transform.position = newPos;

            yield return new WaitForSeconds(postTeleportDelay);

            vis?.SetHidden(false);

            if (postAttack > 0f)
                yield return new WaitForSeconds(postAttack);
        }
    }
}
