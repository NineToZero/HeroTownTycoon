using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
    [SerializeField] private TextMeshProUGUI _quantity;
    [SerializeField] private Image _slot;

    public Slot observedSlot;
    private EventEntrySO _entries;

    public void Init(Slot slot)
    {
        observedSlot = slot;

        SetSprite(slot.id);
        SetQuantity(slot.curStack);

        slot.ItemChanged += SetSprite;
        slot.ItemCountChanged += SetQuantity;

        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);
    }

    public void SetSlotColor()
    {
        if (ReferenceEquals(_slot, null)) return;
        _slot.color = Color.gray;
    }
    public void ResetSlotColor()
    {
        if (ReferenceEquals(_slot, null)) return;
        _slot.color = Color.white;
    }
    public void SetSprite(int itemId)
    {
        _icon.sprite = Managers.Instance.DataManager.GetItemSprites(itemId);
    }

    public void SetQuantity(int count)
    {
        _quantity.text = count <= 1 ? "" : count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _slot.color = Color.gray;
        _entries?.Publish(EventTriggerType.PointerEnter, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _slot.color = Color.white;
        _entries?.Publish(EventTriggerType.PointerEnter, null);
        Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
    }

    private void OnDisable()
    {
        if(_slot.color == Color.gray)
            _slot.color = Color.white;
    }

    public void Refresh(Slot slot)
    {
        observedSlot = slot;
        SetSprite(slot.id);
        SetQuantity(slot.curStack);


        // Initialize
        slot.ItemChanged -= SetSprite;
        slot.ItemCountChanged -= SetQuantity;

        slot.ItemChanged += SetSprite;
        slot.ItemCountChanged += SetQuantity;
    }
}
