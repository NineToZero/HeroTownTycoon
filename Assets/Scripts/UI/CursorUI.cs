using UnityEngine;
using UnityEngine.UI;

public class CursorUI : BaseUI
{
    CursorSlotController _controller;
    [SerializeField] private SlotsContainterUI slotsContainterUI;

    public void Init(CursorSlotController controller, Inventory inventory)
    {
        slotsContainterUI.Init(inventory, 1);
        gameObject.SetActive(false);

        _controller = controller;
        SlotUI slotUi = slotsContainterUI.Slots[0];
        Image slotImage = slotUi.GetComponent<Image>();
        slotImage.raycastTarget = false;
        slotImage.sprite = Managers.Instance.DataManager.GetSO<ImageSO>(Const.SO_NullImageSO).Images[0].SpriteList[0];
    }

    public override void Off()
    {
        base.Off();
        _controller.OnOffCursorAction();
    }
}
