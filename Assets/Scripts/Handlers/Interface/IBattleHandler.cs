using System.Collections.Generic;

public interface IBattleHandler
{
    Dictionary<int, UnitHandler> Units { get; }
    int[,] Map { get; }
    void Battle();
}