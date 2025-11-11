using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Styles/StatUpgradeItem")]
public class StatUpgradeItem : ItemData
{

    private enum Stats
    {
        MaxHealth,
        DamageReduction,
        MoveSpeed,
        DashCooldown,
    }

    [Header("StatUpgradeItem")]
    [SerializeField] Stats UpgradeStat;
    [SerializeField] float Amount;

   
   



    public override void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default)
    {
        switch (UpgradeStat)
        {
            case Stats.MaxHealth:
                player.GetComponent<Health>().BonusMaxHealth += Amount;
                break;
            case Stats.DamageReduction:
                player.GetComponent<Health>().DamageReduction += Amount;
                break;
            case Stats.MoveSpeed:
                player.GetComponent<PlayerMovement>().MoveSpeedBoost += Amount;
                break;
            case Stats.DashCooldown:
                player.GetComponent<PlayerMovement>().DashCDReduction += Amount;
                break;

        }

    }
}
