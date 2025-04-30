using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData
{
    public int Health;
    public int Pesos;
    public int HighestUnlockedLevel;
}

public class SavegameManager : MonoBehaviour
{
    public PlayerData playerData;
    string saveFilePath;

    void Start()
    {
        playerData = new PlayerData();
        saveFilePath = Application.persistentDataPath + "/PlayerData.json";
        NewGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SaveGame();

        if (Input.GetKeyDown(KeyCode.L))
            LoadGame();

        if (Input.GetKeyDown(KeyCode.D))
            DeleteSaveFile();

        if (Input.GetKeyDown(KeyCode.N))
            NewGame();

        if (Input.GetKeyDown(KeyCode.C))
            ChangeData();
    }

    public void SaveGame()
    {
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);

        Debug.Log("Save file created at: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);

            Debug.Log("Load game complete! \nPlayer Health: " + playerData.Health + ", Player Pesos: " + playerData.Pesos + ", Unlocked Levels: " + playerData.HighestUnlockedLevel + ")");
        }

        else
            Debug.Log("There is no save files to load!");
            
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);

            Debug.Log("Save file deleted!");
        }
        else
            Debug.Log("There is nothing to delete!");
    }

    public void NewGame()
    {
        playerData.Health = 100;
        playerData.Pesos = 5;
        playerData.HighestUnlockedLevel = 1;

        Debug.Log("Load game complete! \nPlayer Health: " + playerData.Health + ", Player Pesos: " + playerData.Pesos + ", Unlocked Levels: " + playerData.HighestUnlockedLevel + ")");
    }

    public void ChangeData()
    {
        playerData.Health = 42;
        playerData.Pesos = 123;
        playerData.HighestUnlockedLevel = 3;

        Debug.Log("Load game complete! \nPlayer Health: " + playerData.Health + ", Player Pesos: " + playerData.Pesos + ", Unlocked Levels: " + playerData.HighestUnlockedLevel + ")");
    }
}
