using UnityEngine;
using UnityEngine.UI;

public class HeroHelperUI : HelperUI
{
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _stat;

    [SerializeField] private Button _nextButton;

    private new void Awake()
    {
        base.Awake();

        _nextButton.onClick.AddListener(() => _hero.SetActive(false));
        _nextButton.onClick.AddListener(() => _stat.SetActive(true));
    }

    public override void On()
    {
        base.On();

        _hero.SetActive(true);
        _stat.SetActive(false);
    }
}
