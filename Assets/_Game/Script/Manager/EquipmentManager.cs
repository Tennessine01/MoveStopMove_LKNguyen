using System.Collections.Generic;
using UnityEngine;

public class EquipemtManager : Singleton<EquipemtManager> 
{
    [System.Serializable]
    public struct PrefabEntry
    {
        public string id;
        public GameObject prefab;
    }

    [SerializeField]
    private List<PrefabEntry> prefabEntries;

    private Dictionary<string, GameObject> prefabDictionary;

    void Awake()
    {
        prefabDictionary = new Dictionary<string, GameObject>();
        foreach (PrefabEntry entry in prefabEntries)
        {
            if (!prefabDictionary.ContainsKey(entry.id))
            {
                prefabDictionary.Add(entry.id, entry.prefab);
            }
        }
    }

    public GameObject GetPrefabById(string id)
    {
        if (prefabDictionary.TryGetValue(id, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError("PrefabManager: No prefab found with ID {id}.");
            return null;
        }
    }

    public GameObject InstantiatePrefabById(string id, Transform position)
    {
        GameObject prefab = GetPrefabById(id);
        if (prefab != null)
        {
            return Instantiate(prefab,position);
        }
        return null;
    }
}