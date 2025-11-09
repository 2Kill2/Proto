using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Styles/HealTriggerItem")]
public class HealTriggerItem : ItemData
{
    [Header("HealTriggerItem")]
    [Range(1,100)]
    [SerializeField] float TriggerPercentage;

   
    [SerializeField] float HealAmount;



    public override void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default)
    {
        bool success = Random.value < TriggerPercentage/100;

        if (success)
            player.SendMessage("Heal", HealAmount);
            
    }
}
