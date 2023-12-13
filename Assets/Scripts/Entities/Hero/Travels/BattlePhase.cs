using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : Phase
{
    public Unit[] Units { get; private set; }
    public int[,] Map;

    public BattlePhase(List<HeroHandler> party, List<int> enemyIds, Dictionary<Stats, int> synergies, int width, int height)
    {
        PhaseType = PhaseType.Battle;
        Map = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Map[i, j] = -1;
            }
        }

        int id = 0;
        Units = new Unit[party.Count + enemyIds.Count];
        DataManager dataManager = Managers.Instance.DataManager;

        Unit unit;
        Vector2Int pos;

        foreach (HeroHandler hero in party)
        {
            CombatStat combatStat = new(hero.CombatStat);
            foreach (var synergy in synergies)
            {
                if (synergy.Key == Stats.MaxMana) combatStat.Stat[(int)synergy.Key] -= synergy.Value;
                else combatStat.Stat[(int)synergy.Key] += synergy.Value;
            }

            unit = new Unit(id, hero.IndividualityStat.Name, combatStat, hero.SpriteId);

            do pos = new Vector2Int(Random.Range(0, height / 2 + 1), Random.Range(0, height));
            while (Map[pos.x, pos.y] != -1);

            unit.Pos = pos;
            Map[pos.x, pos.y] = id;
            Units[id++] = unit;
        }
        foreach (int enemyId in enemyIds)
        {
            unit = new Unit(id, dataManager.GetSO<EnemySO>(Const.SO_Enemy, enemyId));
            do pos = new Vector2Int(Random.Range(width - height / 2,  width), Random.Range(0, height));
            while (Map[pos.x, pos.y] != -1);

            unit.Pos = pos;
            Map[pos.x, pos.y] = id;
            Units[id++] = unit;
        }
    }
}