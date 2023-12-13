using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthStat", menuName = "Scriptable Object/HealthStat")]
public class HealthStatSO : ScriptableObject
{
    public int Carbs;
    public int Protein;
    public int Fat;
    public int Vitamin;
    public int Satisfaction;
    public List<StatusEffect> StatusEffects;
}
