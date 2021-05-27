using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public static class SaveSystem
{
    static string absPath = Application.streamingAssetsPath/* + "/LevelsData"*/;
    

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
        FileStream file = File.Create(absPath + "/" + level.name + ".level");

        /*if (!Directory.Exists(absPath.ToString()))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/LevelsData");
            Debug.Log("Directory not founded, so created at: " + absPath);
        }*/


        /*string path = absPath + "/" + level.name + ".level";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, level);

        stream.Close();*/
        formatter.Serialize(file, level);
        file.Close();

        //Debug.Log("Saved in: " + path);
    }

    public static Level Load(string levelName)
    {

        /*string path = absPath + "/" + levelName + ".level";
        if (File.Exists(path))
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream stream = new FileStream(path, FileMode.Open);

            //Level data = formatter.Deserialize(stream) as Level;
            //stream.Close();

            //return data;

            FileStream file = File.Open(path, FileMode.Open);
            Level data = (Level)formatter.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            Debug.LogError("Error: Save file not found in: " + path);
            return null;
            
        }*/

        /*if (Application.platform == RuntimePlatform.Android)
        {
            string path = "jar:file:///" + Application.dataPath + "!/assets/Level1.level";
            WWW wwwfile = new WWW(path);
            while (!wwwfile.isDone) { }
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "alphabet.t");
            File.WriteAllBytes(Application.persistentDataPath + "Level1.level", wwwfile.bytes);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "Level1.level", FileMode.Open);
            Level data = (Level)formatter.Deserialize(file);
            file.Close();
            return data;
        } else
        {
            string path = absPath + "/" + levelName + ".level";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                //FileStream stream = new FileStream(path, FileMode.Open);

                //Level data = formatter.Deserialize(stream) as Level;
                //stream.Close();

                //return data;

                FileStream file = File.Open(path, FileMode.Open);
                Level data = (Level)formatter.Deserialize(file);
                file.Close();
                return data;
            }
            else
            {
                Debug.LogError("Error: Save file not found in: " + path);
                return null;

            }
        }*/

        if (Application.platform == RuntimePlatform.Android)
        {
            /*string path = "jar:file:///" + Application.dataPath + "!/assets/Level1.level";
            UnityWebRequest request = UnityWebRequest.Get(path);
            byte[] results = request.downloadHandler.data;
            Level test = (Level)ByteArrayToObject(results);
            return test;*/
            string path = "jar:file://" + Application.dataPath + "!/assets/Level1.level";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            Debug.Log("Done downloading");

            byte[] yourBytes = www.bytes;
            Level test = /*ByteArrayToObject(yourBytes)*/ReatFile(yourBytes);
            return test;

        }
        else
        {

            string path = absPath + "/" + levelName + ".level";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                //FileStream stream = new FileStream(path, FileMode.Open);

                //Level data = formatter.Deserialize(stream) as Level;
                //stream.Close();

                //return data;

                FileStream file = File.Open(path, FileMode.Open);
                Level data = (Level)formatter.Deserialize(file);
                file.Close();
                return data;
            }
            else
            {
                Debug.LogError("Error: Save file not found in: " + path);
                return null;

            }
        }
            

    }

    public static Level ReatFile(byte[] arr)
    {
        //I have to read the file which I have wrote to an byte array            

        //And now is what I have to do with the byte array of file is to convert it back to object which I have wrote it into a file
        //I am using MemoryStream to convert byte array back to the original object.
        MemoryStream memStream = new MemoryStream(arr);
        BinaryFormatter binForm = new BinaryFormatter();
        Level obj = (Level)binForm.Deserialize(memStream);
        return obj;
    }

    public static Level ByteArrayToObject(byte[] _ByteArray)
    {
        /*try
        {
            // convert byte array to memory stream
            System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_ByteArray);
            // create new BinaryFormatter
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(); 
            // set memory stream position to starting point
            _MemoryStream.Position = 0;
            // Deserializes a stream into an object graph and return as a object.
            return _BinaryFormatter.Deserialize(_MemoryStream);
        }
        catch (IOException _Exception)
        {
            // Error
            Debug.Log("Exception caught in process: {0}", _Exception.ToString());
        }
        // Error occured, return null
        return null;*/
        // convert byte array to memory stream
        MemoryStream _MemoryStream = new MemoryStream(_ByteArray);
        
        // create new BinaryFormatter
        BinaryFormatter _BinaryFormatter = new BinaryFormatter();
        // set memory stream position to starting point
        //_MemoryStream.Position = 0;
        // Deserializes a stream into an object graph and return as a object.
        Level level = (Level)_BinaryFormatter.Deserialize(_MemoryStream);
        return level;
    }

}

public class StaticCoroutine : MonoBehaviour
{
    static public StaticCoroutine instance; //the instance of our class that will do the work

    void Awake()
    { //called when an instance awakes in the game
        instance = this; //set our static reference to our newly initialized instance
    }

    IEnumerator PerformCoroutine()
    { //the coroutine that runs on our monobehaviour instance
        while (true)
        {
            yield return 0;
        }
    }

    static public void DoCoroutine()
    {
        instance.StartCoroutine("PerformCoroutine"); //this will launch the coroutine on our instance
    }
}
