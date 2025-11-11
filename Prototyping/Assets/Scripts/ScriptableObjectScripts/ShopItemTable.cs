using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemTable", menuName = "Create Data Object/ShopItemTable")]
public class ShopItemTable : ScriptableObject
{
    [Tooltip("Drop chances come from the RarityWeight of the ItemData object")]
    [SerializeField] ItemData[] ItemsInTable;
    [SerializeField] int ShopSize;
    public ItemData[] Table => ItemsInTable;
    public int Size => ShopSize;
}
