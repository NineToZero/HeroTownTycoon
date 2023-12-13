using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Synergy
{
    public string Name;
    public List<int> RequiredOriginCode;
    public List<StatValue> Effects;
}

[CreateAssetMenu(fileName = "Synergy", menuName = "Scriptable Object/Synergy")]
public class SynergySO : ScriptableObject
{
    [SerializeField] private List<Synergy> _synergies;

    private Dictionary<int, Synergy> _synergyDict = new();
    private Dictionary<Stats, int> _effectDict = new();
    private List<Synergy> _synergyList = new();
    public void Init()
    {
        foreach(var synergy in _synergies)
        {
            int hash = 0;
            foreach (var originCode in synergy.RequiredOriginCode)
            {
                hash += 1 << originCode % 100;
            }
            _synergyDict.Add(hash, synergy);
        }
    }

    public List<Synergy> GetSynergiesForOrigin(int originHash)
    {
        if (_synergyDict.Count == 0) Init();

        _synergyList.Clear();

        foreach (int hash in _synergyDict.Keys)
        {
            if ((hash & originHash) == hash)
                _synergyList.Add(_synergyDict[hash]);
        }

        return _synergyList;
    }

    public Dictionary<Stats, int> GetEffectsForOrigin(int originHash)
    {
        if (_synergyDict.Count == 0) Init();

        _effectDict.Clear();

        foreach (int hash in _synergyDict.Keys)
        {
            if ((hash & originHash) == hash)
            {
                foreach(var effect in _synergyDict[hash].Effects)
                {
                    if (!_effectDict.ContainsKey(effect.Stat)) _effectDict.Add(effect.Stat, (int)effect.Value);
                    else _effectDict[effect.Stat] += (int)effect.Value;
                }
            }
        }

        return _effectDict;
    }
}