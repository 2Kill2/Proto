using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireDragon : BossBase
{
    [Header("FireDragon")]
    [SerializeField] private bool lockToCenter = true;
    [SerializeField] private Vector2 fixedPosition;
    [SerializeField] private bool faceTarget = true;
    [SerializeField] private float faceLerpSpeed = 15f;

    [Tooltip("Optional: rotate this child toward target. If null, rotates the root.")]
    [SerializeField] private Transform gfx;

    protected override void Awake()
    {
        base.Awake();
        if (lockToCenter)
            fixedPosition = (Vector2)transform.position;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Rb.gravityScale = 0f;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rb.linearVelocity = Vector2.zero;
        Rb.angularVelocity = 0f;

        if (lockToCenter)
            Rb.position = fixedPosition;
    }
    private void FixedUpdate()
    {
        // Keep dragon pinned
        if (lockToCenter)
        {
            if ((Rb.position - fixedPosition).sqrMagnitude > 0.000001f)
                Rb.MovePosition(fixedPosition);
        }
    }
    private void Update()
    {
        if (!faceTarget) return;

        // Face the current target (use projectile origin for more accuracy)
        Vector2 origin = ProjectileOrigin ? (Vector2)ProjectileOrigin.position : Rb.position;
        var targetTf = GetCurrentTarget("Player");
        if (!targetTf) return;

        Vector2 to = (Vector2)targetTf.position - origin;
        if (to.sqrMagnitude < 0.00001f) return;

        float targetAngle = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg;
        float currentZ = (gfx ? gfx.eulerAngles.z : transform.eulerAngles.z);
        float smoothZ = Mathf.LerpAngle(currentZ, targetAngle, 1f - Mathf.Exp(-faceLerpSpeed * Time.deltaTime));

        if (gfx) gfx.rotation = Quaternion.Euler(0f, 0f, smoothZ);
        else transform.rotation = Quaternion.Euler(0f, 0f, smoothZ);
    }
    protected override BossMovement ChooseMovement() => null;
}
