using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveTime {
    static string filePathDirectory = Application.persistentDataPath + "/CatnessOverload.dat";


    public static bool SaveExist()
    {
        if (File.Exists(filePathDirectory)) {
            return true;
        }
        return false;
    }

    public static void SaveTimeData(float time)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(filePathDirectory, FileMode.Create);

        bf.Serialize(file, time);
        file.Close();
    }

    public static float LoadTimeData()
    {
        if (SaveExist()) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePathDirectory, FileMode.Open);
            float data = (float)bf.Deserialize(fs);
            fs.Close();
            return data;
        }
        else {
            return -1;
        }
    }

    public static void DeleteData()
    {
        if (SaveExist()) {
            File.Delete(filePathDirectory);
        }
    }

}
