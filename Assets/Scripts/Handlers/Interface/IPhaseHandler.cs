using System;

public interface IPhaseHandler
{
    event Action OnStartPhaseEvent;
    event Action OnEndPhaseEvent;

    void Enter();
    void Excute();
    void Exit();
}