using UnityEngine;


public class Item : MonoBehaviour
{
    public ItemData Data;

    private PlayerShooting shooting;
    private void Start()
    {

        shooting = GetComponent<PlayerShooting>();
        AssignToEvent();
    }
   

    /// <summary>
    /// What event does this listen to?
    /// </summary>
    private void AssignToEvent()
    {
        switch (Data.Event)
        {
            case ItemData.TriggerEvents.PrimaryFire:
                GetComponent<PlayerShooting>().PrimaryShot += Effect;
                break;
            case ItemData.TriggerEvents.SecondaryFire:
                GetComponent<PlayerShooting>().SecondaryShot += Effect;
                break;
            case ItemData.TriggerEvents.Healed:
                GetComponent<Health>().Healed += Effect;
                break;
            case ItemData.TriggerEvents.Damaged:
                GetComponent<Health>().Damaged += Effect;
                break;
            case ItemData.TriggerEvents.Dashed:
                GetComponent<PlayerMovement>().Dashed += Effect;
                break;
            case ItemData.TriggerEvents.OnPickup:
                Effect();
                break;
           
        }
    }

    /// <summary>
    /// Triggered by an event or custom action
    /// </summary>
    protected void Effect()
    {
        Data.Trigger(transform.position,shooting.AimAngle,gameObject);
    }
    
}
