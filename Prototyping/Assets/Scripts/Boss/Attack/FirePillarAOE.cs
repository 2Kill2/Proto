using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class FirePillarAOE : MonoBehaviour
{
    [Header("Damage")]
    public LayerMask playerMask;
    public float radius = 0.7f;
    public int eruptDamage = 1;
    public bool doDOT = true;
    public int dotDamagePerTick = 1;
    public float tickInterval = 0.25f;

    [Header("Animation")]
    public Animator animator;
    public string eruptTrigger = "Erupt";
    public string endTrigger = "End";
    public float endAnimLead = 0.15f;

    private Projectile proj;
    private bool running;

    void Awake()
    {
        proj = GetComponent<Projectile>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }


    void OnEnable()
    {
        running = true;

        // eruption
        if (animator && !string.IsNullOrEmpty(eruptTrigger)) animator.SetTrigger(eruptTrigger);

        // one-time burst
        DealDamage(eruptDamage);

        // schedule end anim slightly before lifetime ends (if we can read lifetime)
        if (proj && proj.Data != null && endAnimLead > 0f && proj.Data.lifetime > endAnimLead)
            StartCoroutine(PlayEndAnimLater(proj.Data.lifetime - endAnimLead));

        // DoT loop
        StartCoroutine(DOTLoop());
    }

    void OnDisable()
    {
        running = false;
    }
    private IEnumerator DOTLoop()
    {
        float next = Time.time + tickInterval;
        while (running)
        {
            if (doDOT && Time.time >= next)
            {
                next = Time.time + tickInterval;
                DealDamage(dotDamagePerTick);
            }
            yield return null;
        }
    }
    private IEnumerator PlayEndAnimLater(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (running && animator && !string.IsNullOrEmpty(endTrigger)) animator.SetTrigger(endTrigger);
    }
    private void DealDamage(int amount)
    {
        if (amount <= 0) return;

        var hits = Physics2D.OverlapCircleAll(transform.position, radius, playerMask);
        foreach (var h in hits)
        {
            h.gameObject.SendMessage("Damage", amount, SendMessageOptions.DontRequireReceiver);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
