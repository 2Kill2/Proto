using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Styles/ExtraShotItem")]
public class ExtraShotItem : ItemData
{
    [Header("ExtraShotItem")]
    [Range(1, 100)]
    [SerializeField] float TriggerPercentage;
    [SerializeField] Projectile Projectile;
    [SerializeField] int ProjectileCount;
    [SerializeField] float ArcAngle;


    public override void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default)
    {
        bool success = Random.value < TriggerPercentage / 100;

        if (success)
        {
            switch (Projectile.Data.fireType)
            {
                case ProjectileData.FireMode.Single:
                    ProjectileManager.Instance.ShootProjectileFromPosition(Projectile, pos, rotation);
                    break;
                case ProjectileData.FireMode.Arc:
                    ProjectileManager.Instance.ShootProjectilesInArc(Projectile, pos, ProjectileCount, ArcAngle, rotation);
                    break;
            }
        }

    }
}
