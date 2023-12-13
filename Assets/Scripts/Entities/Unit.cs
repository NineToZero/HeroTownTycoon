using UnityEngine;

public class Unit
{
    public readonly int SpriteId;
    public readonly string Name;
    public readonly int Id;
    public readonly bool IsTeamHero;
    public readonly CombatStat Stat;
    // enum으로 만들면 여러 팀이 한 맵에서 전투하는 것도 가능할 듯

    public Vector2Int Pos;
    public int TargetId;

    /// Hero to Unit
    public Unit(int id, string name, CombatStat combatStat, int spriteId)
    {
        Name = name;
        Id = id;
        IsTeamHero = true;
        Stat = combatStat;
        TargetId = -1;
        SpriteId = spriteId;
    }

    /// Enemy to Unit
    public Unit(int id, EnemySO enemySO)
    {
        Name = enemySO.Name;
        Id = id;
        IsTeamHero = false;
        Stat = new CombatStat(enemySO);
        Stat.CurHealth = enemySO.MaxHealth;
        TargetId = -1;
        SpriteId = enemySO.Id + 100;
    }
}
