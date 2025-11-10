using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    public enum TriggerEvents
    {
        OnPickup = 0,
        PrimaryFire = 1,
        SecondaryFire = 2,
        Healed = 3,
        Damaged = 4,
        Dashed = 5,
        OnTimer = 6,
    }

    [Header("Item")]
    [SerializeField] private string Name;
    [SerializeField] private string Description;
    [SerializeField] private Sprite Icon;
    [SerializeField] private int Price;

    [Range(1,10)]
    [SerializeField] private float RarityWeight;

    [Header("Data")]

    [SerializeField] private TriggerEvents TriggerEvent;
    
    //Getters
    public TriggerEvents Event => TriggerEvent;
    public string itemName => Name;
    public string description => Description;
    public Sprite Sprite => Icon;
    public int price => Price;
    public float weight => RarityWeight;
    public abstract void Trigger(Vector3 pos = default, float rotation = default, GameObject player = default);
    
}
