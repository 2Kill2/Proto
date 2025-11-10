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

    private float _primaryCooldown;
    private float _secondaryCooldown;
    private bool _primaryHeld;
    private bool _secondaryHeld;

    private void Update()
    {
        if (!_usingGamepad)
            AimAngle = GetMouseAngle();

        HandleFireRates();
    }

    private void HandleFireRates()
    {
        if (_primaryCooldown > 0)
            _primaryCooldown -= Time.deltaTime;
        if (_secondaryCooldown > 0)
            _secondaryCooldown -= Time.deltaTime;

        // Primary auto fire
        if (_primaryHeld && _primaryCooldown <= 0 && Data.Primary.Data.firerate > 0)
            FirePrimary();

        // Secondary auto fire
        if (_secondaryHeld && _secondaryCooldown <= 0 && Data.Secondary.Data.firerate > 0)
            FireSecondary();
    }

    public void StickAim(InputAction.CallbackContext input)
    {
        Vector2 direction = input.ReadValue<Vector2>();

        if (direction.sqrMagnitude > 0.01f)
        {
            _usingGamepad = true;
            AimAngle = DirectionToAngle(direction);
        }
        else
        {
            _usingGamepad = false;
        }
    }

    public void ShootInputPrimary(InputAction.CallbackContext input)
    {
        if (input.started)
            _primaryHeld = true;
        else if (input.canceled)
            _primaryHeld = false;

        if (!input.performed)
            return;

        if (Data.Primary.Data.firerate <= 0)
            FirePrimary();
    }

    public void ShootInputSecondary(InputAction.CallbackContext input)
    {
        if (input.started)
            _secondaryHeld = true;
        else if (input.canceled)
            _secondaryHeld = false;

        if (!input.performed)
            return;

        if (Data.Secondary.Data.firerate <= 0)
            FireSecondary();
    }

    private void FirePrimary()
    {
        ProjectileManager.Instance.ShootProjectileFromPosition(
            Data.Primary, transform.position, AimAngle);

        _primaryCooldown = (Data.Primary.Data.firerate > 0)
            ? 1f / Data.Primary.Data.firerate
            : 0f;

        PrimaryShot?.Invoke();
    }

    private void FireSecondary()
    {
        ProjectileManager.Instance.ShootProjectilesInArc(
            Data.Secondary, transform.position, 5, 20, AimAngle);

        _secondaryCooldown = (Data.Secondary.Data.firerate > 0)
            ? 1f / Data.Secondary.Data.firerate
            : 0f;

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
            return AimAngle;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
