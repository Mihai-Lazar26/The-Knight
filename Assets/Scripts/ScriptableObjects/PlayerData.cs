using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public string saveName = "";
    public Vector2 playerPosition;
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public int sceneIndex = 1;
    public void SavePlayer() {
        SaveSystem.SavePlayer(saveName, this);
    }

    public void LoadPlayer() {
        PlayerDataSerialized playerData = SaveSystem.LoadPlayer(saveName);
        if (playerData == null) {
            SetDefault();
            return;
        }
        playerMaxHealth = playerData.playerMaxHealth;
        playerCurrentHealth = playerData.playerCurrentHealth;

        playerPosition = new Vector2(playerData.playerPosition[0], playerData.playerPosition[1]);

        sceneIndex = playerData.sceneIndex;
    }

    public void SetDefault() {
        playerPosition = new Vector2(0, 0);
        playerMaxHealth = 100;
        playerCurrentHealth = playerMaxHealth;
        sceneIndex = 1;
    }

    public bool Status() {
        return SaveSystem.PlayerDataExists(saveName);
    }

    public bool DeleteData() {
        return SaveSystem.DeletePlayerData(saveName);
    }
}
