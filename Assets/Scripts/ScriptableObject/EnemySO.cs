using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Object/CombatStat/Enemy")]
public class EnemySO : CombatStatSO
{
    [SerializeField] public string Name;
    [SerializeField] public int Id;
}
