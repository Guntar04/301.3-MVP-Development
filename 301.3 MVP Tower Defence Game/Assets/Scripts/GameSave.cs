using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class GameSave : MonoBehaviour
{
   [System.Serializable]
    public class TowerData
    {
        public string towerType;
        public Vector2 position;
    }

    public class GameData
    { 
      public List<TowerData> towers = new List<TowerData>();
        public int currentWave;
        public int playerHealth;
        public int playerMoney;
    }

    public GameObject[] towerPrefabs;
    public string saveFileName = "towerdefense_save.json";

    private GameData gamedata = new GameData();

    private void Start()
    {
        LoadGame();
    }

    public void RegisterTower(GameObject tower)
    {
        TowerData td = new TowerData
        {
            towerType = tower.name.Replace("(Clone)",""),
            position = tower.transform.position
        };

        gamedata.towers.Add(td);
    }

    public void SaveGame()
    { 
     // gameData.currentWave = FindAnyObjectByType<WaveManager>().currentWave;
     //   GameData.playerHealth = PlayerStats.Health;
      //  GameData.playerMoney = PlayerStats.Money;

        string json = JsonUtility.ToJson(gamedata, true);
        //File.WriteAllText(GetPath(), json);
       // Debug.Log("Game saved to " + GetPath());
    }

    public void LoadGame()
    {
       // if (!File.Exists(GetPath()))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

       // string json =File.ReadAllText(GetPath());
        //gamedata = JsonUtility.FromJson<GameData>(json);

       //
        
       // FindObjectsOfType<WaveManager>().currentWave = 

    }
}
