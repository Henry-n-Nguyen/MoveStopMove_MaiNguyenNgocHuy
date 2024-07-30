using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private const string KEY_USER_DATA = "UserData";

    public UserData userData;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(KEY_USER_DATA, "")))
        {
            userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(KEY_USER_DATA, ""));
        }
        else
        {
            userData = new();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    // Load / Save
    public void LoadData()
    {
        CharacterManager.Ins.player.LoadDataFromUserData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString(KEY_USER_DATA, JsonUtility.ToJson(userData));
    }
}
