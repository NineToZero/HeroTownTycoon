using System;

public class HeroBuilder
{
    private HerosController _heroController;
    
    // base stat의 개념이라 캐싱해둠
    private CombatStatSO _combatStatSO;
    private HealthStatSO _healthStatSO;
    private IndividualityStatSO _individualityStatSO;

    private IndividualityStat _individualityStat;
    private int _curProb => (int)(100 - 85 * (3 * _heroController.HeroHandlers.Count / (Managers.Instance.DayManager.Day + 1.0f)));

    public HeroBuilder(HerosController heroController)
    {
        DataManager dm = Managers.Instance.DataManager;
        _combatStatSO = dm.GetSO<CombatStatSO>(Const.SO_Hero_Combat);
        _healthStatSO = dm.GetSO<HealthStatSO>(Const.SO_Hero_Health);
        _individualityStatSO = dm.GetSO<IndividualityStatSO>(Const.SO_Hero_Individuality);

        if (!_combatStatSO || !_healthStatSO || !_individualityStatSO)
            throw new Exception("Error on Load HeroData");
        
        _heroController = heroController;
        
        Managers.Instance.DayManager.DayChangeEvent += OnDayChangeEvent;
    }

    public void Build()
    {
        if (_individualityStat == null) _individualityStat = new IndividualityStat(_individualityStatSO);
        HeroHandler heroHandler = new HeroHandler(_combatStatSO, _healthStatSO, _individualityStat);
        _heroController.AddHero(heroHandler);
        Managers.Instance.UIManager.ShowNotificationUI("새로운 용사가 영입되었습니다.");

        _individualityStat = null;
    }

    private void OnDayChangeEvent()
    {
        if (UnityEngine.Random.Range(0, 100) < _curProb)
            Managers.Instance.UIManager.ShowPopupUI<HeroPopupUI>(() => { Build(); Managers.Instance.UIManager.CloseUI<HeroPopupUI>(); }, "새로운 용사", "용사가 마을을 방문했습니다!", Managers.Instance.UIManager.CloseUI<HeroPopupUI>, "영입", "무시");
    }

    public IndividualityStat Prebuild()
    {
        _individualityStat = new IndividualityStat(_individualityStatSO);

        return _individualityStat;
    }
}