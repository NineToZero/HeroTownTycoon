using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Transform _rootUI;

    private Dictionary<Type, BaseUI> _uis;

    private int _order;
    private List<BaseUI> _popupStack;

    EventEntrySO _entries;

    private bool _isPopupOpened;
    private bool _isModalOpened;

    public Dictionary<Type, BaseUI> UIs { get { return _uis; } }

    public void Init(PlayerControllerHandler controllers)
    {
        GetUI<InteractionUI>();
        GetUI<PartyUI>().Init(controllers.HerosController);
        GetUI<ConquestUI>().Init(new StageBuilder(controllers.StageController));
        GetUI<ShopPopupUI>().Init(controllers.InventoryController);
        GetUI<CraftingUI>().Init(controllers.CraftingContoller);
        GetUI<InventoryUI>().Init(controllers.InventoryController, false);
        GetUI<HotbarUI>().Init(controllers.InventoryController);
        GetUI<ShopUI>().Init(controllers.InventoryController);
        GetUI<HeroUI>().Init(controllers.HerosController, controllers.InventoryController);
        GetUI<StorageUI>().Init(controllers.InventoryController);
        GetUI<TravelUI>().Init(controllers.StageController);
        GetUI<TravelResultUI>().Init(controllers.InventoryController, controllers.StageController);
        GetUI<HeroDeathUI>().Init(controllers.HerosController);
        GetUI<TravelSummaryUI>().Init(controllers.StageController);
        GetUI<HeroPopupUI>().Init(new HeroBuilder(controllers.HerosController));
    }

    private void Awake()
    {
        _rootUI = new GameObject("UI").transform;

        _order = 10;
        _uis = new();
        _popupStack = new();

        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);
    }

    public T GetUI<T>() where T : BaseUI
    {
        T ui;

        if (_uis.TryGetValue(typeof(T), out BaseUI baseUI))
        {
            ui = baseUI as T;
            if (ReferenceEquals(ui, null)) _uis.Remove(typeof(T));
            else return ui;
        }

        ui = Resources.Load<T>($"{Const.Prefabs_UIPath}/{typeof(T)}");
        ui = Instantiate(ui, _rootUI);

        _uis.Add(typeof(T), ui);
        ui.gameObject.SetActive(false);
        return ui;
    }
    public T OpenUI<T>() where T : BaseUI
    {
        return OpenUI(GetUI<T>());
    }
    public T OpenUI<T>(T ui) where T : BaseUI
    {
        if (ui.gameObject.activeSelf) return ui;

        switch (ui.Type)
        {
            case UIType.Popup:
                ui.gameObject.GetComponent<Canvas>().sortingOrder = _order++;
                _popupStack.Add(ui);
                _isPopupOpened = true;
                break;
            case UIType.Scene:
                ui.gameObject.GetComponent<Canvas>().sortingOrder = 0;
                break;
            case UIType.Notification:
                ui.gameObject.GetComponent<Canvas>().sortingOrder = 5;
                break;
            case UIType.Modal:
                _isModalOpened = true;
                if (!_isPopupOpened) _entries.Publish(UIType.Popup, true);
                ui.gameObject.GetComponent<Canvas>().sortingOrder = 80;
                break;
        }

        _entries.Publish(ui.Type, true);
        ui.On();

        return ui;
    }
    public void CloseUI<T>() where T : BaseUI
    {
        CloseUI(GetUI<T>());
    }
    public void CloseUI<T>(T ui) where T : BaseUI
    {
        if (!ui.gameObject.activeSelf) return;
        switch (ui.Type)
        {
            case UIType.Popup:
                _popupStack.Remove(ui);
                if (_popupStack.Count == 0)
                {
                    _isPopupOpened = false;
                    _order = 10;
                    if (!_isModalOpened) _entries.Publish(ui.Type, false);
                }
                else
                {
                    _order--;
                }
                break;
            case UIType.Modal:
                _isModalOpened = false;
                _entries.Publish(ui.Type, false);
                if (!_isPopupOpened) _entries.Publish(UIType.Popup, false);
                break;
        }

        ui.Off();
    }
    public bool ToggleUI<T>() where T : BaseUI
    {
        T ui = GetUI<T>();

        if (ui.gameObject.activeSelf) CloseUI(ui);
        else OpenUI(ui);

        return ui.gameObject.activeSelf;
    }

    public void CloseRecentPopup()
    {
        if (_popupStack.Count > 0)
        {
            BaseUI ui = _popupStack[_popupStack.Count - 1];

            if (ui.Type == UIType.Popup)
            {
                _popupStack.Remove(ui);
                if (_popupStack.Count == 0)
                {
                    _isPopupOpened = false;
                    _order = 10;
                    if (!_isModalOpened) _entries.Publish(ui.Type, false);
                }
            }

            ui.Off();

            if (ui is ItemInfoUI)
                CloseRecentPopup();
        }
    }
    public void ShowPopupUI<T>(Action confirm, string title, string body, Action cancle, string confirmText, string cancelText) where T : PopupUI
    {
        var ui = OpenUI<T>();
        ui.Refresh(confirm, title, body, cancle, confirmText, cancelText);
    }

    public void ShowPopupUI(Action confirm, string title = "안내", string body = "진행하시겠습니까?", Action cancle = null, string confirmText = "확인", string cancelText = "취소")
    {
        Managers.Instance.SoundManager.PlaySFX(SFXSource.Pop);
        Action callback = CloseUI<PopupUI>;
        callback += confirm;
        cancle += CloseUI<PopupUI>;
        ShowPopupUI<PopupUI>(callback, title, body, cancle, confirmText, cancelText);
    }

    public void ShowNotificationUI(string text, float time = 2.0f)
    {
        var ui = OpenUI<NotificationUI>();
        ui.gameObject.GetComponent<Canvas>().sortingOrder = 100;
        ui.Refresh(text, time);
    }

    public void SwitchInteractionGuideUI(GameObject obj, bool isNull)
    {
        if (isNull)
        {
            CloseUI<InteractionGuideUI>();
        }
        else
        {
            var ui = OpenUI<InteractionGuideUI>();
            ui.Refresh(obj.transform.parent.gameObject);
        }
    }

}
