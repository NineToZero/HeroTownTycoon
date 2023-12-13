using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelUI : BaseUI
{
    [SerializeField] private GameObject _nonBattleView;
    [SerializeField] private Image _battleView;

    [SerializeField] private Transform _first;
    [SerializeField] private Transform _second;

    private List<UnitUI> _firstUnits;
    private List<UnitUI> _secondUnits;

    private StageController _stageController;

    private void Awake()
    {
        _firstUnits = new();
        _secondUnits = new();
    }

    public void Init(StageController stageController)
    {
        _stageController = stageController;
    }

    public override void On()
    {
        base.On();

        List<HeroHandler> heroes = _stageController.StageHandler.Party;

        foreach (var first in _firstUnits) first.gameObject.SetActive(false);
        foreach (var second in _secondUnits) second.gameObject.SetActive(false);

        int firstSize = heroes.Count / 2 + 1;
        int secondSize = heroes.Count - firstSize;

        if (heroes.Count > _firstUnits.Count + _secondUnits.Count)
        {
            UnitUI unit = Managers.Instance.DataManager.GetPrefab<UnitUI>(Const.Prefabs_UnitUI);

            if (firstSize > _firstUnits.Count)
            {
                for (int i = _firstUnits.Count; i < firstSize; i++)
                {
                    _firstUnits.Add(Instantiate(unit, _first));
                }
            }

            if (secondSize > _secondUnits.Count)
            {
                for (int i = _secondUnits.Count; i < secondSize; i++)
                {
                    _secondUnits.Add(Instantiate(unit, _second));
                }
            }
        }

        for (int i = 0; i < firstSize; i++)
        {
            _firstUnits[i].Init(heroes[i]);
            _firstUnits[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < secondSize; i++)
        {
            _secondUnits[i].Init(heroes[i + firstSize]);
            _secondUnits[i].gameObject.SetActive(true);
        }
    }

    public void SetTexture(Texture Texture)
    {
        _battleView.material.SetTexture("_MainTex", Texture);
    }
    public void SetBattleView(bool turnOn)
    {
        _nonBattleView.SetActive(!turnOn);
        _battleView.gameObject.SetActive(turnOn);
    }
}