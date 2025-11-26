using System.Collections;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    [Header("Damage Flash")]
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private Color flashColor = Color.white;

    [SerializeField] Animator anim;
    [SerializeField] Transform gfx;
    [SerializeField] float faceThreshold = 0.05f;

    private bool flashing = false;
    private Color[] originalColors;
    private SpriteRenderer[] spriteRenderers;
    BossBase boss;
    Vector2 lastPos;

    void Awake()
    {
        boss = GetComponentInParent<BossBase>();
        if (!anim && gfx) anim = gfx.GetComponent<Animator>();
        lastPos = boss ? boss.Rb.position : (Vector2)transform.position;
    }

    void Update()
    {
        if (!boss || !anim) return;

        Vector2 pos = boss.Rb.position;
        float speed = ((pos - lastPos).magnitude) / Mathf.Max(Time.deltaTime, 0.0001f);
        anim.SetFloat("Speed", speed);
        anim.SetBool("IsMoving", speed > 0.05f);

        float dx = pos.x - lastPos.x;
        if (Mathf.Abs(dx) > faceThreshold && gfx)
        {
            var s = gfx.localScale;
            s.x = Mathf.Sign(dx) * Mathf.Abs(s.x);
            gfx.localScale = s;
        }

        lastPos = pos;
    }

    public void SetTracking(bool v) { if (anim) anim.SetBool("IsTracking", v); }
    public void SetTelegraph(bool v) { if (anim) anim.SetBool("IsTelegraph", v); }
    public void SetJump(bool v) { if (anim) anim.SetBool("IsJump", v); }
    public void SetBreath(bool v) { if (anim) anim.SetBool("FireBreath", v); }
    public void SetMoving(bool v) { if (anim) anim.SetBool("IsMoving", v); }
    public void TriggerImpact() { if (anim) anim.SetTrigger("DoImpact"); }
    public void TriggerCast() { if (anim) anim.SetTrigger("Cast"); }
    
    public void TriggerHurt()
    {
        if (anim) anim.SetTrigger("Hurt");
        DoDamageFlash();
    }
    public void TriggerDeath() { if (anim) anim.SetTrigger("Dead"); }

    public void SetHidden(bool hidden)
    {
        var renders = gfx ? gfx.GetComponentsInChildren<Renderer>(true) : GetComponentsInChildren<Renderer>(true);
        foreach (var r in renders) r.enabled = !hidden;
    }

    public void DoDamageFlash()
    {
        if (!flashing)
            StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator DamageFlashRoutine()
    {
        flashing = true;

        if (spriteRenderers == null)
        {
            spriteRenderers = gfx.GetComponentsInChildren<SpriteRenderer>();
            originalColors = new Color[spriteRenderers.Length];
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
            originalColors[i] = spriteRenderers[i].color;

        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].color = originalColors[i];

        flashing = false;
    }
}
