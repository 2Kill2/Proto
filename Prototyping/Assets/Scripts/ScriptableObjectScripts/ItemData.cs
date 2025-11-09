using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    public enum TriggerEvents
    {
        Passive = 0,
        PrimaryFire = 1,
        SecondaryFire = 2,
        Healed = 3,
        Damaged = 4,
        Dashed = 5
    }

    [Header("Item")]
    [SerializeField] private string Name;
    [SerializeField] private string Description;
    [SerializeField] private Sprite Icon;
    [SerializeField] private int Price;


    [Header("Data")]

    [SerializeField] private TriggerEvents TriggerEvent;
    
    //Getters
    public TriggerEvents Event => TriggerEvent;
    public string itemName => Name;
    public string description => Description;
    public Sprite Sprite => Icon;
    public int price => Price;

    public abstract void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default);
    
}
