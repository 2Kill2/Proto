using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public event Action PrimaryShot;
    public event Action SecondaryShot;

    [SerializeField] ClassData Data;

    float _stickAngle;
    public void ShootInputPrimary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;
        InputDevice device = input.control.device;

        float angle = 0;

        if (device is Gamepad)
            angle = _stickAngle;
        else
            angle = GetMouseAngle();

        ProjectileManager.Instance.ShootProjectileFromPosition(Data.Primary, transform.position, angle);
        if(PrimaryShot != null) PrimaryShot.Invoke();

    }
    

    public void ShootInputSecondary(InputAction.CallbackContext input)
    {
        if (!input.performed) return;
        InputDevice device = input.control.device;

        float angle = 0;

        if(device is Gamepad)
             angle = _stickAngle;
        else
            angle = GetMouseAngle();



            ProjectileManager.Instance.ShootProjectileInRing(Data.Secondary, transform.position, 5, 20, angle);
        if(SecondaryShot != null) SecondaryShot.Invoke();
    }
    

    public void StickAim(InputAction.CallbackContext input)
    {
        _stickAngle = DirectionToAngle(input.ReadValue<Vector2>());
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
            return 0f;

        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
