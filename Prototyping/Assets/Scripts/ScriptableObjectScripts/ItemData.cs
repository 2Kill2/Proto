using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Create Data Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum TriggerEvents
    {
        Passive = 0,
        PrimaryFire = 1,
        SecondaryFire = 2,
        Healed = 3,
        Damaged = 4,
    }

    [Header("Item")]
    [SerializeField] private string Name;
    [SerializeField] private string Description;
    [SerializeField] private Sprite Icon;


    [Header("Data")]

    [SerializeField] private TriggerEvents TriggerEvent;
    
    //Getters
    public TriggerEvents Event => TriggerEvent;
    public string itemName => Name;
    public string description => Description;
    public Sprite Sprite => Icon;
}
