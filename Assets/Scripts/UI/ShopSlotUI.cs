using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlotUI : SlotUI, IScrollHandler
{
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] private TextMeshProUGUI _itemName;

    private RectTransform _scrollviewContent;
    public void Init(Slot slot, RectTransform content)
    {
        base.Init(slot);
        SetSprite(slot.id);
        _itemPrice.text = (Managers.Instance.DataManager.Items[slot.id].Price).ToString();
        _itemName.text = Managers.Instance.DataManager.Items[slot.id].Name;
        _scrollviewContent = content;
    }

    public void OnScroll(PointerEventData eventData)
    {
        _scrollviewContent.anchoredPosition -= eventData.scrollDelta * 3;
    }
} 