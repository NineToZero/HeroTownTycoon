using UnityEngine;
using UnityEngine.UI;

public class FarmingHelperUI : HelperUI
{
    [SerializeField] private GameObject _first;
    [SerializeField] private GameObject _second;

    [SerializeField] private Button _nextButton;

    private new void Awake()
    {
        base.Awake();

        _nextButton.onClick.AddListener(() => _first.SetActive(false));
        _nextButton.onClick.AddListener(() => _second.SetActive(true));
    }

    public override void On()
    {
        base.On();

        _first.SetActive(true);
        _second.SetActive(false);
    }
}
