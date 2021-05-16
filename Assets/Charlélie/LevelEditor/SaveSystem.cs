using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    static string absPath = Application.dataPath + "/LevelsData";

    /*public static void SaveLevelData(LDData levelData)
    {


        int name = Directory.GetFiles(absPath, "*.data", SearchOption.TopDirectoryOnly).Length;
        BinaryFormatter formatter = new BinaryFormatter();

        string path = absPath + "/" + name + ".data";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(levelData);

        formatter.Serialize(stream, data);


        stream.Close();

        Debug.Log("Saved in: " + path);
    }



    public static LevelData LoadLevelData()
    {


        Random.InitState((int)System.DateTime.Now.Ticks);

        int fCount = Directory.GetFiles(absPath, "*.data", SearchOption.TopDirectoryOnly).Length - 1;
        int name = Random.Range(0, fCount);

        string path = absPath + "/" + name + ".data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Error: Save file not found in: " + path);
            return null;
        }
    }*/

    public static void Save(Room[] rooms, int mapW, int mapH, int roomNbr, string lvlName)
    {
        Level level = new Level(mapW, mapH, roomNbr, lvlName);
        level.SetRoomArray();

        for (int i = 0; i < rooms.Length; ++i)
        {
            level.AddRoomToArray(rooms[i].pos, RoomType.BASE);
        }

        int index = 0;
        for (int i = 0; i < level.rooms.GetLength(1); ++i)
        {
            for (int k = 0; k < level.rooms.GetLength(0); ++k)
            {
                if (level.rooms[k, i] != null)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        if (rooms[index].linkedRooms[j] != null)
                        {
                            Debug.Log(k + " " + i + " " + j + " " +rooms[index].linkedRooms[j]);
                            level.rooms[k, i].LinkRoom(rooms[index].linkedRooms[j].pos);
                        }                       
                    }
                    index++;
                }                                
            }               
        }


        BinaryFormatter formatter = new BinaryFormatter();

        string path = absPath + "/" + level.name + ".level";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, level);

        stream.Close();

        Debug.Log("Saved in: " + path);
    }

    public static Level Load(string levelName)
    {

        string path = absPath + "/" + levelName + ".level";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Level data = formatter.Deserialize(stream) as Level;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Error: Save file not found in: " + path);
            return null;
        }
    }


}
