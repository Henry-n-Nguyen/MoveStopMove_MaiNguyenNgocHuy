using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;

    private const string KEY_USER_DATA = "UserData";

    public UserData userData;

    public Player player;

    public bool isSave;

    private void Update()
    {
        if(isSave)
        {
            Save();
            isSave = false;
        }
    }

    private void Awake()
    {
        instance = this;

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(KEY_USER_DATA, "")))
        {
            userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(KEY_USER_DATA, ""));
        }
        else
        {
            userData = new();
        }
    }

    public void Load()
    {
        player.LoadDataFromUserData();
    }

    public void Save()
    {
        PlayerPrefs.SetString(KEY_USER_DATA, JsonUtility.ToJson(userData));
    }
}
