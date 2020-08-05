using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void Save<T>(T save)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + typeof(T).ToString() + ".bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static T Load<T>()
    {
        string path = Application.persistentDataPath + "/" + typeof(T).ToString() + ".bin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            T stats = (T)formatter.Deserialize(stream);
            stream.Close();
            return stats;
        }
        else
        {
            Debug.LogError("Save file not found in: " + path);
            return default;
        }
    }
}
