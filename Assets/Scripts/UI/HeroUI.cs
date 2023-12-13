using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class HeroUI : BaseUI
{
    private HerosController _herosController;

    private HeroHandler _selectedHero;
    private List<UIButton> _heroButtons;
    private List<TextSlot> _infoTexts;
    private EventEntrySO _entries;
    private int _selectedInfo;

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _heroStatus;
    [SerializeField] private Button _individualityButton;
    [SerializeField] private Button _combatButton;
    [SerializeField] private Button _healthButton;
    [SerializeField] private Transform _infoContent;
    [SerializeField] private GameObject _heroFrame;

    [SerializeField] private Image _heroImage;

    [SerializeField] private InventoryUI _subInventory;

    public void Init(HerosController herosController, InventoryController inventoryController)
    {
        _herosController = herosController;
        _heroButtons = new();
        _infoTexts = new();
        _entries = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);
        herosController.OnChangeHeroEvnet += Refresh;
        herosController.OnChangeHeroEvnet += ShowInfo;
        _selectedInfo = 0;

        _individualityButton.onClick.AddListener(() => { _selectedInfo = 0; ShowInfo(); });
        _combatButton.onClick.AddListener(() => { _selectedInfo = 1; ShowInfo(); });
        _healthButton.onClick.AddListener(() => { _selectedInfo = 2; ShowInfo(); });

        _subInventory.Init(inventoryController, true);
    }

    public override void On()
    {
        base.On();
        Refresh(); // subscribe on change count event of controller
    }

    public override void Off()
    {
        base.Off();
        _heroStatus.SetActive(false);

        _entries.Publish(EventTriggerType.PointerExit, CursorSource.None);
    }

    private void Refresh()
    {
        ClearScroll();
        BuildScroll();
    }

    private void BuildScroll()
    {
        int heroSize = _herosController.HeroHandlers.Count;
        if (heroSize > _heroButtons.Count)
        {
            UIButton heroButtonPrefab = Managers.Instance.DataManager.GetPrefab<UIButton>(Const.Prefabs_UIButton);
            UIButton newHeroButton;

            for (int i = _heroButtons.Count; i < heroSize; i++)
            {
                newHeroButton = Instantiate(heroButtonPrefab, _content);
                newHeroButton.SetButton(SetHero);

                _heroButtons.Add(newHeroButton);
            }

            for (int i = 0; i < _heroButtons.Count; i++)
            {
                _heroButtons[i].SetParamAndText(i, _herosController.HeroHandlers[i].IndividualityStat.Name);
                _heroButtons[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < heroSize; i++)
            {
                _heroButtons[i].SetParamAndText(i, _herosController.HeroHandlers[i].IndividualityStat.Name);
                _heroButtons[i].gameObject.SetActive(true);
            }
        }
    }

    private void ClearScroll()
    {
        foreach (var heroButton in _heroButtons)
        {
            heroButton.gameObject.SetActive(false);
        }
    }

    private void SetHero(int heroId)
    {
        _herosController.SelectedHeroIndex = heroId;
        _selectedHero = _herosController.HeroHandlers[heroId];

        _heroImage.sprite = Managers.Instance.DataManager.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, _selectedHero.SpriteId).GetSprite("Idle", "0");
        _heroStatus.SetActive(true);
        ShowInfo();
    }

    public void ShowInfo()
    {
        if (!_heroStatus.activeSelf) return;
        foreach (var infoText in _infoTexts) infoText.gameObject.SetActive(false);

        int infoSize = _selectedInfo switch
        {
            0 => 5, // indivi
            1 => 14, // combat
            2 => 5, // health
            _ => 5
        };

        if (infoSize > _infoTexts.Count)
        {
            TextSlot TextSlotPrefab = Managers.Instance.DataManager.GetPrefab<TextSlot>(Const.Prefabs_TextSlot);
            TextSlot newTextSlot;

            for (int i = _infoTexts.Count; i < infoSize; i++)
            {
                newTextSlot = Instantiate(TextSlotPrefab, _infoContent);

                _infoTexts.Add(newTextSlot);
            }
        }
        switch (_selectedInfo)
        {
            case 0: // individuality stat
                Dictionary<int, BaseIndividualityStatData> indis = Managers.Instance.DataManager.Indis;
                _infoTexts[0].Name("이름").Value(_selectedHero.IndividualityStat.Name).gameObject.SetActive(true);
                _infoTexts[1].Name("출신").Value(indis[_selectedHero.IndividualityStat.OriginCode].Name).gameObject.SetActive(true);
                _infoTexts[2].Name("성격").Value(indis[_selectedHero.IndividualityStat.NatureCode].Name).gameObject.SetActive(true);
                _infoTexts[3].Name("직업").Value(indis[_selectedHero.IndividualityStat.JobCode].Name).gameObject.SetActive(true);
                _infoTexts[4].Name("기호").Value(indis[_selectedHero.IndividualityStat.FlavorCode].Name).gameObject.SetActive(true);
                break;
            case 1: // combat stat
                int[] stat = _selectedHero.CombatStat.Stat;
                _infoTexts[0].Name("최대 체력").Value(stat[(int)Stats.MaxHealth].ToString()).gameObject.SetActive(true);
                _infoTexts[1].Name("현재 체력").Value(_selectedHero.CurHealth.ToString()).gameObject.SetActive(true);
                _infoTexts[2].Name("체력 재생").Value(stat[(int)Stats.GenHealth].ToString()).gameObject.SetActive(true);
                _infoTexts[3].Name("요구 마나").Value(stat[(int)Stats.MaxMana].ToString()).gameObject.SetActive(true);
                _infoTexts[4].Name("시작 마나").Value(stat[(int)Stats.InitMana].ToString()).gameObject.SetActive(true);
                _infoTexts[5].Name("마나 재생").Value(stat[(int)Stats.GenMana].ToString()).gameObject.SetActive(true);
                _infoTexts[6].Name("공격력").Value(stat[(int)Stats.AtkPower].ToString()).gameObject.SetActive(true);
                _infoTexts[7].Name("공격 사거리").Value(stat[(int)Stats.AtkRange].ToString()).gameObject.SetActive(true);
                _infoTexts[8].Name("공격 속도").Value($"{(stat[(int)Stats.AtkSpeed] / 100f).ToString("0.0")} / sec").gameObject.SetActive(true);
                _infoTexts[9].Name("마공력").Value(stat[(int)Stats.MagPower].ToString()).gameObject.SetActive(true);
                _infoTexts[10].Name("치명타 확률").Value($"{stat[(int)Stats.CriticalChance]} %").gameObject.SetActive(true);
                _infoTexts[11].Name("치명타 데미지").Value($"{stat[(int)Stats.CriticalDamage] + 100} %").gameObject.SetActive(true);
                _infoTexts[12].Name("회피율").Value($"{stat[(int)Stats.DodgeChance]} %").gameObject.SetActive(true);
                _infoTexts[13].Name("방어력").Value(stat[(int)Stats.Armor].ToString()).gameObject.SetActive(true);
                break;
            case 2: // health stat
                stat = _selectedHero.HealthStat.CurNutritions;
                _infoTexts[0].Name("탄수화물").Value(stat[(int)Nutrients.Carbs].ToString()).gameObject.SetActive(true);
                _infoTexts[1].Name("단백질").Value(stat[(int)Nutrients.Protein].ToString()).gameObject.SetActive(true);
                _infoTexts[2].Name("지방").Value(stat[(int)Nutrients.Fat].ToString()).gameObject.SetActive(true);
                _infoTexts[3].Name("비타민").Value(stat[(int)Nutrients.Vitamin].ToString()).gameObject.SetActive(true);
                _infoTexts[4].Name("만족도").Value(_selectedHero.HealthStat.Satisfaction.ToString()).gameObject.SetActive(true);
                break;
        }
    }
}
