using System;

public class BonusPhaseHandler : IPhaseHandler
{
    private Phase _phase;
    private StageHandler _stageHandler;

    public event Action OnStartPhaseEvent;
    public event Action OnEndPhaseEvent;

    public BonusPhaseHandler(StageHandler stageHandler, BonusPhase bonusPhase)
    {
        _stageHandler = stageHandler;
        _phase = bonusPhase;
    }

    public void Enter()
    {
        OnStartPhaseEvent?.Invoke();
    }

    public void Excute()
    {
        OnEndPhaseEvent?.Invoke();
    }

    public void Exit()
    {
        _stageHandler.AddRewards(_phase.Rewards);
    }
}