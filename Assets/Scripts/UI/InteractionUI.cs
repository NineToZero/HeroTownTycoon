using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : BaseUI
{

    [SerializeField] private GameObject[] _lines;
    [SerializeField] private Image[] _icons;
    [SerializeField] private Transform _cursor;
    [SerializeField] private TMP_Text _info;
    private readonly float ICONDISTANCE = 180;

    private ImageSO _iconSprite;
    private Vector2 _scrrenCenter;
    private Vector2 _mousePos;
    private BaseBuilding _building;
    private InteractionButton[] _buttons;

    private float _buttonAngle;
    private int _currentButtonNumber;

    public BaseBuilding Building { get { return _building; } }
    private string[] _buttonKor;

    #region Unity Event
    private void Awake()
    {
        Camera _camera = Camera.main;
        _scrrenCenter = new Vector2(_camera.pixelWidth / 2, _camera.pixelHeight / 2);
        _iconSprite = Resources.Load<ImageSO>(Const.SO_InteractionIcons);

        _currentButtonNumber = 0;
        Off();
        _buttonKor = new string[]
        {"취소","정보","파괴","승급","조합","의료","요리","토벌","모험","거래","창고","치료","용사 정보"};
    }

    private void Update()
    {
        GetMousePosition();
        RotateCursor();
        if (!TryChangeButton())
            return;

        SetInfoText();
    }
    #endregion

    #region General
    public override void On()
    {
        base.On();
        Managers.Instance.UIManager.CloseUI<InteractionGuideUI>();
    }

    private bool TryChangeButton()
    {
        float angle360 = 359.9f - _cursor.rotation.eulerAngles.z;
        int selectButton = (int)(angle360 / _buttonAngle);
        if (_currentButtonNumber == selectButton)
            return false;

        _currentButtonNumber = selectButton;
        return true;
    }

    private void SetInfoText()
    {
        int btnType = (int)_buttons[_currentButtonNumber];
        string text = _buttonKor[btnType];
        _info.text = text;
    }
    #endregion

    #region Initialization
    public void Refresh(InteractionButton[] Buttons, BaseBuilding building)
    {
        _buttons = Buttons;
        _building = building;
        SetUIObject();
        SetButtonIcon();
        TryChangeButton();
        SetInfoText();
    }

    private void SetUIObject()
    {
        _buttonAngle = (360f / _buttons.Length);
        float iconAngle = _buttonAngle * 0.5f;

        for (int i = 0; i < _lines.Length; i++)
        {
            if (_buttons.Length <= i)
            {
                if (!_lines[i].activeSelf)
                    return;

                SetActiveButtonBundle(i, false);
                continue;
            }

            SetActiveButtonBundle(i, true);

            float dir = -i * _buttonAngle;
            _lines[i].transform.rotation = Quaternion.Euler(0, 0, dir);

            float rad = (dir + iconAngle + 90) * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            _icons[i].rectTransform.anchoredPosition = new Vector2(x * ICONDISTANCE, y * ICONDISTANCE);
        }

        void SetActiveButtonBundle(int index, bool isActive)
        {
            _icons[index].gameObject.SetActive(isActive);
            _lines[index].SetActive(isActive);
        }
    }

    private void SetButtonIcon()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i+1;
            if (index == _buttons.Length)
                index = 0;

            int btnindex = (int)_buttons[i];

            _icons[index].sprite = _iconSprite.Images[0].SpriteList[btnindex];
        }
    }
    #endregion

    #region Mouse
    private void GetMousePosition(Vector3 pos = default) // TODO : 인풋 시스템 연결
    {
        pos = Input.mousePosition;
        _mousePos = pos;
    }

    private void RotateCursor()
    {
        Vector2 dir = _scrrenCenter - _mousePos;
        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angleZ += 90f;

        _cursor.rotation = Quaternion.Euler(0, 0, angleZ);
    }

    public int PressMouseButton()
    {
        Managers.Instance.UIManager.CloseUI<InteractionUI>();
        Managers.Instance.UIManager.OpenUI<InteractionGuideUI>();
        return _currentButtonNumber;
    }

    #endregion
}
