using System;
using System.Collections.Generic;

public class StageHandler
{
    private readonly Stage _stage;
    public string Name => _stage.Name;
    public List<HeroHandler> Party => _stage.Party;
    public int PhaseSize => _stage.Phases.Length;
    public IPhaseHandler PhaseHandler { get; private set; }
    public Reward TotalReward => _totalReward;
    private Reward _totalReward;
    public int CurPhaseNum { get; private set; }

    public event Action OnEndEvent;
    public event Action OnStartPhaseEvent;
    public event Action OnEndPhaseEvent;

    public StageHandler(List<HeroHandler> party, StageSO stageSO, Dictionary<Stats, int> synergies) {
        _stage = new Stage(stageSO, party, synergies);

        foreach (var hero in party)
        {
            hero.IsBusied = true;
        }
    }

    public void StartNext()
    {
        if (!ReferenceEquals(PhaseHandler, null))
        {
            PhaseHandler.Exit();
            CurPhaseNum++;

            if (IsDeadAllHeroes())
            {
                Managers.Instance.UIManager.CloseUI<TravelUI>();
                Managers.Instance.UIManager.OpenUI<TravelResultUI>().Refresh(isWon: false);
                foreach(var hero in _stage.Party) hero.IsBusied = false;
                OnEndEvent?.Invoke();
                return;
            }
        }

        if (_stage.Phases.Length == CurPhaseNum)
        {
            Managers.Instance.UIManager.CloseUI<TravelUI>();
            Managers.Instance.UIManager.OpenUI<TravelResultUI>().Refresh(isWon: true);
            foreach (var hero in _stage.Party) hero.IsBusied = false;
            OnEndEvent?.Invoke();
            return;
        }

        PhaseHandler = _stage.Phases[CurPhaseNum].PhaseType switch
        {
            PhaseType.Battle => new BattlePhaseHandler(this, _stage.Phases[CurPhaseNum] as BattlePhase),
            PhaseType.Bonus => new BonusPhaseHandler(this, _stage.Phases[CurPhaseNum] as BonusPhase),
            _ => throw new Exception("Error on Convert Phase"),
        };

        PhaseHandler.OnStartPhaseEvent += OnStartPhaseEvent;
        PhaseHandler.OnEndPhaseEvent += OnEndPhaseEvent;

        PhaseHandler.Enter();
    }

    public void AddRewards(Reward rewards)
    {
        _totalReward.Add(rewards);
    }

    private bool IsDeadAllHeroes()
    {
        foreach (HeroHandler hero in _stage.Party)
            if(hero.IsAlive)
                return false;

        return true;
    }

    public void ApplyChangedHealth(int heroId, int changedHp)
    {
        // if changeHp => StatValue, get more scalability
        _stage.Party[heroId].Modify(new StatValue() { Stat = Stats.CurHealth, Value = changedHp }, ModifyType.Override);
    }
    public int BringChangedHealth(int heroId)
    {
        // if return int => StatValue, get more scalability
        return _stage.Party[heroId].CurHealth;
    }
}
