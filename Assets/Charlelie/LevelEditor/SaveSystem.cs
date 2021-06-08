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
        Level level = new Level(mapW, mapH, roomNbr, lvlName, LevelType.EASY);
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
            string path = "jar:file://" + Application.dataPath + "!/assets/" + LevelManager.GetInstance().levelName + ".level";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            Debug.Log("Done downloading");

            byte[] yourBytes = www.bytes;
            Level test = /*ByteArrayToObject(yourBytes)*/ReadFile(yourBytes);
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

    public static Level ReadFile(byte[] arr)
    {
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

    public static void SaveInventory()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/InventoryList.data";

            //Stream fileStream = File.Open(filename, FileMode.Create, FileAccess.Write);
        }
        else
        {
            path = absPath + "/InventoryList.data";
        }

            
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }
        Debug.Log(Inventory.inventory.itemList.Count);
        if (Inventory.inventory.itemList.Count <= 0) return;

        //can be optimized with using int key or hash instead of dict => prevent iterate through list by instantiate object with the key.
        List<Dictionary<string, NItem.ERarity>> itemsList = new List<Dictionary<string, NItem.ERarity>>(Inventory.inventory.itemList.Count);
        for (int i = 0; i < Inventory.inventory.itemList.Count; ++i)
        {
            itemsList.Add(new Dictionary<string, NItem.ERarity>() 
            {
                { Inventory.inventory.itemList[i].GetComponent<ItemInInventory>().item.itemName, Inventory.inventory.itemList[i].GetComponent<ItemInInventory>().item.itemRarity } 
            });            
        }
        //itemsList.AddRange(LoadInventory());
        Debug.Log(itemsList.Count);
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.ReadWrite,
                                       FileShare.ReadWrite);
        formatter.Serialize(stream, itemsList);
        Debug.Log("Saved in: " + path);
        stream.Close();
    }

    public static List<Dictionary<string, NItem.ERarity>> LoadInventory()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath + "/InventoryList.data";
            Debug.Log("Try to load: " + path);
            if (File.Exists(path)) Debug.Log("File exist");
            else
            {
                Debug.Log("File don't exist");
                File.Create(path);
            }
            Stream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
            if (fileStream.Length <= 0) return new List<Dictionary<string, NItem.ERarity>>();
            BinaryFormatter formatter = new BinaryFormatter();
            List<Dictionary<string, NItem.ERarity>> list = (List<Dictionary<string, NItem.ERarity>>)formatter.Deserialize(fileStream);
            Debug.Log(list.Count);
            fileStream.Close();
            //List<Dictionary<string, NItem.ERarity>> list = ReadItems(results);
            if (list.Count <= 0)
            {
                Debug.Log("This bich empty. YEET!!");
                return new List<Dictionary<string, NItem.ERarity>>();
            } else
            {
                Debug.Log(list.Count);
                List<Dictionary<string, NItem.ERarity>> itemList = list;
                return itemList;
                    

            }
            
        } else
        {
            string path = absPath + "/InventoryList.data";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.ReadWrite,
                                       FileShare.ReadWrite);
                if (file.Length <= 0) return new List<Dictionary<string, NItem.ERarity>>();
                List<Dictionary<string, NItem.ERarity>> data = formatter.Deserialize(file) as List<Dictionary<string, NItem.ERarity>>;
                Debug.Log(data.Count);
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

    public static void SaveMoney()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/Money.data";
        }
        else
        {
            path = absPath + "/Money.data";
        }


        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }

        (int, int) money = (Inventory.inventory.golds, Inventory.inventory.souls);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.ReadWrite,
                                       FileShare.ReadWrite);
        formatter.Serialize(stream, money);
        Debug.Log("Saved in: " + path);
        stream.Close();
    }

    public static (int, int) LoadMoney()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath + "/Money.data";
            Debug.Log("Try to load: " + path);
            if (File.Exists(path)) Debug.Log("File exist");
            else
            {
                Debug.Log("File don't exist");
                File.Create(path);
            }
            Stream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            (int, int) data = ((int, int))formatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        else
        {
            string path = absPath + "/Money.data";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.ReadWrite,
                                       FileShare.ReadWrite);
                (int, int) data = ((int, int))formatter.Deserialize(file);
                file.Close();
                return data;
            }
            else
            {
                Debug.LogError("Error: Save file not found in: " + path);
                return (0, 0);
            }
        }
    }

    public static void SavePlayers()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/Chars.data";
        }
        else
        {
            path = absPath + "/Chars.data";
        }


        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }

        CharSave[] chars = new CharSave[3];
        for (int i = 0; i < 3; ++i)
        {
            if (CharacterManager.characterManager.AskForCharacter(i) != null)
                chars[i] = new CharSave(CharacterManager.characterManager.AskForCharacter(i), i);
        }


        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.ReadWrite,
                                       FileShare.ReadWrite);
        formatter.Serialize(stream, chars);
        Debug.Log("Saved in: " + path);
        stream.Close();
    }

    public static CharSave[] LoadPlayers()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath + "/Chars.data";
            Debug.Log("Try to load: " + path);
            if (File.Exists(path)) Debug.Log("File exist");
            else
            {
                Debug.Log("File don't exist");
                File.Create(path);
            }
            Stream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            CharSave[] data = (CharSave[])formatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        else
        {
            string path = absPath + "/Chars.data";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = new FileStream(path,
                                       FileMode.Open,
                                       FileAccess.ReadWrite,
                                       FileShare.ReadWrite);
                if (file.Length <= 0) return null;
                else Debug.Log(file.Length);
                CharSave[] data = (CharSave[])formatter.Deserialize(file);
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

    public static List<Dictionary<string, NItem.ERarity>> ReadItems(byte[] arr)
    {
        MemoryStream memStream = new MemoryStream(arr);
        BinaryFormatter binForm = new BinaryFormatter();
        List<Dictionary<string, NItem.ERarity>> obj = (List<Dictionary<string, NItem.ERarity>>)binForm.Deserialize(memStream);
        return obj;
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
