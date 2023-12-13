using System;
using System.Collections.Generic;

public class BattlePhaseHandler : IPhaseHandler, IBattleHandler
{
    public int[,] Map => _phase.Map;
    public Dictionary<int, UnitHandler> Units => _units;
    private readonly Dictionary<int, UnitHandler> _units;

    private readonly BattlePhase _phase;
    private readonly StageHandler _stageHandler;

    public event Action OnStartPhaseEvent;
    public event Action OnEndPhaseEvent;

    public bool IsWon => _isAliveHero && !_isAliveEnemy;
    private bool _isAliveHero;
    private bool _isAliveEnemy;

    public BattlePhaseHandler(StageHandler stageHandler, BattlePhase battlePhase)
    {
        _stageHandler = stageHandler;
        _phase = battlePhase;
        _units = new Dictionary<int, UnitHandler>();

        Unit unit;
        UnitHandler unitHandler;
        for (int i = 0; i < _phase.Units.Length; i++)
        {
            unit = _phase.Units[i];
            unitHandler = new UnitHandler(this, unit);

            if (unit.IsTeamHero)
            {
                unitHandler.Modify(new StatValue() { Stat = Stats.CurHealth, Value = stageHandler.BringChangedHealth(unit.Id) }, ModifyType.Override);
                if (unitHandler.CombatStat.CurHealth <= 0) unitHandler.State = UnitState.Dead;
            }

            _units.Add(unit.Id, unitHandler);
        }
    }

    public void Enter()
    {
        OnStartPhaseEvent?.Invoke();
    }

    public void Excute()
    {
        Battle();
    }

    public void Exit()
    {
        _stageHandler.AddRewards(_phase.Rewards);
    }

    public void Battle()
    {
        _isAliveHero = false;
        _isAliveEnemy = false;

        foreach (var unit in _units.Values)
        {
            if (unit.Unit.IsTeamHero && !_isAliveHero && unit.State != UnitState.Dead)
            {
                _isAliveHero = true;
            }
            else if (!unit.Unit.IsTeamHero && !_isAliveEnemy && unit.State != UnitState.Dead)
            {
                _isAliveEnemy = true;
            }
        }

        if (!_isAliveHero || !_isAliveEnemy)
        {
            foreach (var unit in _units)
                if (unit.Value.Unit.IsTeamHero)
                    _stageHandler.ApplyChangedHealth(unit.Key, unit.Value.CombatStat.CurHealth);

            OnEndPhaseEvent?.Invoke();
        }
    }
}