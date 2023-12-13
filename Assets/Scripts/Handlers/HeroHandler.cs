public class HeroHandler : ICombatStatHandler, IHealthStatHandler, IIndividualityStatHandler
{
    public CombatStat CombatStat => GetCombatStatForCombat(); 
    [ES3Serializable]
    private readonly CombatStat _combatStat;

    public HealthStat HealthStat => _healthStat;
    [ES3Serializable]
    private readonly HealthStat _healthStat;

    public IndividualityStat IndividualityStat => _individualityStat;
    [ES3Serializable]
    private readonly IndividualityStat _individualityStat;

    public int CurHealth => _combatStat.CurHealth;

    public bool IsBusied;
    public bool IsAlive => _combatStat.CurHealth > 0;
    public int SpriteId => _individualityStat.SpriteCode;
    
    public HeroHandler()
    {
        //Only for Easy Save 3 !!!
    }
    
    public HeroHandler(CombatStatSO combatStatSO, HealthStatSO healthStatSO, IndividualityStat individualityStat)
    {
        _combatStat = new CombatStat(combatStatSO);
        _healthStat = new HealthStat(healthStatSO);
        _individualityStat = individualityStat;
    }

    public void Modify(NutrientValue nutrientValue, ModifyType modifyType)
    {
        int nutrient = (int)nutrientValue.nutrient;
        switch (modifyType)
        {
            case ModifyType.Add:
                _healthStat.CurNutritions[nutrient] += nutrientValue.value;
                break;
            case ModifyType.Override:
                _healthStat.CurNutritions[nutrient] = nutrientValue.value;
                break;
            case ModifyType.Multiple:
                _healthStat.CurNutritions[nutrient] *= nutrientValue.value;
                break;
        }

        if (_healthStat.CurNutritions[nutrient] < 0) _healthStat.CurNutritions[nutrient] = 0;
        else if (_healthStat.CurNutritions[nutrient] > 100) _healthStat.CurNutritions[nutrient] = 100;
    }
    public void Modify(StatValue statValue, ModifyType modifyType)
    {
        switch (modifyType)
        {
            case ModifyType.Add:
                if (statValue.Stat == Stats.CurHealth) AddHP((int)statValue.Value);
                else if (statValue.Stat == Stats.CurMana) return;
                else _combatStat.Stat[(int)statValue.Stat] = (int)(_combatStat.Stat[(int)statValue.Stat] + statValue.Value);
                break;
            case ModifyType.Override:
                if (statValue.Stat == Stats.CurHealth) _combatStat.CurHealth = (int)statValue.Value;
                else if (statValue.Stat == Stats.CurMana) _combatStat.CurMana = (int)statValue.Value;
                else _combatStat.Stat[(int)statValue.Stat] = (int)statValue.Value;
                break;
            case ModifyType.Multiple:
                if (statValue.Stat == Stats.CurHealth) return;
                else if (statValue.Stat == Stats.CurMana) return;
                else _combatStat.Stat[(int)statValue.Stat] = (int)(_combatStat.Stat[(int)statValue.Stat] * statValue.Value);
                break;
        }
    }
    public void AddHP(int hp)
    {
        _combatStat.CurHealth += hp;

        if (_combatStat.CurHealth > _combatStat.Stat[(int)Stats.MaxHealth])
        {
            _combatStat.CurHealth = _combatStat.Stat[(int)Stats.MaxHealth];
        }

        if (_combatStat.CurHealth <= 0)
        {
            //  사망
        }
    }

    public void AddSatisfaction(int satisfaction)
    {
        _healthStat.Satisfaction += satisfaction;

        if (_healthStat.Satisfaction <= 0)
        {
            //  마을을 떠남?
        }
    }
    public void AddStatusEffect(StatusEffect effect)
    {
        _healthStat.StatusEffects.Add(effect);
    }
    public void RemoveStatusEffect(StatusEffect effect)
    {
        _healthStat.StatusEffects.Remove(effect);
    }

    public void Eat(NutrientValue[] nutrientValues)
    {
        foreach(var nutrientValue in nutrientValues)
        {
            Modify(nutrientValue, ModifyType.Add);
        }
    }

    public void Cure(int healValue)
    {
        AddHP(healValue);
    }

    private void Metabolize()
    {
        // 소모치를 개성 스탯에 따라 범위를 정해주고 그 안에서 랜덤을 돌리면 더 재밌을 듯
        // 감소 로직
        #region Carb
        NutrientValue carb = new NutrientValue();
        carb.nutrient = Nutrients.Carbs;
        carb.value = _healthStat.CurNutritions[(int)Nutrients.Carbs];

        if (carb.value >= 30)
        {
            StatValue addedMagPower = new StatValue();
            addedMagPower.Stat = Stats.MagPower;
            addedMagPower.Value = 10;
            Modify(addedMagPower, ModifyType.Add);

            StatValue addedCriticalChance = new StatValue();
            addedCriticalChance.Stat = Stats.CriticalChance;
            addedCriticalChance.Value = 10;
            Modify(addedCriticalChance, ModifyType.Add);
        }
        carb.value -= 25;
        #endregion

        #region Protein
        NutrientValue protein = new NutrientValue();
        protein.nutrient = Nutrients.Protein;
        protein.value = _healthStat.CurNutritions[(int)Nutrients.Protein];

        if (protein.value >= 50)
        {
            StatValue addedAtkSpeed = new StatValue();
            addedAtkSpeed.Stat = Stats.AtkSpeed;
            addedAtkSpeed.Value = 10;
            Modify(addedAtkSpeed, ModifyType.Add);

            StatValue addedArmor = new StatValue();
            addedArmor.Stat = Stats.Armor;
            addedArmor.Value = 10;
            Modify(addedArmor, ModifyType.Add);
        }
        protein.value -= 25;
        #endregion

        #region Fat
        NutrientValue fat = new NutrientValue();
        fat.nutrient = Nutrients.Fat;
        fat.value = _healthStat.CurNutritions[(int)Nutrients.Protein];

        if (fat.value >= 30 && fat.value <= 60)
        {
            StatValue addedAtkPower = new StatValue();
            addedAtkPower.Stat = Stats.AtkPower;
            addedAtkPower.Value = 10;
            Modify(addedAtkPower, ModifyType.Add);

            StatValue addedMaxHealth = new StatValue();
            addedMaxHealth.Stat = Stats.MaxHealth;
            addedMaxHealth.Value = 10;
            Modify(addedMaxHealth, ModifyType.Add);
        }
        if (carb.value > 60)
        {
            fat.value += carb.value - 60;
            carb.value = 60;
        }
        fat.value -= 25;
        #endregion

        #region Vitamin
        NutrientValue vitamin = new NutrientValue();
        vitamin.nutrient = Nutrients.Vitamin;
        vitamin.value = _healthStat.CurNutritions[(int)Nutrients.Protein];

        if (vitamin.value >= 40)
        {
            StatValue addedGenHealth = new StatValue();
            addedGenHealth.Stat = Stats.GenHealth;
            addedGenHealth.Value = 1;
            Modify(addedGenHealth, ModifyType.Add);
        }
        vitamin.value -= 25;
        #endregion
        
        Modify(carb, ModifyType.Override);
        Modify(protein, ModifyType.Override);
        Modify(fat, ModifyType.Override);
        Modify(vitamin, ModifyType.Override);
    }

    public CombatStat GetCombatStatForCombat()
    {
        CombatStat combatStat = new CombatStat(_combatStat);
        Nature nature = Managers.Instance.DataManager.Indis[_individualityStat.NatureCode] as Nature;

        if (nature == null || nature.PositiveStat == nature.NegativeStat) return combatStat;
        combatStat.Stat[(int)nature.PositiveStat] = (int)(combatStat.Stat[(int)nature.PositiveStat] * 1.1f);
        combatStat.Stat[(int)nature.NegativeStat] = (int)(combatStat.Stat[(int)nature.NegativeStat] * 0.9f);

        combatStat.CurHealth = combatStat.Stat[(int)Stats.MaxHealth];
        combatStat.CurMana = combatStat.Stat[(int)Stats.InitMana];
        return combatStat;
    }

    public void DoDayEvent()
    {
        if(!IsAlive || HealthStat.Satisfaction <= 0)
        {
            Managers.Instance.UIManager.OpenUI<HeroDeathUI>().SetHero(this);
        }
        else
        {
            Modify(new StatValue() { Stat = Stats.CurHealth, Value = CombatStat.Stat[(int)Stats.MaxHealth] }, ModifyType.Override);
            Metabolize();
        }
    }
}
