using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    private int _heroId;

    private Color _selectedColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    private Color _originColor = new Color(1f, 1f, 1f, 1f);

    private bool _isPressed;
    public bool IsPressed {
        get { return _isPressed; }
        set { if(_isPressed != value) { _isPressed = value; _image.color = value ? _selectedColor : _originColor; } }
    }

    public void SetButton(UnityAction<int> action)
    {
        _button.onClick.AddListener(() => { action(_heroId); });
    }

    public void SetParamAndText(int heroId, string heroName)
    {
        _heroId = heroId;
        _text.text = heroName;
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }

    public void InitPressState(bool isPressed = false)
    {
        IsPressed = isPressed;
    }
}