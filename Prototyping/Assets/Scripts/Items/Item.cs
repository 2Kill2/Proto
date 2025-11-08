using UnityEngine;


public abstract class Item : MonoBehaviour
{
    public ItemData Data;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = Data.Sprite;
        AssignToEvent();
    }
    private void Update()
    {
        PassiveEffect();
    }

    /// <summary>
    /// What event does this listen to?
    /// </summary>
    private void AssignToEvent()
    {
        switch (Data.Event)
        {
            case(ItemData.TriggerEvents.PrimaryFire):
                GetComponent<PlayerShooting>().PrimaryShot += Effect;
                break;
            case(ItemData.TriggerEvents.SecondaryFire):
                GetComponent<PlayerShooting>().SecondaryShot += Effect;
                break;
            case(ItemData.TriggerEvents.Healed):
                GetComponent<Health>().Healed += Effect;
                break;
            case (ItemData.TriggerEvents.Damaged):
                GetComponent<Health>().Damaged += Effect;
                break;
        }
    }

    /// <summary>
    /// Triggered by an event or custom action
    /// </summary>
    protected abstract void Effect();
    protected virtual void PassiveEffect()
    {
        //Optional field
    }
    


}
