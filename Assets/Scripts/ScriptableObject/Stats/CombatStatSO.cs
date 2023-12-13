using UnityEngine;

[CreateAssetMenu(fileName = "CombatStat", menuName = "Scriptable Object/CombatStat")]
public class CombatStatSO : ScriptableObject
{
    [SerializeField] public int MaxHealth;
    [SerializeField] public int GenHealth;   
    [SerializeField] public int InitMana;     
    [SerializeField] public int MaxMana;      
    [SerializeField] public int GenMana;       
    [SerializeField] public int AtkPower;
    [Range(1, 7)]
    [SerializeField] public int AtkRange;      
    [SerializeField] public int AtkSpeed;      
    [SerializeField] public int MagPower;      
    [SerializeField] public int CriticalChance;
    [SerializeField] public int CriticalDamage;
    [Range(0, 100)]
    [SerializeField] public int DodgeChance;
    [SerializeField] public int Armor;   
}    