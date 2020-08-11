using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveUserData(LocalUser localUser)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "HarrySpotterLocalData");
        FileStream stream = new FileStream(path, FileMode.Create);

        LocalUserData data = new LocalUserData(localUser);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LocalUserData LoadUserData()
    {
        string path = Path.Combine(Application.persistentDataPath, "HarrySpotterLocalData");
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LocalUserData data = formatter.Deserialize(stream) as LocalUserData;
            stream.Close();

            return data;

        } else
        {
            Debug.Log("Save File not found");
            return null;
        }
    }
}
