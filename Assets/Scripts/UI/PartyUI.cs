using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyUI : BaseUI
{
    private StageBuilder _stageBuilder;
    private HerosController _herosController;

    private List<PartySlot> _partySlots;
    private List<UIButton> _heroButtons;
    private List<TextSlot> _synergySlots;

    [SerializeField] private Transform _slotContent;
    [SerializeField] private Transform _heroContent;
    [SerializeField] private Transform _synergyContent;
    [SerializeField] private Button _closeButton;

    private Dictionary<int, PartySlot> _buttonSlotMappingTable;
    public void Init(HerosController heroesController)
    {
        _herosController = heroesController;
        _partySlots = new();
        _heroButtons = new();
        _synergySlots = new();
        _buttonSlotMappingTable = new();

        _closeButton.onClick.AddListener(Managers.Instance.UIManager.CloseUI<PartyUI>);
    }

    public void Refresh(StageBuilder stageBuilder)
    {
        _stageBuilder = stageBuilder;

        if (_stageBuilder.PartyCount > 0) return;

        _buttonSlotMappingTable.Clear();
        foreach (var slot in _partySlots) slot.gameObject.SetActive(false);
        foreach (var slot in _synergySlots) slot.gameObject.SetActive(false);

        int heroSize = _herosController.HeroHandlers.Count;
        if (heroSize > _heroButtons.Count)
        {
            UIButton heroButtonPrefab = Managers.Instance.DataManager.GetPrefab<UIButton>(Const.Prefabs_UIButton);
            UIButton newHeroButton;

            for (int i = _heroButtons.Count; i < heroSize; i++)
            {
                newHeroButton = Instantiate(heroButtonPrefab, _heroContent);
                newHeroButton.SetButton(SelectHero);

                _heroButtons.Add(newHeroButton);
            }
        }

        for (int i = 0; i < heroSize; i++)
        {
            _heroButtons[i].SetParamAndText(i, _herosController.HeroHandlers[i].IndividualityStat.Name);
            _heroButtons[i].SetInteractable(!_herosController.HeroHandlers[i].IsBusied && _herosController.HeroHandlers[i].IsAlive);
            _heroButtons[i].InitPressState();
            _heroButtons[i].gameObject.SetActive(true);
        }
    }

    public void SelectHero(int heroId)
    {
        if (!_heroButtons[heroId].IsPressed)
        {
            if (_stageBuilder.PartyCapacity <= _stageBuilder.PartyCount) return;

            _stageBuilder.PutOrTakeHero(_herosController.HeroHandlers[heroId]);

            PartySlot slot = GetUnusedPartySlot();
            _buttonSlotMappingTable.Add(heroId, slot);

            slot.Refresh(_herosController.HeroHandlers[heroId]);

            RefreshSynergies();
        }
        else
        {
            _stageBuilder.PutOrTakeHero(_herosController.HeroHandlers[heroId]);

            PartySlot slot = _buttonSlotMappingTable[heroId];
            _buttonSlotMappingTable.Remove(heroId);
            slot.gameObject.SetActive(false);

            RefreshSynergies();
        }

        _heroButtons[heroId].IsPressed = !_heroButtons[heroId].IsPressed;
    }

    private PartySlot GetUnusedPartySlot()
    {
        PartySlot slot = _partySlots.FirstOrDefault(x => !x.gameObject.activeSelf);

        if (slot != null)
        {
            slot.gameObject.SetActive(true);

            return slot;
        }

        slot = Instantiate(Managers.Instance.DataManager.GetPrefab<PartySlot>(Const.Prefabs_PartySlot), _slotContent);
        _partySlots.Add(slot);

        return slot;
    }

    private void RefreshSynergies()
    {
        Dictionary<Stats, int> effects = Managers.Instance.DataManager.GetSO<SynergySO>(Const.SO_Synergy).GetEffectsForOrigin(_stageBuilder.SynergyCode);

        foreach (var slot in _synergySlots) slot.gameObject.SetActive(false);

        int effectSize = effects.Count;
        if (effectSize > _synergySlots.Count)
        {
            TextSlot textSlotPrefab = Managers.Instance.DataManager.GetPrefab<TextSlot>(Const.Prefabs_TextSlot);

            for (int i = _synergySlots.Count; i < effectSize; i++)
            {
                _synergySlots.Add(Instantiate(textSlotPrefab, _synergyContent));
            }
        }

        foreach (var effect in effects)
        {
            string name = effect.Key switch
            {
                Stats.MaxHealth => "최대 체력",
                Stats.GenHealth => "체력 재생",
                Stats.InitMana => "시작 마나",
                Stats.MaxMana => "요구 마나",
                Stats.GenMana => "마나 재생",
                Stats.AtkPower => "공격력",
                Stats.AtkRange => "공격 사거리",
                Stats.AtkSpeed => "공격 속도",
                Stats.MagPower => "마공력",
                Stats.CriticalChance => "치명타 확률",
                Stats.CriticalDamage => "치명타 데미지",
                Stats.DodgeChance => "회피율",
                Stats.Armor => "방어력",
                _ => string.Empty
            };
            string value = effect.Key switch
            {
                Stats.MaxMana => $"- {effect.Value} / sec",
                Stats.AtkSpeed => $"+ {((float)effect.Value / 100).ToString("0.0")} / sec",
                Stats.CriticalChance or Stats.CriticalDamage or Stats.DodgeChance => $"+ {effect.Value} %",
                Stats.CurHealth or Stats.CurMana => string.Empty,
                _ => $"+ {effect.Value}"
            };
            _synergySlots[--effectSize].Name(name).Value(value).gameObject.SetActive(true);
        }
    }
}
