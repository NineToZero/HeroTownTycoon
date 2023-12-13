using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlotContainerUI : SlotsContainterUI
{
    private List<ShopSlotUI> _shopSlots;

    private void Awake()
    {
        _shopSlots = new List<ShopSlotUI>();
    }

    public new void Init(Inventory inventory, int count)
    {
        foreach (var slotUI in _shopSlots) slotUI.gameObject.SetActive(false);

        if (count > _shopSlots.Count)
        {
            ShopSlotUI slotUIPrefab = Managers.Instance.DataManager.GetPrefab<ShopSlotUI>(Const.Prefabs_ShopSlotUI);
            for (int i = _shopSlots.Count; i < count; ++i)
            {
                ShopSlotUI shopSlotUI = Instantiate(slotUIPrefab, transform);
                _shopSlots.Add(shopSlotUI);
            }
        }

        for (int i = 0; i < count; ++i)
        {
            _shopSlots[i].Init(inventory.Slots[i], gameObject.GetComponent<RectTransform>());
            _shopSlots[i].gameObject.SetActive(true);
        }
    }

}