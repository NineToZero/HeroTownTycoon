using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopPopupUI : PopupUI
{
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private TMP_InputField _countInputField;

    private InventoryController _controller;
    private Slot _cursorSlot;
    private BaseItemData _item;
    private int _count;
    private int _price;

    public void Init(InventoryController controller)
    {
        _controller = controller;

        _countInputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
    }
    public void SetSlot(Slot slot)
    {
        //Initial Setting
        _cursorSlot = slot;
    }
    public override void On()
    {
        base.On();

        _count = 1;
        _iconImage.sprite = Managers.Instance.DataManager.GetItemSprites(_item.Id);
        _countInputField.SetTextWithoutNotify(_count.ToString());
        _headText.text = _item.Name;
        _bodyText.text = _price.ToString();
    }

    public UnityAction<string> SetCount(CursorSource source)
    {
        return (text) =>
        {
            if (string.IsNullOrEmpty(text))
            {
                _count = 0;
            }
            else
            {
                _count = Convert.ToInt32(text);

                if (source == CursorSource.Inventory)
                {
                    if (_count * _price > _controller.Gold) _count = _controller.Gold / _price;
                    else if (_count <= 0) _count = 0;
                }
                else
                {
                    if (_count > _cursorSlot.curStack) _count = _cursorSlot.curStack;
                    else if (_count <= 0) _count = 1;
                }

                _countInputField.SetTextWithoutNotify(_count.ToString());
                _bodyText.text = (_count * _price).ToString();
            }
        };
        
    }

    public void ShowPopupUI(CursorSource shopType)
    {
        _item = Managers.Instance.DataManager.Items[_cursorSlot.id];

        _countInputField.onValueChanged.RemoveAllListeners();
        _countInputField.onValueChanged.AddListener(SetCount(shopType));
        switch (shopType)
        {
            case CursorSource.Inventory:
                _price = _item.Price;
                Managers.Instance.UIManager.ShowPopupUI<ShopPopupUI>(Buy, _item.Name, _price.ToString(), Cancel, "구매", "취소");
                break;
            case CursorSource.Shop:
                _price = (int)(_item.Price * 0.8f + 0.5f);
                Managers.Instance.UIManager.ShowPopupUI<ShopPopupUI>(Sell, _item.Name, _price.ToString(), Cancel, "판매", "취소");
                break;
        }
    }

    public void Sell()
    {
        Managers.Instance.SoundManager.PlaySFX(SFXSource.Coin);
        _cursorSlot.RemoveItem(_count);
        _controller.Gold += _price * _count;
        Managers.Instance.UIManager.CloseUI<CursorUI>();
        Managers.Instance.UIManager.CloseUI<ShopPopupUI>();
    }
    public void Buy()
    {
        if (_controller.Gold >= _item.Price * _count)
        {
            _controller.Inventory.TryAdd(_item.Id, _count, out int remain);
            
            _controller.Gold -= _item.Price * (_count - remain);
            Managers.Instance.SoundManager.PlaySFX(SFXSource.Coin);
        }
        Managers.Instance.UIManager.CloseUI<CursorUI>();
        Managers.Instance.UIManager.CloseUI<ShopPopupUI>();
    }
    public void Cancel()
    {
        Managers.Instance.UIManager.CloseUI<ShopPopupUI>();
        Managers.Instance.UIManager.CloseUI<CursorUI>();
    }
}
