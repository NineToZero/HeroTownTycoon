using UnityEngine;

public class ShopUI : InventoryUI
{
    private InventoryController inventoryController;
    
    [SerializeField] private ShopSlotContainerUI _shopSlotContainer;
    [SerializeField] private InventoryUI _subInventory;

    public void Init(InventoryController inventoryController)
    {
        this.inventoryController = inventoryController;
        
        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);

        _subInventory.Init(inventoryController, true);
    }

    public void SetShopItems(BuildingType buildingType)
    {
        inventoryController.SetShopItems(buildingType);
        _shopSlotContainer.Init(inventoryController.ShopInventory, inventoryController.ShopInventory.Slots.Count);
    }
}