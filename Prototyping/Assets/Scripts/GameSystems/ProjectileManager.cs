
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

  

   
   public void ShootProjectilesInArc(Projectile projectile, Vector3 pos, int projectileCount, float arcAngle, float rotationOffset = 0f)
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
        Projectile projectile = null;

        if (ProjectilePool.TryGetValue(spawn.Data.nameID, out var list))
        {
            // Look for an inactive projectile
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].gameObject.activeInHierarchy)
                {
                    projectile = list[i];
                    list.RemoveAt(i);
                    break;
                }
            }
        }

        if (projectile == null)
        {
            projectile = Instantiate(spawn, position, Quaternion.Euler(0f, 0f, rotation));
        }
        else
        {
            projectile.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, rotation));
            projectile.gameObject.SetActive(true);
        }

        // Set velocity inside projectile's OnEnable
        if (projectile.Data.shootAudio != null)
            AudioSource.PlayClipAtPoint(projectile.Data.shootAudio, position);
    }


}
