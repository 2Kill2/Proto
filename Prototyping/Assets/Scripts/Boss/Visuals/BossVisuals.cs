using System.Diagnostics;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform gfx;
    [SerializeField] float faceThreshold = 0.05f;

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
    public void TriggerImpact() { if (anim) anim.SetTrigger("DoImpact"); }
    public void TriggerHurt() { if (anim) anim.SetTrigger("Hurt"); }
    public void TriggerDeath() { if (anim) anim.SetTrigger("Dead"); }

    public void SetHidden(bool hidden)
    {
        var renders = gfx ? gfx.GetComponentsInChildren<Renderer>(true) : GetComponentsInChildren<Renderer>(true);
        foreach (var r in renders) r.enabled = !hidden;
    }
}
