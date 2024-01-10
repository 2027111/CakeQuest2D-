using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValueWrapper> values = new List<TValueWrapper>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void Add(TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            keys.Add(key);
            values.Add(new TValueWrapper(value));
            dictionary.Add(key, value);
        }
        else
        {
            // Handle the case where the key already exists
            Debug.LogWarning("Key already exists in the dictionary: " + key);
        }
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            yield return new KeyValuePair<TKey, TValue>(keys[i], values[i].Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [System.Serializable]
    private class TValueWrapper
    {
        public TValue Value;

        public TValueWrapper(TValue value)
        {
            Value = value;
        }
    }
}
