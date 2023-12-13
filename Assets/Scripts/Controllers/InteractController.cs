using UnityEngine;

public class InteractController : MonoBehaviour
{
    public float RayDistance = 10f;

    private Vector3 _screenCenter;

    private GameObject _pointingObject;
    public GameObject PointingObject
    {
        get { return _pointingObject; }
        private set
        {
            if (_pointingObject == value) return;
            _pointingObject = value;
            if (!value)
            {
                Managers.Instance.UIManager.SwitchInteractionGuideUI(value, true);
                _objectPointingArrow.gameObject.SetActive(false);
                return;
            }
            Managers.Instance.UIManager.SwitchInteractionGuideUI(value, false);
            _objectPointingArrow.gameObject.SetActive(true);
            _objectPointingArrow.transform.position = value.transform.position + Vector3.up;
        }
    }
    private Transform _objectPointingArrow;

    private int _availableLayer;
    private int _buildingLayer;

    private GameObject _crosshairUI;

    private void Awake()
    {
        Transform arrowPrefab = Managers.Instance.DataManager.GetPrefab<Transform>(Const.Prefabs_ObjectPointingArrow);
        _objectPointingArrow = Instantiate(arrowPrefab);
        _buildingLayer = LayerMask.NameToLayer("Building");

        _availableLayer = 1 << _buildingLayer;

        _crosshairUI = Instantiate(Managers.Instance.DataManager.GetPrefab(Const.Prefabs_CrosshairUI));

        Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Subscribe(UIType.Popup, SetCursorUnlock);
    }

    private void Start()
    {
        ChangeCameraCenter();
        Managers.Instance.OptionManager.OptionData.OnChangeResolution += ChangeCameraCenter;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(_screenCenter);
        RaycastHit rayData;
        bool isHit = Physics.Raycast(ray, out rayData, RayDistance, _availableLayer);
        if (!isHit)
        {
            PointingObject = null;
            return;
        }

        PointingObject = rayData.collider.gameObject;
    }

    public void Interact()
    {
        if (PointingObject == null) return;
        if (!PointingObject.transform.parent.TryGetComponent(out IInteractable interactable)) return;
        interactable.Interact();
    }

    public void SetCursorUnlock(bool isUnlocked)
    {
        if (isUnlocked)
        {
            Cursor.lockState = CursorLockMode.None;
            _crosshairUI.gameObject.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _crosshairUI.gameObject.SetActive(true);
        }
    }

    private void ChangeCameraCenter()
    {
#if UNITY_EDITOR
        _screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
#else
        var optionData = Managers.Instance.OptionManager.OptionData;
        var resoultions = optionData.Resolutions;
        var index = optionData.ResolutionIndex;
        Vector3 center = new Vector3(resoultions[index].width * 0.5f, resoultions[index].height * 0.5f);
        _screenCenter = center;
#endif
    }
}
