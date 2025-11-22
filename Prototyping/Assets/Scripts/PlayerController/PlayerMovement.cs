
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    //Handles player movement and dashing
    //Use the "Invoke Unity Events" behaviour on the player input component
    //Call the functions in this script as unity events from the input component


    //Fields
    [Header("Required")]
    [Tooltip("Rigidbody to Move")]
    [SerializeField] Rigidbody2D RB2D;
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] SpriteRenderer SpriteRenderer;
    [Header("Tweakable")]

    [Tooltip("I dont know what the units are here")]
    [SerializeField] float WalkSpeed;

    [Tooltip("The amount of force per second while dodging")]
    [SerializeField] float DodgeForce;

    [Tooltip("The length in seconds that the player dodges for")]
    [SerializeField] float DodgeLength;

    [Tooltip("The time in seconds betweem dodges")]
    [SerializeField] float DodgeCooldown;

    //Private variables

    //Toggles on when the player is recieving a move input
    private bool _moveInput;

    //What direction is to move in
    private Vector3 _moveDirection;

    //is dodge on cooldown
    private bool _dodgeCD = true;

    public event Action Dashed;

    [Header("PermanentBoosts")]
    public float MoveSpeedBoost = 0;
    public float DashCDReduction = 1;
    

    private void Update()
    {
        MovePlayer(); 
        SpeedBuffHandler();
    }

    /// <summary>
    /// Takes a Vector2 Input and tells the mover where to move to
    /// </summary>
    /// <param name="Input">Gets extra information from the input</param>
    public void WalkInput(InputAction.CallbackContext Input)
    {
        if (Input.performed)
        {
            _moveInput = true;
            _moveDirection = Input.ReadValue<Vector2>();
            if(_moveDirection.x < 0)
                SpriteRenderer.flipX = true;
            else
                SpriteRenderer.flipX = false;
        }
        else if (Input.canceled)
        {
            _moveInput = false;
            return;
        }
    }

    /// <summary>
    /// Takes a single button or key press and calls a dodge
    /// </summary>
    /// <param name="Input"></param>
    public void DodgeInput(InputAction.CallbackContext Input)
    {
        if(Input.performed && _dodgeCD)
            StartCoroutine(DodgeSequence());

    }

    /// <summary>
    /// Performs the dodge
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeSequence()
    {
        _dodgeCD = false;
        float dodgeTime = DodgeLength;
        Vector2 dodgeDir = _moveDirection;

        if (Dashed != null)
            Dashed.Invoke();

        while(dodgeTime > 0)
        {
            RB2D.linearVelocity = dodgeDir * DodgeForce/10;
            yield return new WaitForSecondsRealtime(0.1f);
            dodgeTime -= 0.1f;
        }
        RB2D.linearVelocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(DodgeCooldown * (1 - DashCDReduction / 100f));

        _dodgeCD = true;
    }

    /// <summary>
    /// Performs movement actions on update
    /// </summary>
    private void MovePlayer()
    {
        if (!_moveInput)
            return;

        RB2D.transform.position += _moveDirection * ((WalkSpeed + MoveSpeedBoost) * _speedBuffMultiplier) * Time.deltaTime;

    }

    private float _speedBuffMultiplier = 1f;
    private float _speedBuffTimer = 0f;

    public void AddSpeedBuff(float boostStrength, float duration)
    {
        _speedBuffMultiplier = boostStrength;
        _speedBuffTimer = duration;

    }

    private void SpeedBuffHandler()
    {
        if (_speedBuffTimer > 0)
        {
            _speedBuffTimer -= Time.deltaTime;
        }
        else
        {
            _speedBuffMultiplier = 1f;
        }
    }

}
