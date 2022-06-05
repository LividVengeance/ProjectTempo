using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveAllPlayerData(CharacterController PlayerCharacter)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        string SavePath = Application.persistentDataPath + "/player.save";
        FileStream Stream = new FileStream(SavePath, FileMode.Create);

        SaveData Data = new SaveData(PlayerCharacter);

        Formatter.Serialize(Stream, Data);
        Stream.Close();
    }

    public static SaveData LoadData()
    {
        string SavePath = Application.persistentDataPath + "/player.save";
        if (!File.Exists(SavePath))
        {
            BinaryFormatter BinaryFormatter = new BinaryFormatter();
            FileStream Stream = new FileStream(SavePath, FileMode.Open);
            
            SaveData Data =  BinaryFormatter.Deserialize(Stream) as SaveData;
            Stream.Close();
            return Data;
        }
        else
        {
            Debug.LogError("No Save Data File Could Be Loaded From Path " + SavePath);
            return null;
        }
    }
}
