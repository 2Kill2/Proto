using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ExtraArcItem")]
public class ExtraArcItem : ItemData
{
    [Header("Extra Shot Item")]
    [Range(1,100)]
    [SerializeField] float TriggerPercentage;
    [SerializeField] Projectile Projectile;
    [SerializeField] int ProjectileCount;
    [SerializeField] float ArcAngle;


    public override void Trigger(Vector3 pos = default, float rotation = default)
    {
        bool success = Random.value < TriggerPercentage/100;

        if (success)
            ProjectileManager.Instance.ShootProjectilesInArc(Projectile, pos,ProjectileCount,ArcAngle,rotation);
    }
}
