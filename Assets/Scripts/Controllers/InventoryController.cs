using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Inventory Inventory;
    public Inventory ShopInventory;

    private int _gold;

    public int Gold 
    { 
        get { return _gold; }
        set 
        {
            _gold = value;
            OnChangeGold?.Invoke(value);
        } 
    }
    private int _selectedIndex;
    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set
        {
            _selectedIndex = value;
            OnChangeSelectedSlot?.Invoke(value);
        }
    }

    public event Action<int> OnChangeGold;
    public event Action<int> OnChangeSelectedSlot;

    private void Awake()
    {
        Inventory = ES3.Load(Const.Save_InventorySaveData, new Inventory(40));
        Managers.Instance.DayManager.DayChangeEvent += OnDayChangeEvent;
    }

    private void Start()
    {
        InitPlayerInventory();
        OnChangeGold?.Invoke(_gold);
    }

    private void InitPlayerInventory()
    {
        Gold = ES3.Load(Const.Save_GoldSaveData, 200);
    }

    public void ClickObject(IClickable clicked)
    {
        Slot itemSlot = GetSelectedSlot();
        clicked.Click(itemSlot.id, out int consumedCount);
        
        switch (Util.GetItemType(itemSlot.id))
        {
            case (ItemType.SeedsType):
                itemSlot.RemoveItem(consumedCount);
                break;
            case (ItemType.FarmingitemType):
                itemSlot.RemoveItem(consumedCount);
                break;
        }
    }

    public Slot GetSelectedSlot()
    {
        return Inventory.Slots[SelectedIndex];
    }

    public void ToggleInventory()
    {
        UIManager ui = Managers.Instance.UIManager;

        if (!ui.ToggleUI<InventoryUI>())
        {
            ui.CloseUI<CursorUI>();
            ui.CloseUI<ItemInfoUI>();
        }
    }

    public void SetShopItems(BuildingType buildingType)
    {
        DataManager dm = Managers.Instance.DataManager;
        int[] shopItems = dm.GetShopItemDatas(buildingType);
        ShopInventory = new Inventory(shopItems.Length);

        foreach (int itemCode in shopItems)
            ShopInventory.Add(itemCode);
    }

    private void OnDayChangeEvent()
    {
        ES3.Save(Const.Save_InventorySaveData, Inventory);
        ES3.Save(Const.Save_GoldSaveData, Gold);
    }
}
