using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IndividualityStat", menuName = "Scriptable Object/IndividualityStat")]
public class IndividualityStatSO : ScriptableObject
{
    [SerializeField] public char[] FirstName;
    [SerializeField] public char[] MiddleName;
    [SerializeField] public char[] LastName;

    [SerializeField] public Job[] Jobs;
    [SerializeField] public BaseIndividualityStatData[] Origins;
    [SerializeField] public Nature[] Natures;
    [SerializeField] public BaseIndividualityStatData[] Flavors;
}


[Serializable]
public class BaseIndividualityStatData
{
    public string Name;
    public int id;
}

[Serializable]
public class Job : BaseIndividualityStatData
{
    public int[] SpriteIds;
}

[Serializable]
public class Nature : BaseIndividualityStatData
{
    public Stats PositiveStat;
    public Stats NegativeStat;
}