using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public event Action PrimaryShot;
    public event Action SecondaryShot;

    [SerializeField] Projectile PrimaryProjectile;
    [SerializeField] Projectile SecondaryProjectile;

    public void ShootInputPrimary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;

        float angle = GetMouseAngle();
        ProjectileManager.Instance.ShootProjectileFromPosition(PrimaryProjectile, transform.position, angle);
        //PrimaryShot.Invoke();

        

    }

    public void ShootInputSecondary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;

        float angle = GetMouseAngle();
        ProjectileManager.Instance.ShootProjectileInRing(SecondaryProjectile, transform.position, 5, 20, angle);
        //SecondaryShot.Invoke();
    }

    private float GetMouseAngle()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector2 direction = mousePos - transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
