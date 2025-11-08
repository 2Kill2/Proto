using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ItemBuyObject : MonoBehaviour
{
    private Item StoredItem;
    [SerializeField] Canvas InfoDisplay;

    public void ItemToStore(Item item, int price)
    {
        StoredItem = item;
    }

    private void Interact()
    {
        Debug.Log("Interacted");
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
