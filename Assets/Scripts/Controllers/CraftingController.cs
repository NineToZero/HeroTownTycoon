using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    private Dictionary<int, int> _cachedItems;
    private Inventory _inventory;

    // for ui
    public event Action OnCacheInventoryEvent;
    public event Action OnChangeCraftingCountEvent;

    public int CraftingCount { get { return _craftingCount; } set { _craftingCount = value; OnChangeCraftingCountEvent?.Invoke(); } }
    private int _craftingCount;

    public void Init(Inventory inventory)
    {
        _cachedItems = new();
        _inventory = inventory;
    }

    public void CacheInventory()
    {
        _cachedItems.Clear();

        foreach (Slot slot in _inventory.Slots)
        {
            if (slot.id != 0)
            {
                if (_cachedItems.ContainsKey(slot.id))
                {
                    _cachedItems[slot.id] += slot.curStack;
                }
                else _cachedItems.Add(slot.id, slot.curStack);
            }
        }

        OnCacheInventoryEvent?.Invoke();
    }

    public void TryCraft(RecipeData recipe)
    {
        for(int i = 0; i < _craftingCount; i++)
        {
            bool canCraft = true;

            foreach (ItemIdQuantityPair reduction in recipe.Inputs)
            {
                if (!(_cachedItems.ContainsKey(reduction.ItemId) && (_cachedItems[reduction.ItemId] >= reduction.Quantity)))
                {
                    canCraft = false;
                }
            }

            if (canCraft)
            {
                bool ableAdd = _inventory.TryAdd(recipe.Output.ItemId, recipe.Output.Quantity, out int remain);
                if (ableAdd)
                {
                    foreach (var reduction in recipe.Inputs)
                    {
                        _inventory.Remove(reduction.ItemId, reduction.Quantity);
                    }
                    CacheInventory();
                }
                else
                {
                    Managers.Instance.ItemManager.SpawnCollectable(recipe.Output.ItemId, transform.position, remain);
                }

                Managers.Instance.UIManager.ShowNotificationUI("아이템을 성공적으로 제작했습니다.");
                Managers.Instance.SoundManager.PlaySFX(SFXSource.Craft);
            }
        }
    }

    public int GetQuantity(int itemId)
    {
        return _cachedItems.TryGetValue(itemId, out int quantity) ? quantity : 0;
    }
}
