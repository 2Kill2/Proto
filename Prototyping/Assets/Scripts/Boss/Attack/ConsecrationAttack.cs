using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsecrationAttack", menuName = "Bosses/Attacks/Consecration")]
public class ConsecrationAttack : BossAttack
{
    [Header("Placement")]
    public float minDistanceFromBoss = 2f;
    public float maxPlacementTries = 10;

    [Header("Telegraph")]
    public GameObject telegraphPrefab;
    public float telegraphTime = 0.8f;
    public float circleRadius = 2.5f;

    [Header("Damage Zone")]
    public GameObject zonePrefab;
    public float zoneDuration = 4f;
    public float tickInterval = 0.5f;
    public int damagePerTick = 4;
    public LayerMask playerLayers;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();

        Vector2 bossPos = boss.transform.position;
        Vector2 center = bossPos;

        var bounds = boss.GetArenaBounds();
        int tries = 0;
        while (tries < maxPlacementTries)
        {
            Vector2 p = boss.RandomPointInsideArena();
            if (Vector2.Distance(p, bossPos) >= minDistanceFromBoss)
            {
                center = p;
                break;
            }
            tries++;
        }

        GameObject telegraph = null;
        if (telegraphPrefab != null)
        {
            telegraph = Object.Instantiate(telegraphPrefab, center, Quaternion.identity);
            float d = circleRadius * 2f;
            telegraph.transform.localScale = new Vector3(d, d, 1f);
        }

        vis?.SetTelegraph(true);
        yield return new WaitForSeconds(telegraphTime);
        vis?.SetTelegraph(false);

        if (telegraph != null) Object.Destroy(telegraph);

        GameObject zone = null;
        if (zonePrefab != null)
        {
            zone = Object.Instantiate(zonePrefab, center, Quaternion.identity);
            float d = circleRadius * 2f;
            zone.transform.localScale = new Vector3(d, d, 1f);
        }

        float elapsed = 0f;
        if (tickInterval <= 0f) tickInterval = 0.25f;

        while (elapsed < zoneDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, circleRadius, playerLayers);

            foreach (var h in hits)
            {
                h.SendMessage("TakeDamage", damagePerTick, SendMessageOptions.DontRequireReceiver);
            }

            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        if (zone != null) Object.Destroy(zone);

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}
