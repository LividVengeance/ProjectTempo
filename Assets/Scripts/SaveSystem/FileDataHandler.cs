using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string DataDirPath = "";
    private string DataFileName = "";

    private bool bUseEncryption = false;
    private readonly string EncryptionCode = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool bUseEncryption)
    {
        DataDirPath = dataDirPath;
        DataFileName = dataFileName;
        this.bUseEncryption = bUseEncryption;
    }

    public GameData Load()
    {
        string FullPath = Path.Combine(DataDirPath, DataFileName);
        GameData LoadedData = null;
        if (File.Exists(FullPath))
        {
            try
            {
                string DataToLoad = "";

                using (FileStream Stream = new FileStream(FullPath, FileMode.Open))
                {
                    using (StreamReader Reader = new StreamReader(Stream))
                    {
                        DataToLoad = Reader.ReadToEnd();    
                    }
                }

                if (bUseEncryption)
                {
                    DataToLoad = EncryptDecrypt(DataToLoad);
                }

                LoadedData = JsonUtility.FromJson<GameData>(DataToLoad);
            }
            catch
            {

            }
        }
        return LoadedData;
    }

    public void Save(GameData InGameData)
    {
        string FullPath = Path.Combine(DataDirPath, DataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

            string DataToStore = JsonUtility.ToJson(InGameData, true);

            if (bUseEncryption)
            {
                DataToStore = EncryptDecrypt(DataToStore);
            }

            using (FileStream Stream = new FileStream(FullPath, FileMode.Create))
            {
                using (StreamWriter Writer = new StreamWriter(Stream))
                { 
                    Writer.Write(DataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occoured when trying to save data to file: " + FullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string InData)
    {
        string ModifiedData = "";
        for (int Index = 0; Index < InData.Length; Index++)
        {
            ModifiedData += (char)(InData[Index] ^ EncryptionCode[Index % EncryptionCode.Length]);
        }
        return ModifiedData;
    }
}
