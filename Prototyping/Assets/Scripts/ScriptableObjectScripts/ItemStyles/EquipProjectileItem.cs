using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Styles/EquipProjectileItem")]
public class EquipProjectileItem : ItemData
{

    private enum Slot
    {
        Primary,
        Secondary
    }


    [Header("EquipProjectileItem")]
    [SerializeField] Projectile ProjectileToEquip;
    [SerializeField] Slot EquipInSlot;



    public override void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default)
    {
        switch (EquipInSlot)
        {
            case Slot.Primary:
                player.GetComponent<PlayerShooting>().Primary = ProjectileToEquip;
                break;
            case Slot.Secondary:
                player.GetComponent<PlayerShooting>().Secondary = ProjectileToEquip;
                break;

        }
    }
}
