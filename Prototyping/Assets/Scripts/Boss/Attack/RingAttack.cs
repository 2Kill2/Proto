using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "RingAttack", menuName = "Bosses/Attacks/Ring Attack")]
public class RingAttack : BossAttack
{
    [Header("Ring Settings")]
    public int projectileCount = 8;
    //To rotate ring pattern
    public float angleOffset = 0f;
    public float spawnRadius = 0.2f;
    [Header("Projectile")]
    public Projectile projectilePrefab;

    public override IEnumerator Execute(BossBase boss)
    {
        if(windUp > 0f) yield return new WaitForSeconds(windUp);

        Vector2 origin = boss.ProjectileOrigin != null 
            ? (Vector2)boss.ProjectileOrigin.position 
            : (Vector2)boss.transform.position;

        Vector2 startPos = origin + (Vector2.right * spawnRadius);

        if (ProjectileManager.Instance == null || projectilePrefab == null)
        {
            Debug.LogWarning("[RingAttack] Missing ProjectileManager or projectilePrefab.");
        }
        else
        {
            ProjectileManager.Instance.ShootProjectilesInArc(projectilePrefab, startPos, projectileCount, 360f, angleOffset);
        }

        if (postAttack > 0f)yield return new WaitForSeconds(postAttack);
    }
}
