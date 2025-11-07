using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Projectile PrimaryProjectile;
    [SerializeField] Projectile SecondaryProjectile;

    public void ShootInputPrimary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;

        float angle = GetMouseAngle();
        ProjectileManager.Instance.ShootProjectileFromPosition(PrimaryProjectile, transform.position, angle);
    }

    public void ShootInputSecondary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;

        float angle = GetMouseAngle();
        ProjectileManager.Instance.ShootProjectileInRing(SecondaryProjectile, transform.position, 5, 30, angle);
    }

    private float GetMouseAngle()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // ensure same plane
        Vector2 direction = mousePos - transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
