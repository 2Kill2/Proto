using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EngulfAttack", menuName = "Bosses/Attacks/Engulf")]
public class EngulfAttack : BossAttack
{
    [Header("Engulf")]
    public float expandMultiplier = 1.8f;
    public float expandDuration = 0.25f;
    public float holdDuration = 1.25f;
    public float shrinkDuration = 0.25f;

    [Header("Catch")]
    public float captureRadius = 1.6f;
    public string playerTag = "Player";
    public float ejectForce = 10f;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var col = boss.GetComponent<Collider2D>();
        var rb = boss.Rb;
        if (!col) yield break;

        //save state
        bool originalTrigger = col.isTrigger;
        Vector3 originalScale = boss.transform.localScale;

        //expand + become trigger
        col.isTrigger = true;
        yield return ScaleOverTime(boss.transform, originalScale, originalScale * expandMultiplier, expandDuration);

        //try to capture
        Transform player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        Transform captured = null;

        float endHold = Time.time + holdDuration;
        while (Time.time < endHold)
        {
            if (captured == null && player != null)
            {
                if (Vector2.Distance(boss.transform.position, player.position) <= captureRadius)
                {
                    //Trap Player - stops movement
                    captured = player;
                    captured.SetParent(boss.transform);
                    captured.gameObject.SendMessage("SetStunned", true, SendMessageOptions.DontRequireReceiver);

                    //Place player in center
                    captured.position = boss.transform.position;
                    // stop player velocity if they have RB
                    var prb = captured.GetComponent<Rigidbody2D>();
                    if (prb) prb.linearVelocity = Vector2.zero;
                }
            }
            yield return null;
        }

        //Release
        if (captured != null)
        {
            var prb = captured.GetComponent<Rigidbody2D>();
            captured.gameObject.SendMessage("SetStunned", false, SendMessageOptions.DontRequireReceiver);
            captured.SetParent(null);
            if (prb) prb.AddForce(Vector2.up * ejectForce, ForceMode2D.Impulse);
        }

        //Scale Boss back to normal size and restor boss collider
        yield return ScaleOverTime(boss.transform, boss.transform.localScale, originalScale, shrinkDuration);
        col.isTrigger = originalTrigger;

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }

    private IEnumerator ScaleOverTime(Transform t, Vector3 from, Vector3 to, float secs)
    {
        if (secs <= 0f) { t.localScale = to; yield break; }
        float s = 0f;
        while (s < 1f)
        {
            s += Time.deltaTime / secs;
            t.localScale = Vector3.Lerp(from, to, Mathf.Clamp01(s));
            yield return null;
        }
    }
}
