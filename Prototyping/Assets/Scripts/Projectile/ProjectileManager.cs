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

    [SerializeField] Projectile TestProjectile;
    [SerializeField] Projectile TestProjectile2;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectileFromPosition(TestProjectile, Vector2.zero, 0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShootProjectileFromPosition(TestProjectile2, Vector2.zero, 0);
        }
    }

    public void ShootProjectileFromPosition(Projectile projectile, Vector3 pos, float rotation)
    {
        NewProjectile(projectile, pos, rotation);
    }

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
    private Projectile NewProjectile(Projectile spawn, Vector3 position, float rotation)
    {
        if (ProjectilePool.TryGetValue(spawn.Data.nameID, out var list) && list.Count > 0)
        {
            Projectile cached = list[0];
            list.RemoveAt(0);

            cached.gameObject.SetActive(true);
            cached.transform.SetPositionAndRotation(position, Quaternion.Euler(new Vector3(0, 0, rotation)));

            return cached;
        }

        Projectile projectile = Instantiate(spawn, position, Quaternion.Euler(new Vector3(0, 0, rotation)));
        return projectile;
    }
}
