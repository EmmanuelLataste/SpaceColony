using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{


    public static void SaveData (CharacterController playerController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "dataSaves";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataSave data = new DataSave(playerController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataSave LoadData ()
    {
        string path = Application.persistentDataPath + "dataSaves";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataSave data = formatter.Deserialize(stream) as DataSave;
            stream.Close();
            return data;

        }

        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


}

