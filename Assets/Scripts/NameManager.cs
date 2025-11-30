using UnityEngine;
using TMPro;
using System.IO;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;
    public TMP_InputField Name;

    public string PlayerName;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //DeleteSaveFile();
        LoadName();
    }

    [System.Serializable]
    class SaveData
    {
        public string Name;
    }

    public void SaveName()
    {
        SaveData data = new SaveData();
        data.Name = Name.text;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savenamefile.json", json);
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savenamefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            PlayerName = data.Name;
            if (Name != null) Name.text = data.Name;
        }
    }

    public string GetPlayerName()
    {
        LoadName();
        Debug.Log(PlayerName);
        return PlayerName;
    }

    public void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/savenamefile.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }

}
