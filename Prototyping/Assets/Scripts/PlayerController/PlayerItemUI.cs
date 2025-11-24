using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUI : MonoBehaviour
{
    [Tooltip("The parent object that holds all item icons")]
    [SerializeField] public GameObject PlayerUI;

    [SerializeField] ItemIcon EmptyItemPrefab;

    // Stores how many of each item we have
    private Dictionary<ItemData, int> Items = new Dictionary<ItemData, int>();

    // Stores the UI object for each item type
    private Dictionary<ItemData, ItemIcon> ItemIcons = new Dictionary<ItemData, ItemIcon>();


    public void AddItem(ItemData item)
    {
        // Increase count or add new item
        if (Items.ContainsKey(item))
        {
            Items[item]++;
            UpdateExistingIcon(item);   // only update text
        }
        else
        {
            Items.Add(item, 1);
            CreateNewIcon(item, 1);    // create one new icon
        }
    }


    private void CreateNewIcon(ItemData data, int amount)
    {
        ItemIcon icon = Instantiate(EmptyItemPrefab, PlayerUI.transform);

        if (icon.Icon != null)
            icon.Icon.sprite = data.ItemIcon;

        if (icon.Count != null)
            icon.Count.text = amount >= 2 ? amount.ToString() : "";

        if (icon.Name != null)
            icon.Name.text = data.name;

        if (icon.Description != null)
            icon.Description.text = data.description;

        ItemIcons.Add(data, icon);
    }



    private void UpdateExistingIcon(ItemData data)
    {
      
        ItemIcon icon = ItemIcons[data];

        
        if (icon.Count != null )
        {
            if (Items[data] >= 2)
                icon.Count.text = Items[data].ToString();
            else
                icon.Count.text = "";

        }
            
    }
}
