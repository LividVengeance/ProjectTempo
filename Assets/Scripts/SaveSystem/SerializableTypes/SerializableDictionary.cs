using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> LocalKeys = new List<TKey>();
    [SerializeField] private List<TValue> LocalValues = new List<TValue>();

    public void OnAfterDeserialize()
    {
        LocalKeys.Clear();
        LocalValues.Clear(); 
        foreach (KeyValuePair<TKey, TValue> Pair in this)
        {
            LocalKeys.Add(Pair.Key);
            LocalValues.Add(Pair.Value);
        }
    }

    public void OnBeforeSerialize()
    {
        this.Clear();
        for (int Index = 0; Index < Keys.Count; Index++)
        {
            this.Add(LocalKeys[Index], LocalValues[Index]);
        }
    }
}
