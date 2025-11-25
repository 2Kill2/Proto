using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BonePrisonAttack", menuName = "Bosses/Attacks/Bone Prison")]
public class BonePrisonAttack : BossAttack
{
    [Header("Prefabs")]
    public GameObject telegraphPrefab; //Dark Circle prefab or a rune
    public GameObject prisonPrefab;    //bone spikes

    [Header("Timings")]
    public float telegraphTime = 0.7f;
    public float prisonActiveTime = 2.5f;
    public float damageTickInterval = 0.5f;

    [Header("Damage")]
    public float prisonRadius = 2.0f;
    public LayerMask playerLayers;
    public int damagePerTick = 4;

    [Header("Optional Scale")]
    public Vector2 telegraphScaleRange = new Vector2(1f, 1f);

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();
        Transform target = boss.GetCurrentTarget();

        Vector2 centerPos = target ? (Vector2)target.position : (Vector2)boss.transform.position;

        GameObject telegraph = null;
        if (telegraphPrefab)
        {
            telegraph = Object.Instantiate(telegraphPrefab, centerPos, Quaternion.identity);
            float scale = Random.Range(telegraphScaleRange.x, telegraphScaleRange.y);
            telegraph.transform.localScale = new Vector3(scale, scale, 1f);
        }

        vis?.SetTelegraph(true);
        yield return new WaitForSeconds(telegraphTime);
        vis?.SetTelegraph(false);

        //Replace telegraph with bone prison
        if (telegraph) Object.Destroy(telegraph);

        GameObject prison = null;
        if (prisonPrefab)
        {
            prison = Object.Instantiate(prisonPrefab, centerPos, Quaternion.identity);
        }

        vis?.TriggerCast();

        float elapsed = 0f;
        if (damageTickInterval <= 0f) damageTickInterval = 0.3f;
        while (elapsed < prisonActiveTime)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(centerPos, prisonRadius, playerLayers);

            foreach (var h in hits)
            {
                h.SendMessage("TakeDamage", damagePerTick, SendMessageOptions.DontRequireReceiver);
            }

            elapsed += damageTickInterval;
            yield return new WaitForSeconds(damageTickInterval);
        }

        if (prison) Object.Destroy(prison);

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}
