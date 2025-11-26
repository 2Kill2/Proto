using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulDrainAttack", menuName = "Bosses/Attacks/Soul Drain")]
public class SoulDrainAttack : BossAttack
{
    [Header("Soul Drain Settings")]
    public float telegraphTime = 0.4f;
    public float channelDuration = 2.0f;
    public float tickInterval = 0.4f;

    [Tooltip("Radius around the lich to drain from.")]
    public float drainRadius = 4f;

    [Tooltip("Layer(s) used to find players.")]
    public LayerMask playerLayers;

    [Tooltip("Damage dealt per tick to each player.")]
    public int damagePerTick = 3;

    [Tooltip("Fraction of total damage converted to healing (e.g., 0.5 = 50%).")]
    public float healMultiplier = 0.5f;

    [Header("Beam Visual")]
    public Material lineMaterial;
    public float lineWidth = 0.05f;
    public Color lineColor = Color.green;

    [Header("Telegraph Circle (code)")]
    public Material telegraphMaterial;
    public float telegraphLineWidth = 0.05f;
    public Color telegraphColor = Color.green;
    public int telegraphSegments = 48;

    public override IEnumerator Execute(BossBase boss)
    {
        if (windUp > 0f) yield return new WaitForSeconds(windUp);

        var vis = boss.GetComponent<BossVisuals>();

        //find origin for the beam
        var lich = boss as WraithLich;
        Transform originTf = lich ? lich.SoulDrainOrigin : null;

        //Telegraph circle
        GameObject telegraphObj = null;
        LineRenderer teleLR = null;

        if (telegraphMaterial != null)
        {
            telegraphObj = new GameObject("SoulDrainTelegraph");
            telegraphObj.transform.SetParent(boss.transform);
            telegraphObj.transform.localPosition = Vector3.zero;

            teleLR = telegraphObj.AddComponent<LineRenderer>();
            teleLR.material = telegraphMaterial;
            teleLR.widthMultiplier = telegraphLineWidth;
            teleLR.loop = true;
            teleLR.useWorldSpace = false;
            teleLR.positionCount = telegraphSegments;

            teleLR.startColor = telegraphColor;
            teleLR.endColor = telegraphColor;

            float step = 2f * Mathf.PI / telegraphSegments;
            for (int i = 0; i < telegraphSegments; i++)
            {
                float a = step * i;
                Vector3 p = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * drainRadius;
                teleLR.SetPosition(i, p);
            }
        }

        vis?.SetTelegraph(true);
        yield return new WaitForSeconds(telegraphTime);
        vis?.SetTelegraph(false);

        if (telegraphObj != null) Object.Destroy(telegraphObj);

        vis?.TriggerCast();

        GameObject lineObj = null;
        LineRenderer lr = null;

        if (lineMaterial != null)
        {
            lineObj = new GameObject("SoulDrainLine");
            lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.material = lineMaterial;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.startColor = lineColor;
            lr.endColor = lineColor;
            lr.useWorldSpace = true;
        }

        float elapsed = 0f;
        if (tickInterval <= 0f) tickInterval = 0.25f;

        while (elapsed < channelDuration)
        {
            int totalDrain = 0;
            Vector2 center = boss.transform.position;

            Transform target = boss.GetCurrentTarget();

            //beam from origin transform (if set) to target, as long as target is in radius
            if (lr != null && target != null && Vector2.Distance(center, target.position) <= drainRadius)
            {
                Vector3 originPos = originTf ? originTf.position : boss.transform.position;
                lr.enabled = true;
                lr.SetPosition(0, originPos);
                lr.SetPosition(1, target.position);
            }
            else if (lr != null)
            {
                lr.enabled = false;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(center, drainRadius, playerLayers);

            foreach (var h in hits)
            {
                h.SendMessage("TakeDamage", damagePerTick, SendMessageOptions.DontRequireReceiver);
                totalDrain += damagePerTick;
            }

            if (totalDrain > 0)
            {
                int healAmount = Mathf.RoundToInt(totalDrain * healMultiplier);
                (boss as WraithLich)?.Heal(healAmount);
            }

            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        if (lineObj != null) Object.Destroy(lineObj);

        if (postAttack > 0f) yield return new WaitForSeconds(postAttack);
    }
}
