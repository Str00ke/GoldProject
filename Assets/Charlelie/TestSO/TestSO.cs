using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



[System.Serializable]
public class testSOC
{
    int tIntC = 42;
    string tStringC = "Je suis un classe";
    public string tName = "Test01";
}

public class TestSO : MonoBehaviour
{

    public SOToSave[] goToSave;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        foreach(SOToSave go in goToSave)
        {
            data data = new data(go.so); 
            BinaryFormatter formatter = new BinaryFormatter();

            string path = System.IO.Directory.GetCurrentDirectory() + "/Assets/Charlelie/TestSO/Save"  + go.so.tString + ".level";
            Debug.Log(path);
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);

            stream.Close();
        }
        //Debug.Log("<b><size=15> <color=#0AA374>All objects of type  </color><color=#CD1426FF>" + goToSave.ToString() + "</color>" + "<color=#0392CF>: " + goToSave.Length + "  </color> </size></b>");
    }

    public void Load()
    {
        for (int i = 1; i < 4; ++i)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Assets/Charlelie/TestSO/SO" + i + ".level";
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data data = formatter.Deserialize(stream) as data;
            stream.Close();
        }
        
    }
}
