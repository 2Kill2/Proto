using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public event Action PrimaryShot;
    public event Action SecondaryShot;

    public Projectile Primary;
    public Projectile Secondary;

    public float AimAngle;
    private bool _usingGamepad;

    private float _primaryCooldown;
    private float _secondaryCooldown;
    private bool _primaryHeld;
    private bool _secondaryHeld;
    private Rigidbody2D _rb;

  
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
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
        if (_primaryHeld && _primaryCooldown <= 0 && Primary.Data.firerate > 0)
        {
            Fire(Primary);
            _primaryCooldown = (Primary.Data.firerate > 0)
                 ? 1f / Primary.Data.firerate : 0f;

            PrimaryShot?.Invoke();
        }
            

        // Secondary auto fire
        if (_secondaryHeld && _secondaryCooldown <= 0 && Secondary.Data.firerate > 0)
        {
            Fire(Secondary);
            _secondaryCooldown = (Secondary.Data.firerate > 0)
       ? 1f / Secondary.Data.firerate
       : 0f;

            SecondaryShot?.Invoke();
        }
          
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

        if (Primary.Data.firerate <= 0)
        {
            Fire(Primary);
            _primaryCooldown = (Primary.Data.firerate > 0)
                 ? 1f / Primary.Data.firerate : 0f;

            PrimaryShot?.Invoke();
        }
           

     
    }

    public void ShootInputSecondary(InputAction.CallbackContext input)
    {
        if (input.started)
            _secondaryHeld = true;
        else if (input.canceled)
            _secondaryHeld = false;

        if (!input.performed)
            return;

        if (Secondary.Data.firerate <= 0)
        {
            Fire(Secondary);
            _secondaryCooldown = (Secondary.Data.firerate > 0)
         ? 1f / Secondary.Data.firerate
         : 0f;

            SecondaryShot?.Invoke();
        }
           
    }

    private void Fire(Projectile shot)
    {
        if (shot.Data.fireType == ProjectileData.FireMode.Single)
        {
            ProjectileManager.Instance.ShootProjectileFromPosition(
            shot, transform.position, AimAngle);
        }
        else
        {
            ProjectileManager.Instance.ShootProjectilesInArc(
            shot, transform.position, shot.Data.count, shot.Data.angle, AimAngle);
        }
        if (shot.Data.recoil != Vector2.zero)
            StartCoroutine(Recoil(shot));


    }

    /// <summary>
    /// Handles recoil, if any
    /// </summary>
    /// <param name="shot"></param>

    
    IEnumerator Recoil(Projectile shot)
    {
        Vector2 rotatedRecoil = Quaternion.Euler(0, 0, AimAngle + -90) * shot.Data.recoil;
        _rb.AddForce(rotatedRecoil, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);
        _rb.linearVelocity = Vector2.zero;
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
