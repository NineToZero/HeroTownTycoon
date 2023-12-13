using System;
using System.Collections.Generic;

public class HealthStat
{
    public int[] CurNutritions;
    public int Satisfaction;
    public List<StatusEffect> StatusEffects;

    public HealthStat()
    {
        //Only for Easy Save 3 !!!    
    }
    
    public HealthStat(HealthStatSO healthStatSO)
    {
        CurNutritions = new int[Enum.GetValues(typeof(Nutrients)).Length];
        CurNutritions[(int)Nutrients.Carbs] = healthStatSO.Carbs;
        CurNutritions[(int)Nutrients.Protein] = healthStatSO.Protein;
        CurNutritions[(int)Nutrients.Fat] = healthStatSO.Fat;
        CurNutritions[(int)Nutrients.Vitamin] = healthStatSO.Vitamin;

        Satisfaction = healthStatSO.Satisfaction;
        StatusEffects = healthStatSO.StatusEffects;
    }
}
