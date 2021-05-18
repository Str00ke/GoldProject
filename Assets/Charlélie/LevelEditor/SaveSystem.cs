using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    static string absPath = Application.dataPath + "/LevelsData";

    static Room GetRoomAt(Room[] rooms, LevelRoom levelRoom)
    {
        foreach(Room room in rooms)
        {
            if (room.pos == levelRoom.pos)
                return room;
        }
        return null;
    }
    

    public static void Save(Room[] rooms, int mapW, int mapH, int roomNbr, string lvlName)
    {
        Level level = new Level(mapW, mapH, roomNbr, lvlName);
        level.SetRoomArray();

        for (int i = 0; i < rooms.Length; ++i)
        {
            level.AddRoomToArray(rooms[i].pos, rooms[i].roomType);
        }

        int index = 0;  
        for (int i = 0; i < level.rooms.GetLength(1); ++i)
        {
            for (int k = 0; k < level.rooms.GetLength(0); ++k)
            {
                Debug.Log(level.rooms[k, i]);
                if (level.rooms[k, i] != null)
                {
                    Room room = GetRoomAt(rooms, level.rooms[k, i]);
                    Debug.Log("yee");
                    for (int j = 0; j < 4; ++j)
                    {
                        if (room.linkedRooms[j] != null)
                        {                           
                            Debug.Log(k + " " + i + " " + j + " " + room.linkedRooms[j]);
                            level.rooms[k, i].LinkRoom(room.linkedRooms[j].pos);
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
