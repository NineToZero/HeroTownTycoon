using UnityEngine;

public class StorageUI : InventoryUI
{
    [SerializeField] private InventoryUI _subInventory;
    private Storage _observedStorage;

    public void Init(InventoryController inventoryController)
    {
        slotsContainterUI.Init(new Inventory(60), 60);

        _subInventory.Init(inventoryController, true);

        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);
    }

    public override void On()
    {
        base.On();

        slotsContainterUI.Refresh(_observedStorage.StorageInventory);
    }

    public void SetStorage(Storage storage)
    {
        _observedStorage = storage;
    }
}