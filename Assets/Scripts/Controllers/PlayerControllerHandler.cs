using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerHandler : MonoBehaviour
{
    public MovementController MovementController { get; private set; }
    public InteractController InteractController { get; private set; }
    public InventoryController InventoryController { get; private set; }
    public HerosController HerosController { get; private set; }
    public StageController StageController { get; private set; }
    public UIController UIController { get; private set; }
    public CraftingController CraftingContoller { get; private set; }
    public BuildingController BuildingController { get; set; }
    public CharacterAnimatorController CharacterAnimatorController { get; private set; }
    public CursorSlotController CursorSlotController { get; private set; }

    private int _hotbarNumber;
    private bool _isInputable;

    private void Awake()
    {
        MovementController = gameObject.AddComponent<MovementController>();
        InteractController = gameObject.AddComponent<InteractController>();
        InventoryController = gameObject.AddComponent<InventoryController>();
        HerosController = gameObject.AddComponent<HerosController>();
        StageController = gameObject.AddComponent<StageController>();
        UIController = gameObject.AddComponent<UIController>();
        CraftingContoller = gameObject.AddComponent<CraftingController>();
        UIController.BuildingController = BuildingController;
        CharacterAnimatorController = gameObject.AddComponent<CharacterAnimatorController>();
        CursorSlotController = gameObject.AddComponent<CursorSlotController>();

        Init();
    }

    private void Init()
    {
        CraftingContoller.Init(InventoryController.Inventory);
        CursorSlotController.Init(InventoryController);

        Subscribe();
    }

    private void Start()
    {
        Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Publish(UIType.Popup, false);

        if (Managers.Instance.DayManager.Day == 0)
        {
            Managers.Instance.UIManager.OpenUI<BuildingHelperUI>();
            Managers.Instance.UIManager.OpenUI<HeroHelperUI>();
            Managers.Instance.UIManager.OpenUI<FarmingHelperUI>();
            Managers.Instance.UIManager.OpenUI<KeyHelperUI>();

            Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Publish(UIType.Popup, true);
        }
    }

    private void Subscribe()
    {
        Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Subscribe(UIType.Popup, (isOpened) => _isInputable = !isOpened);
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        MovementController.OnMoveInput(inputVector);

        if (inputVector.magnitude < 0.1f) CharacterAnimatorController.SetState(AnimationState.Idle);
        else CharacterAnimatorController.SetState(AnimationState.Walking);
        CharacterAnimatorController.SetSpriteFlip(inputVector.x < -0.1f);
    }

    public void OnInteract()
    {
        if (_isInputable)
            InteractController.Interact();
    }

    public void OnHotbar(InputValue value)
    {
        int number = (int)value.Get<float>();

        if (_isInputable)
            InventoryController.SelectedIndex = number;

        if (_hotbarNumber == number)
            return;

        if (BuildingController.BlueprintHandler.gameObject.activeSelf)
            BuildingController.OffBlueprintHandler();
    }
    public void OnHotbarScroll(InputValue value)
    {
        if (_isInputable)
            InventoryController.SelectedIndex = (InventoryController.SelectedIndex - (int)value.Get<Vector2>().normalized.y + 10) % 10;
    }

    public void OnClick()
    {
        BuildingController.BlueprintHandler.IsSuccessBlueprint();
        if (_isInputable)
        {
            GameObject pointingObject = InteractController.PointingObject;
            if (pointingObject == null)
            {
                if (InventoryController.GetSelectedSlot().id >= 16000 && InventoryController.GetSelectedSlot().id < 17000)
                {
                    BlueprintData blueprintData = Managers.Instance.DataManager.Items[InventoryController.GetSelectedSlot().id] as BlueprintData;
                    
                    BuildingController.OnBlueprintHandler(blueprintData.BuildingType, InventoryController.GetSelectedSlot().UseBlurprint);
                }
                return;
            }
            if (!pointingObject.transform.parent.TryGetComponent(out IClickable clickable)) return;
            InventoryController.ClickObject(clickable);
            CharacterAnimatorController.SetTrigger(AnimationTrigger.Slash);
        }
    }

    public void OnInventory()
    {
        if (_isInputable || Managers.Instance.UIManager.GetUI<InventoryUI>().gameObject.activeSelf)
            InventoryController.ToggleInventory();

        if (BuildingController.BlueprintHandler.gameObject.activeSelf)
            BuildingController.OffBlueprintHandler();
    }

    public void OnEnter(InputValue value)
    {
        if(_isInputable && !StageController.IsTraveling)
        {
            bool input = value.isPressed;
            UIController.InputEnter(input);
        }
        else if(StageController.IsTraveling)
        {
            Managers.Instance.UIManager.ShowNotificationUI("아직 돌아오지 못한 용사가 있습니다!");
        }
    }

    public void OnMap()
    {
        if(StageController.IsTraveling)
            Managers.Instance.UIManager.ToggleUI<TravelUI>();
    }

    public void OnEsc()
    {
        if(!_isInputable)
            Managers.Instance.UIManager.CloseRecentPopup();
        else
            Managers.Instance.UIManager.OpenUI<OptionUI>();
    }

    public void OnHelper(InputValue value)
    {
        if (_isInputable)
        {
            switch (value.Get<float>())
            {
                case 0: Managers.Instance.UIManager.OpenUI<KeyHelperUI>(); break;
                case 1: Managers.Instance.UIManager.OpenUI<FarmingHelperUI>(); break;
                case 2: Managers.Instance.UIManager.OpenUI<HeroHelperUI>(); break;
                case 3: Managers.Instance.UIManager.OpenUI<BuildingHelperUI>(); break;
            }
        }
    }
}