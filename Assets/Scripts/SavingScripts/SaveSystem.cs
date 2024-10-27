using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SavePlayer (string saveName, PlayerData player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player_" + saveName + ".save";

        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerDataSerialized playerData = new PlayerDataSerialized(player);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerDataSerialized LoadPlayer(string saveName) {
        string path = Application.persistentDataPath + "/player_" + saveName + ".save";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerDataSerialized playerData = formatter.Deserialize(stream) as PlayerDataSerialized;

            stream.Close();

            return playerData;
        }
        else {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    public static bool PlayerDataExists(string saveName) {
        string path = Application.persistentDataPath + "/player_" + saveName + ".save";
        return File.Exists(path);
    }

    public static bool DeletePlayerData(string saveName) {
        string path = Application.persistentDataPath + "/player_" + saveName + ".save";
        if (File.Exists(path)) {
            try {
                File.Delete(path);
                Debug.Log("File deleted succesfully.");
                return true;
            }
            catch (IOException e) {
                Debug.Log("An error occurred while deleting the file: " + e.Message);
                return false;
            }
        }
        return true;
    }

    public static void SaveSettings(string saveName, SettingsData settings) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings_" + saveName + ".save";

        FileStream stream = new FileStream(path, FileMode.Create);
        SettingsDataSerialized settingsData = new SettingsDataSerialized(settings);

        formatter.Serialize(stream, settingsData);
        stream.Close();
    }

    public static SettingsDataSerialized LoadSettings(string saveName) {
        string path = Application.persistentDataPath + "/settings_" + saveName + ".save";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsDataSerialized data = formatter.Deserialize(stream) as SettingsDataSerialized;

            stream.Close();

            return data;
        }
        else {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
