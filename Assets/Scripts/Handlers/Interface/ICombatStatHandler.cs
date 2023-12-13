public interface ICombatStatHandler
{
    CombatStat CombatStat { get; }

    void Modify(StatValue statValue, ModifyType modifyType);
    void AddHP(int hp);
}
