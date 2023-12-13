using UnityEngine;

public class UIController : MonoBehaviour
{
    private BuildingController _buildingController;
    private DayUI _dayUI;
    private InteractionUI _interactionUI;

    public BuildingController BuildingController { set { _buildingController = value; } }

    private void Start()
    {
        UIManager uIManager = Managers.Instance.UIManager;

        _dayUI = uIManager.OpenUI<DayUI>();
        _interactionUI = uIManager.GetUI<InteractionUI>();
    }

    public void InputEnter(bool isPressed)
    {
        if (isPressed)
            _dayUI.StartEnter();
        else
            _dayUI.CancelEnter();
    }

    public void OnClick()
    {
        if (_interactionUI.gameObject.activeSelf)
        {
            var building = _interactionUI.Building;
            int index = _interactionUI.PressMouseButton();
            _buildingController.ClickInteractButton(building, index);
        }
    }
}
