using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadController : MonoBehaviour
{
    /*private static SaveLoadController instance;

    private string filePath;

    private void Awake()
    {
        // Ensure there is only one instance of SaveLoadController
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        filePath = Path.Combine(Application.persistentDataPath, "panelData.json");
    }

    public static void SavePanelData(Dictionary<string, PanelData> panelData)
    {
        string json = JsonUtility.ToJson(new SerializableDictionary<string, PanelData>(panelData));
        File.WriteAllText(instance.filePath, json);
    }

    public static Dictionary<string, PanelData> LoadPanelData()
    {
        if (File.Exists(instance.filePath))
        {
            string json = File.ReadAllText(instance.filePath);
            SerializableDictionary<string, PanelData> data = JsonUtility.FromJson<SerializableDictionary<string, PanelData>>(json);
            return data.ToDictionary();
        }

        return new Dictionary<string, PanelData>();
    }
}

[System.Serializable]
public class PanelData
{
    public Vector3 position;
    public Vector3 scale;

    // Add this constructor
    public PanelData(Vector3 position, Vector3 scale)
    {
        this.position = position;
        this.scale = scale;
    }
}

// A helper class to serialize dictionaries with non-primitive types as keys
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> target = new Dictionary<TKey, TValue>();

    public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        target = dictionary;
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return target;
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in target)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        target = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            target[keys[i]] = values[i];
        }
    }*/
}
