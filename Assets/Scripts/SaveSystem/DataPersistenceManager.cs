using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string FileName;
    [SerializeField] private bool bUseEncyption;

    private GameData GameData;
    public static DataPersistenceManager instance { get; private set;}
    private List<IDataPersistence> DataPersistanceObjects;
    private FileDataHandler FileDataHandler;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Data Persistence manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName, bUseEncyption);
        DataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        GameData = new GameData();
    }

    public void LoadGame()
    {
        GameData = FileDataHandler.Load();

        if (GameData == null)
        {
            Debug.Log("No data found. Initializing data to defaults");
            NewGame();
        }

        foreach(IDataPersistence DataObjects in DataPersistanceObjects)
        {
            DataObjects.LoadData(GameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence DataObjects in DataPersistanceObjects)
        {
            DataObjects.SaveData(ref GameData);
        }

        FileDataHandler.Save(GameData);
    }

    private List<IDataPersistence> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistence> LocalDataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(LocalDataPersistanceObjects);
    }
}
