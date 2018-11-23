using UnityEngine;
using System.IO;

public class Settings
{
    public string NetworkAddress;
}


public class SettingManager : Singleton<SettingManager>
{

    Settings setting = new Settings();

    public Settings Setting { 
        get {
            return setting; 
        }
    }

    private void Awake()
    {

        DontDestroyOnLoad(this);

        if (instance != null)
            Destroy(gameObject);

        Setting.NetworkAddress = "localhost";   //기본값

        if (File.Exists(System.Environment.CurrentDirectory + "/Settings.json"))
        {
            string jsonSetting = File.ReadAllText(System.Environment.CurrentDirectory + "/Settings.json");

            setting = JsonUtility.FromJson<Settings>(jsonSetting);
        }
    }    
}
