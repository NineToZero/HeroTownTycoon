using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializedDictionary<V> : Dictionary<int, V>, ISerializationCallbackReceiver
{
    [Serializable]
    public class KeyValue
    {
        public int Key;
        public V Value;

        public KeyValue(int key, V value)
        {
            Key = key;
            Value = value;
        }
    }
    
    [SerializeField]
    List<KeyValue> KeyValues = new();

    public void OnBeforeSerialize()
    {
        KeyValues.Clear();
        
        foreach (KeyValuePair<int, V> pair in this)
        {
            KeyValues.Add(new KeyValue(pair.Key, pair.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0, icount = KeyValues.Count; i < icount; ++i)
        {
            int key = KeyValues[i].Key;
            while (this.ContainsKey(key)) ++key;
            this.Add(key, KeyValues[i].Value);
        }
    }
}