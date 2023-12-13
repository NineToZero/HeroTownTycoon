using UnityEngine;
using UnityEngine.UI;

public class HelperUI : BaseUI
{
    [SerializeField] private Button _closeButton;

    protected void Awake()
    {
        _closeButton.onClick.AddListener(() => Managers.Instance.UIManager.CloseUI(this));
    }
}

public class KeyHelperUI : HelperUI
{
    public override void Off()
    {
        base.Off();

        Managers.Instance.UIManager.OpenUI<MouseHelperUI>();
    }
}
