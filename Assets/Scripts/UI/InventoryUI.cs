using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : BaseUI
{
    [SerializeField] public SlotsContainterUI slotsContainterUI;
    [SerializeField] protected TMP_Text _playerGold;

    protected EventEntrySO _entries;

    public void Init(InventoryController inventoryController,bool setActive = false)
    {
        Init(inventoryController, inventoryController.Inventory.Slots.Count, setActive);
    }
    
    public void Init(InventoryController inventoryController, int count, bool setActive = false)
    {
        slotsContainterUI.Init(inventoryController.Inventory, count);
        gameObject.SetActive(setActive);

        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);

        inventoryController.OnChangeGold += RefreshGoldText;
    }

    public override void Off()
    {
        base.Off();

        _entries.Publish(EventTriggerType.PointerExit, CursorSource.None);
    }

    public void RefreshGoldText(int currentGold)
    {
        if (ReferenceEquals(_playerGold, null)) return;
        _playerGold.text = currentGold.ToString();
    }
}
