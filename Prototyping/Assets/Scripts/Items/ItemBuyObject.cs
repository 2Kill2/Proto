using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ItemBuyObject : MonoBehaviour
{
    private Item _storedItem;

    //Display info using item.Data
    [SerializeField] Canvas InfoDisplay;

    public void ItemToStore(Item item, int price)
    {
        _storedItem = item;
    }

    /// <summary>
    /// Called when interacted by a player
    /// </summary>
    private void Interact(GameObject player)
    {
        //Buy logic
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
