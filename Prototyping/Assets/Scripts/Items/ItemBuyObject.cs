
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(RectTransform))]
public class ItemBuyObject : MonoBehaviour
{
    [SerializeField] private ItemData StoredItem;
    private int Price;


    [Header("UI")]
   
    //Display info using item.Data
    [SerializeField] Canvas InfoDisplay;
    [SerializeField] TextMeshProUGUI DescriptionText;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI PriceText;
    [SerializeField] Image IconImage;

    public void ItemToStore(ItemData item)
    {
        StoredItem = item;
        Price = item.price;

        //DescriptionText.text = item.description;
        //NameText.text = item.name;
        //PriceText.text = Price.ToString() + " Gold";

        //IconImage = item.ItemIcon;
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
