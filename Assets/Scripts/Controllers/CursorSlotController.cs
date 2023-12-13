using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorSlotController : MonoBehaviour
{
    private CursorUI _cursorUI;

    [SerializeField] private Slot _cursorSlot;
    [SerializeField] private Slot _targetSlot;

    [SerializeField] private CursorSource _cursorSlotSource;
    [SerializeField] private CursorSource _targetSource;

    private UIManager _uiManager;
    private ShopPopupUI _shopPopupUI;
    private InventoryController _inventoryController;
    private EventEntrySO _entries;

    private bool _isClickable = true;

    private void Awake()
    {
        _uiManager = Managers.Instance.UIManager;
        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);

        _shopPopupUI = _uiManager.GetUI<ShopPopupUI>();
        _cursorUI = _uiManager.GetUI<CursorUI>();

        Inventory cursorInventory = new Inventory(1);
        _cursorSlot = cursorInventory.Slots[0];

        _cursorUI.Init(this, cursorInventory);

        _shopPopupUI.SetSlot(_cursorSlot);
    }

    public void Init(InventoryController inventoryController)
    {
        _inventoryController = inventoryController;

        _entries.Subscribe(EventTriggerType.PointerEnter, (BaseEventData eventData) => SetTargetSlot(eventData));
        _entries.Subscribe(EventTriggerType.PointerEnter, (CursorSource source) => SetTargetSource(source));
        _entries.Subscribe(EventTriggerType.PointerExit, (CursorSource source) => SetTargetSource(source));
        _entries.Subscribe(UIType.Modal, (isOpened) => _isClickable = !isOpened);
    }


    public void ShowShopPopupUI()
    {
        _shopPopupUI.ShowPopupUI(_targetSource);
        Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
    }

    public void OnHotbar(InputValue value)
    {
        if (_cursorSlot?.id != 0 && (_cursorSlotSource == CursorSource.Inventory || _cursorSlotSource == CursorSource.Storage))
        {
            Slot hotbarSlot = _inventoryController.Inventory.Slots[(int)value.Get<float>()];

            SwapSlots(hotbarSlot, _cursorSlot);
        }
        else if (_targetSlot?.id != 0 && (_targetSource == CursorSource.Inventory || _targetSource == CursorSource.Storage))
        {
            Slot hotbarSlot = _inventoryController.Inventory.Slots[(int)value.Get<float>()];

            SwapSlots(_targetSlot, hotbarSlot);
        }
    }

    public void OnClick()
    {
        if (!_isClickable) return;
        switch (_cursorSlotSource)
        {
            case CursorSource.None:
                if (_targetSource == CursorSource.None) return;
                if (ReferenceEquals(_targetSlot, null)) return;
                Managers.Instance.UIManager.OpenUI<CursorUI>();
                Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
                switch (_targetSource)
                {
                    case CursorSource.Inventory:
                        if(_targetSlot.id != 0)
                            InsertSlot(_targetSlot.id, _targetSlot.RemoveItem());
                        break;
                    case CursorSource.Storage:
                        if (_targetSlot.id != 0) 
                            InsertSlot(_targetSlot.id, _targetSlot.RemoveItem());
                        break;
                    case CursorSource.Shop:
                        if (_targetSlot.id != 0)
                            InsertSlot(_targetSlot.id, 1);
                        break;
                }
                break;
            case CursorSource.Inventory:
                switch (_targetSource)
                {
                    case CursorSource.None:
                        ThrowItemFromCursorSlot();
                        break;
                    case CursorSource.Inventory:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, _cursorSlot.curStack);
                            DeleteSlot(_cursorSlot);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            int capacity = _targetSlot.GetCapacity();
                            capacity = capacity > _cursorSlot.curStack ? _cursorSlot.curStack : capacity;

                            _targetSlot.AddItem(_cursorSlot.id, capacity);
                            DeleteSlot(_cursorSlot, capacity);
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Storage:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, _cursorSlot.curStack);
                            DeleteSlot(_cursorSlot);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            int capacity = _targetSlot.GetCapacity();
                            capacity = capacity > _cursorSlot.curStack ? _cursorSlot.curStack : capacity;

                            _targetSlot.AddItem(_cursorSlot.id, capacity);
                            DeleteSlot(_cursorSlot, capacity);
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Shop:
                        ShowShopPopupUI();
                        break;
                    case CursorSource.Hero:
                        if (!(Util.GetItemType(_cursorSlot.id) == ItemType.DishType || Util.GetItemType(_cursorSlot.id) == ItemType.MedicineType)) break;

                        ItemIdQuantityPair item = new() { ItemId = _cursorSlot.id, Quantity = 1 };
                        _entries.Publish(_cursorSlotSource, _targetSource, item);
                        DeleteSlot(_cursorSlot, 1);
                        break;
                }
                break;
            case CursorSource.Storage:
                switch (_targetSource)
                {
                    case CursorSource.None:
                        ThrowItemFromCursorSlot();
                        break;
                    case CursorSource.Inventory:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, _cursorSlot.curStack);
                            DeleteSlot(_cursorSlot);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            int capacity = _targetSlot.GetCapacity();
                            capacity = capacity > _cursorSlot.curStack ? _cursorSlot.curStack : capacity;

                            _targetSlot.AddItem(_cursorSlot.id, capacity);
                            DeleteSlot(_cursorSlot, capacity);
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Storage:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, _cursorSlot.curStack);
                            DeleteSlot(_cursorSlot);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            int capacity = _targetSlot.GetCapacity();
                            capacity = capacity > _cursorSlot.curStack ? _cursorSlot.curStack : capacity;

                            _targetSlot.AddItem(_cursorSlot.id, capacity);
                            DeleteSlot(_cursorSlot, capacity);
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Shop:
                        ShowShopPopupUI();
                        break;
                }
                break;
            case CursorSource.Shop:
                switch (_targetSource)
                {
                    case CursorSource.None:
                        DeleteSlot(_cursorSlot);
                        break;
                    case CursorSource.Inventory:
                        ShowShopPopupUI();
                        break;
                    case CursorSource.Shop:
                        DeleteSlot(_cursorSlot);
                        break;
                }
                break;
        }
    }

    public void OnRightClick()
    {
        if (!_isClickable) return;
        switch (_cursorSlotSource)
        {
            case CursorSource.None:
                if (_targetSource == CursorSource.None) return;
                if (ReferenceEquals(_targetSlot, null)) return;
                Managers.Instance.UIManager.OpenUI<CursorUI>();
                Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
                switch (_targetSource)
                {
                    case CursorSource.Inventory:
                        if (_targetSlot.id != 0)
                            InsertSlot(_targetSlot.id, _targetSlot.RemoveItem((int)(_targetSlot.curStack * 0.5f + 0.5f)));
                        break;
                    case CursorSource.Storage:
                        if (_targetSlot.id != 0)
                            InsertSlot(_targetSlot.id, _targetSlot.RemoveItem((int)(_targetSlot.curStack * 0.5f + 0.5f)));
                        break;
                    case CursorSource.Shop:
                        if (_targetSlot.id != 0)
                            InsertSlot(_targetSlot.id, 1);
                        break;
                }
                break;
            case CursorSource.Inventory:
                switch (_targetSource)
                {
                    case CursorSource.None:
                        ThrowItemFromCursorSlot(1);
                        break;
                    case CursorSource.Inventory:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, 1);
                            DeleteSlot(_cursorSlot, 1);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            if (_targetSlot.GetCapacity() > 1)
                            {
                                _targetSlot.AddItem(_cursorSlot.id, 1);
                                DeleteSlot(_cursorSlot, 1);
                            }
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Storage:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, 1);
                            DeleteSlot(_cursorSlot, 1);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            if (_targetSlot.GetCapacity() > 1)
                            {
                                _targetSlot.AddItem(_cursorSlot.id, 1);
                                DeleteSlot(_cursorSlot, 1);
                            }
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Shop:
                        ShowShopPopupUI();
                        break;
                    case CursorSource.Hero:
                        if (!(Util.GetItemType(_cursorSlot.id) == ItemType.DishType || Util.GetItemType(_cursorSlot.id) == ItemType.MedicineType)) break;

                        ItemIdQuantityPair item = new() { ItemId = _cursorSlot.id, Quantity = 1 };
                        _entries.Publish(_cursorSlotSource, _targetSource, item);
                        DeleteSlot(_cursorSlot, 1);
                        break;
                }
                break;
            case CursorSource.Storage:
                switch (_targetSource)
                {
                    case CursorSource.None:
                        ThrowItemFromCursorSlot(1);
                        break;
                    case CursorSource.Inventory:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, 1);
                            DeleteSlot(_cursorSlot, 1);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            if (_targetSlot.GetCapacity() > 1)
                            {
                                _targetSlot.AddItem(_cursorSlot.id, 1);
                                DeleteSlot(_cursorSlot, 1);
                            }
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                    case CursorSource.Storage:
                        if (ReferenceEquals(_targetSlot, null)) return;
                        if (_targetSlot.curStack == 0)
                        {
                            _targetSlot.AddItem(_cursorSlot.id, 1);
                            DeleteSlot(_cursorSlot, 1);
                        }
                        else if (_targetSlot.id == _cursorSlot.id)
                        {
                            if (_targetSlot.GetCapacity() > 1)
                            {
                                _targetSlot.AddItem(_cursorSlot.id, 1);
                                DeleteSlot(_cursorSlot, 1);
                            }
                        }
                        else
                        {
                            SwapSlots(_targetSlot, _cursorSlot);
                        }
                        break;
                }
                break;
            case CursorSource.Shop:
                switch (_targetSource)
                {
                    case CursorSource.None:
                        DeleteSlot(_cursorSlot);
                        break;
                    case CursorSource.Inventory:
                        ShowShopPopupUI();
                        break;
                    case CursorSource.Storage:
                        break;
                    case CursorSource.Shop:
                        DeleteSlot(_cursorSlot);
                        break;
                }
                break;
        }
    }

    public void OnOffCursorAction()
    {
        switch (_cursorSlotSource)
        {
            case CursorSource.Inventory:
                if (_inventoryController.Inventory.TryAdd(_cursorSlot.id, _cursorSlot.curStack, out int remain)) DeleteSlot(_cursorSlot);
                else
                {
                    DeleteSlot(_cursorSlot, _cursorSlot.curStack - remain);
                    ThrowItemFromCursorSlot();
                }
                break;
            case CursorSource.Storage:
                break;
            case CursorSource.Shop:
                DeleteSlot(_cursorSlot);
                break;
        }

        if (_targetSlot != null)
        {
            Managers.Instance.UIManager.OpenUI<ItemInfoUI>().Refresh(_targetSlot.id);
        }
    }

    public void InsertSlot(int itemId, int count)
    {
        if (count > 0)
        {
            _cursorSlot.AddItem(itemId, count);
            _cursorSlotSource = _targetSource;
        }
    }

    public void DeleteSlot(Slot slot)
    {
        DeleteSlot(slot, slot.curStack);
    }

    public void DeleteSlot(Slot slot, int count)
    {
        slot.RemoveItem(count);

        if(slot.curStack == 0 && slot == _cursorSlot)
        {
            _cursorSlotSource = CursorSource.None;
            Managers.Instance.UIManager.CloseUI<CursorUI>();
        }
    }

    public void ThrowItemFromCursorSlot()
    {
        ThrowItemFromCursorSlot(_cursorSlot.curStack);
    }

    public void ThrowItemFromCursorSlot(int count)
    {
        Managers.Instance.ItemManager.SpawnCollectable(_cursorSlot.id, transform.position, count);
        DeleteSlot(_cursorSlot, count);
    }

    private void SwapSlots(Slot a, Slot b)
    {
        if (a == null || b == null) return;

        int aId = a.id;
        int aCount = a.RemoveItem();

        a.AddItem(b.id, b.RemoveItem());
        b.AddItem(aId, aCount);

        if (_targetSlot != null && _targetSlot.id != 0) Managers.Instance.UIManager.OpenUI<ItemInfoUI>().Refresh(_targetSlot.id);
        else Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
    }

    private void SetTargetSlot(BaseEventData baseEventData)
    {
        if (baseEventData == null)
        {
            _targetSlot = null;
            return;
        }

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (!ReferenceEquals(pointerEventData, null) && !ReferenceEquals(pointerEventData.pointerEnter, null))
        {
            if (pointerEventData.pointerEnter.TryGetComponent(out SlotUI slotUI))
            {
                _targetSlot = slotUI.observedSlot;
                if (_targetSlot.id != 0)
                {
                    Managers.Instance.UIManager.OpenUI<ItemInfoUI>().Refresh(_targetSlot.id);
                }
            }
        }
    }

    private void SetTargetSource(CursorSource source)
    {
        _targetSource = source;
        if (source == CursorSource.None)
        {
            _targetSlot = null;
        }
    }
}