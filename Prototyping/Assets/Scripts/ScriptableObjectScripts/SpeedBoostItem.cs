using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Styles/SpeedBoostItem")]
public class SpeedBoostItem : ItemData
{
    [Header("Extra Shot Item")]
    [Range(1,100)]
    [SerializeField] float TriggerPercentage;

    [Range(0,5)]
    [SerializeField] float SpeedBoostAmount;

    [SerializeField] float SpeedBoostDuration;



    public override void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default)
    {
        bool success = Random.value < TriggerPercentage/100;

        if (success)
           player.GetComponent<PlayerMovement>().AddSpeedBuff(SpeedBoostAmount,SpeedBoostDuration);
            
    }
}
