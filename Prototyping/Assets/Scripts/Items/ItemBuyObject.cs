
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ItemBuyObject : MonoBehaviour
{
    [SerializeField] private ItemData StoredItem;
    [SerializeField] private int Price;
    //Display info using item.Data
    [SerializeField] Canvas InfoDisplay;

    public void ItemToStore(ItemData item)
    {
        StoredItem = item;
        Price = item.price;
    }

    /// <summary>
    /// Called when interacted by a player
    /// </summary>
    private void Interact(GameObject player)
    {
        if (GameManager.instance.Gold < Price) return;


        GameManager.instance.ChangeGold(-Price);

        Item item = player.AddComponent<Item>();
        item.Data = StoredItem;

        Destroy(gameObject);
       
    }

    //Display Tooltips
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InfoDisplay.enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        InfoDisplay.enabled = false;
    }
}
