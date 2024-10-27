using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataSerialized
{
    public float[] playerPosition;
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public int sceneIndex;

    public PlayerDataSerialized (PlayerData player) {
        playerMaxHealth = player.playerMaxHealth;
        playerCurrentHealth = player.playerCurrentHealth;
        playerPosition = new float[2];
        playerPosition[0] = player.playerPosition.x;
        playerPosition[1] = player.playerPosition.y;
        sceneIndex = player.sceneIndex;
    }
}
