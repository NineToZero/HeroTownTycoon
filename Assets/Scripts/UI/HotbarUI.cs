using UnityEngine;

public class HotbarUI : BaseUI
{
    [SerializeField] protected SlotsContainterUI slotsContainterUI;

    protected Inventory _inventory;

    public int HandedSlotIndex = 0;

    public void Init(InventoryController inventoryController)
    {
        _inventory = inventoryController.Inventory;
        slotsContainterUI.Init(inventoryController.Inventory, 10);
        gameObject.SetActive(true);

        inventoryController.OnChangeSelectedSlot += SetHandedSlot;
    }
    private void Start()
    {
        slotsContainterUI.Slots[HandedSlotIndex].SetSlotColor();
    }
    public void SetHandedSlot(int index)
    {
        slotsContainterUI.Slots[HandedSlotIndex].ResetSlotColor();

        //TO-DO : ��ǲ�� ���� ����ִ� ���� �ٲٱ�
        HandedSlotIndex = index;

        slotsContainterUI.Slots[HandedSlotIndex].SetSlotColor();
    }
}


