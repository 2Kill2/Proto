
using System.Collections.Generic;

using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    [SerializeField] protected Dictionary<string, List<Projectile>> ProjectilePool = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

   

    #region SpawnTypes

    /// <summary>
    /// Fire a single shot
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="pos"></param>
    /// <param name="rotation">Angle to shoot</param>
    public void ShootProjectileFromPosition(Projectile projectile, Vector3 pos, float rotation)
    {
        NewProjectile(projectile, pos, rotation);
    }

    /// <summary>
    /// Fire in an arc
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="pos">where to shoot from</param>
    /// <param name="projectileCount">How many shots to fire</param>
    /// <param name="spawnRadius">Set to 360 to shoot in a full circle</param>
    /// <param name="offset">direction to shoot</param>
    public void ShootProjectileInRing(Projectile projectile, Vector3 pos, int projectileCount, float spawnRadius, float offset)
    {
        ShootProjectilesInArc(projectile, pos, projectileCount, spawnRadius,offset);
    }

   
   private void ShootProjectilesInArc(Projectile projectile, Vector3 pos, int projectileCount, float arcAngle, float rotationOffset = 0f)
   {
        if (projectileCount <= 0) return;

        float step = arcAngle / projectileCount;

        // Start angle so projectiles are centered around rotationOffset
        float startAngle = rotationOffset - arcAngle / 2f + step / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + step * i;
            NewProjectile(projectile, pos, angle);
        }
   }






    #endregion


    /// <summary>
    /// Called by projectiles when they are set inactive
    /// </summary>
    /// <param name="projectile"></param>
    public void AddToPool(Projectile projectile)
    {
        if (!ProjectilePool.TryGetValue(projectile.Data.nameID, out var list))
            ProjectilePool[projectile.Data.nameID] = list = new List<Projectile>();

        list.Add(projectile);
    }

    /// <summary>
    /// Calls to spawn a projectile somewhere, pulls from existing pool if possible
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private void NewProjectile(Projectile spawn, Vector3 position, float rotation)
    {
        if (ProjectilePool.TryGetValue(spawn.Data.nameID, out var list) && list.Count > 0)
        {
            Projectile cached = list[0];
            list.RemoveAt(0);

            cached.gameObject.SetActive(true);
            cached.transform.SetPositionAndRotation(position, Quaternion.Euler(new Vector3(0, 0, rotation)));

            // Set linear velocity along current rotation
            if (cached.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.linearVelocity = (Vector2)cached.transform.right * cached.Data.velocity;
            }

            return;
        }

        Projectile projectile = Instantiate(spawn, position, Quaternion.Euler(new Vector3(0, 0, rotation)));

        if (projectile.TryGetComponent<Rigidbody2D>(out var rbNew))
        {
            rbNew.linearVelocity = Vector2.zero;
            rbNew.linearVelocity = (Vector2)projectile.transform.right * projectile.Data.velocity;
        }

        return;
    }

}
