using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence 
{
    void LoadData(GameData InGameData);

    void SaveData(ref GameData InGameData);
}
