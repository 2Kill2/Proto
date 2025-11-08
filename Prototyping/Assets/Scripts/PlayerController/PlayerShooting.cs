using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public event Action PrimaryShot;
    public event Action SecondaryShot;

    [SerializeField] private ClassData Data;

    public float AimAngle;
    private bool _usingGamepad;

    private void Update()
    {
        // Continuously update aim if using mouse
        if (!_usingGamepad)
        {
            AimAngle = GetMouseAngle();
        }
    }

    public void StickAim(InputAction.CallbackContext input)
    {
        Vector2 direction = input.ReadValue<Vector2>();

        // If the stick is moved, switch to gamepad aiming
        if (direction.sqrMagnitude > 0.01f)
        {
            _usingGamepad = true;
            AimAngle = DirectionToAngle(direction);
        }
        else
        {
            // If the stick is released, fall back to mouse aiming
            _usingGamepad = false;
        }
    }

    public void ShootInputPrimary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;

        ProjectileManager.Instance.ShootProjectileFromPosition(
            Data.Primary, transform.position, AimAngle);

        PrimaryShot?.Invoke();
    }

    public void ShootInputSecondary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;

        ProjectileManager.Instance.ShootProjectileInRing(
            Data.Secondary, transform.position, 5, 20, AimAngle);

        SecondaryShot?.Invoke();
    }

    private float GetMouseAngle()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector2 direction = mousePos - transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private float DirectionToAngle(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.0001f)
            return AimAngle; // Keep last valid angle
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
