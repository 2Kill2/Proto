
using System.Collections.Generic;

using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] protected Dictionary<string, Projectile> ProjectileCache = new Dictionary<string, Projectile>();

    //Singleton
    public static ProjectileManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);



      
    }

    [SerializeField] Projectile TestProjectile;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShootProjectileFromPosition(TestProjectile, Vector2.zero, Vector2.left);
        }
    }
    public void ShootProjectileFromPosition(Projectile projectile, Vector3 Pos, Vector3 Rotation)
    {
        NewProjectile(projectile, Pos, Rotation);
    }
    public void AddToCache(Projectile projectile)
    {
       ProjectileCache.Add(projectile.Name, projectile);
    }

    /// <summary>
    /// Acts as a modified instantiate which only creates new instances if there isnt one already
    /// </summary>
    /// <param name="Spawn">The projectile</param>
    /// <param name="Position">the position</param>
    /// <param name="Rotation">the rotation</param>
    /// <returns></returns>
    private Projectile NewProjectile(Projectile Spawn, Vector3 Position, Vector3 Rotation)
    {
        if (ProjectileCache.TryGetValue(Spawn.Name, out Projectile CachedProjectile))
        {
            CachedProjectile.gameObject.SetActive(true);
            ProjectileCache.Remove(CachedProjectile.Name);

            CachedProjectile.transform.position = Position;
            CachedProjectile.transform.rotation = Quaternion.Euler(Rotation);

            return CachedProjectile;
        }
       
        Projectile projectile = Instantiate(Spawn, Position, Quaternion.Euler(Rotation));

        ProjectileCache.Add(projectile.Name, projectile);

        return projectile;
    }

  
}
