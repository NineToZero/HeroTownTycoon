using System;

public class CombatStat
{
    public int[] Stat;

    public int CurHealth;
    public int CurMana;

    public CombatStat()
    {
        //Only for Easy Save 3 !!!
    }
    
    // Generator
    public CombatStat(CombatStatSO baseStat)
    {
        Stat = new int[Enum.GetValues(typeof(Stats)).Length - 2];
        Stat[(int)Stats.MaxHealth] = baseStat.MaxHealth;
        Stat[(int)Stats.GenHealth] = baseStat.GenHealth;
        Stat[(int)Stats.InitMana] = baseStat.InitMana;
        Stat[(int)Stats.MaxMana] = baseStat.MaxMana;
        Stat[(int)Stats.GenMana] = baseStat.GenMana;
        Stat[(int)Stats.AtkPower] = baseStat.AtkPower;
        Stat[(int)Stats.AtkRange] = baseStat.AtkRange;
        Stat[(int)Stats.AtkSpeed] = baseStat.AtkSpeed;
        Stat[(int)Stats.MagPower] = baseStat.MagPower;
        Stat[(int)Stats.CriticalChance] = baseStat.CriticalChance;
        Stat[(int)Stats.CriticalDamage] = baseStat.CriticalDamage;
        Stat[(int)Stats.DodgeChance] = baseStat.DodgeChance;
        Stat[(int)Stats.Armor] = baseStat.Armor;

        CurHealth = baseStat.MaxHealth;
        CurMana = baseStat.InitMana;
    }

    // Clone
    public CombatStat(CombatStat baseStat)
    {
        Stat = new int[baseStat.Stat.Length];
        Array.Copy(baseStat.Stat, Stat, baseStat.Stat.Length);

        CurHealth = baseStat.CurHealth;
        CurMana = baseStat.CurMana;
    }
}
