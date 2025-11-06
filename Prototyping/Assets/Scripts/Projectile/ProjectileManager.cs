
using System.Collections.Generic;

using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private List<Projectile> ProjectileCache = new List<Projectile>();


    public void ShootProjectileFromPosition()
    {

    }


    private Projectile NewProjectile(Projectile Spawn, Vector3 Position, Vector3 Rotation)
    {
        Projectile projectile = Instantiate(Spawn, Position, Quaternion.Euler(Rotation));

        return projectile;
    }
}
