using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ShopSpawner : MonoBehaviour
{
    [Header("Loot Tables")]
    [SerializeField] private ShopItemTable EarlyGameDrops;
    [SerializeField] private ShopItemTable MidGameDrops;
    [SerializeField] private ShopItemTable LateGameDrops;

    [Header("Shops")]
    [SerializeField] private ItemBuyObject ShopPrefab;
    private readonly List<ItemBuyObject> _shops = new();
    private GameManager _gameManager;
    [SerializeField] LayoutGroup grid;
    private void Start()
    {
        _gameManager = GameManager.instance;
        NewShop();
    }

    public void NewShop()
    {
        if (_gameManager == null)
            _gameManager = GameManager.instance;

        if (_shops.Count > 0)
        {
            foreach (var shop in _shops)
                if(shop != null)
                Destroy(shop.gameObject);

            
        }

        ShopItemTable chosenTable = _gameManager.BossesKilled switch
        {
            <= 3 => EarlyGameDrops,
            > 3 and <= 6 => MidGameDrops,
            > 6 => LateGameDrops
            
        };

        SpawnShop(chosenTable);
    }

    private void SpawnShop(ShopItemTable table)
    {
        if (table == null) return;

        for (int i = 0; i < table.Size; i++)
        {
            ItemBuyObject newShop = Instantiate(ShopPrefab, transform);
            ItemData item = GetRandomFromTable(table);
            newShop.ItemToStore(item);
            _shops.Add(newShop);
        }
        grid.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid.GetComponent<RectTransform>());
        grid.enabled = false;
    }

    private ItemData GetRandomFromTable(ShopItemTable table)
    {
        var items = table.Table;
        if (items == null || items.Length == 0)
            return null;

        float totalWeight = 0f;
        foreach (var item in items)
            totalWeight += item.weight;

        if (totalWeight <= 0f)
            return null;

        float randomValue = Random.Range(0, totalWeight);

        foreach (var item in items)
        {
            randomValue -= item.weight;
            if (randomValue <= 0)
                return item;
        }

        return items[^1];
    }
}
