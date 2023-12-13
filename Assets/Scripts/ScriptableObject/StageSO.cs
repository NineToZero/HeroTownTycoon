using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Object/Stage")]
public class StageSO : ScriptableObject
{
    public int Id;
    public int Tier;
    public string Name;
    public List<int> EnemyIds;
    public Reward Reward;

    [Range(1, 5)]
    public int MaxPhase;
    [Range(1, 6)]
    public int PartyCapacity;
    [Range(1, 10)]
    public int BattlePhaseRatio;

    [Header("Enemy Spawn Amount Per Phase")]
    public int Min;
    public int Max;

    [Range(3, 13)]
    [SerializeField] private int _mapLength;
    [HideInInspector] public Vector2Int MapSize => new Vector2Int(_mapLength * 2, _mapLength);
}