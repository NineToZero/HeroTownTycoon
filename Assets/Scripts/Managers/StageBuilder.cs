using System.Collections.Generic;

public class StageBuilder
{
    private StageController _controller;

    private List<HeroHandler> _party;
    private StageSO _stageSO;
    public int PartyCount => _party.Count;
    public int PartyCapacity => _stageSO.PartyCapacity;
    public int SelectedStageId { set { _stageSO = Managers.Instance.DataManager.GetSO<StageSO>(Const.SO_Stage, value); _party.Clear(); } }
    public int SynergyCode
    {
        get
        {
            int hashSum = 0;
            foreach(var hero in _party)
            {
                int origin = 1 << (hero.IndividualityStat.OriginCode % 100);
                if ((hashSum & origin) != origin)
                    hashSum += origin;
            }
            return hashSum;
        }
    }

    public StageBuilder(StageController stageController)
    {
        _party = new List<HeroHandler>();

        _controller = stageController;
    }
    public void PutOrTakeHero(HeroHandler hero)
    {
        if(_party.Contains(hero))
        {
            _party.Remove(hero);
        }
        else
        {
            _party.Add(hero);
        }
    }

    public void Build()
    {
        if (_controller.IsTraveled)
        {
            Managers.Instance.UIManager.ShowNotificationUI("오늘은 더이상 토벌을 보낼 수 없습니다.");
            return;
        }

        SynergySO synergy = Managers.Instance.DataManager.GetSO<SynergySO>(Const.SO_Synergy);
        StageHandler stageHandler = new StageHandler(_party, _stageSO, synergy.GetEffectsForOrigin(SynergyCode));
        _controller.SetHandler(stageHandler);
        _controller.StartStage();
        _party = new();
    }
}