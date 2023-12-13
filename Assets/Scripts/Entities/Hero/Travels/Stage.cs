using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public string Name { get; private set; }
    public List<HeroHandler> Party { get; private set; }
    public Phase[] Phases { get; private set; }

    public Stage(StageSO stageSO, List<HeroHandler> party, Dictionary<Stats, int> synergies)
    {
        Name = stageSO.Name;
        Party = party;

        // Make Phases
        Phases = new Phase[stageSO.MaxPhase];
        Phase newPhase;
        List<int> rewardItem;
        int rewardGold;

        for (int i = 0; i < stageSO.MaxPhase; i++)
        {
            if (Random.Range(0, 10) < stageSO.BattlePhaseRatio)
            {
                // 전투 페이즈
                int enemyCount = Random.Range(stageSO.Min, stageSO.Max + 1);
                List<int> EnemyIds = new List<int>(enemyCount);
                for (int cnt = 0; cnt < enemyCount; cnt++)
                {
                    int id = stageSO.EnemyIds[Random.Range(0, stageSO.EnemyIds.Count)];
                    EnemyIds.Add(id);
                }

                newPhase = new BattlePhase(Party, EnemyIds, synergies, stageSO.MapSize.x, stageSO.MapSize.y);

                rewardItem = new();
                if (Random.Range(0, 100) < 50 + enemyCount * 5)
                {
                    if (stageSO.Reward.Items.Count > 0)
                        rewardItem.Add(stageSO.Reward.Items[Random.Range(0, stageSO.Reward.Items.Count)]);
                }
                rewardGold = (int)(stageSO.Reward.Gold / stageSO.MaxPhase * (1 + 0.1f * enemyCount));
            }
            else
            {
                // 파밍 페이즈
                newPhase = new BonusPhase();

                rewardItem = new();
                if (Random.Range(0, 100) < 50)
                {
                    rewardItem.Add(stageSO.Reward.Items[Random.Range(0, stageSO.Reward.Items.Count)]);
                }
                rewardGold = stageSO.Reward.Gold / stageSO.MaxPhase;
            }
            newPhase.Rewards = new Reward(rewardItem, rewardGold);
            Phases[i] = newPhase;
        }
    }
}
