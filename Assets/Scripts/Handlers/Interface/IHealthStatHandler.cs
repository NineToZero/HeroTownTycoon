public interface IHealthStatHandler
{
    HealthStat HealthStat { get; }

    void Modify(NutrientValue nutrients, ModifyType modifyType);

    void AddSatisfaction(int satisfaction);

    void AddStatusEffect(StatusEffect effect);

    void RemoveStatusEffect(StatusEffect effect);
}
