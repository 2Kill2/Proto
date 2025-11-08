using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ExtraShotItem")]
public class ExtraShotItem : ItemData
{
    [Header("Extra Shot Item")]
    [Range(1,100)]
    [SerializeField] float TriggerPercentage;
    [SerializeField] Projectile Projectile;



    public override void Trigger(Vector3 pos = default, float rotation = default)
    {
        bool success = Random.value < TriggerPercentage/100;

        if (success)
            ProjectileManager.Instance.ShootProjectileFromPosition(Projectile, pos, rotation);
    }
}
