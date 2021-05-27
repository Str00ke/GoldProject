using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.Networking;

public class test : MonoBehaviour
{
    public GameObject testTxt;
    // Start is called before the first frame update
    void Start()
    {
        /*#if UNITY_ANDROID
                string path = "jar:file://" + Application.dataPath + "!/assets/alphabet.txt";
                WWW wwwfile = new WWW(path);
                while (!wwwfile.isDone) { }
                var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "alphabet.t");
                File.WriteAllBytes(filepath, wwwfile.bytes);

                StreamReader wr = new StreamReader(filepath);
                string line;
                while ((line = wr.ReadLine()) != null)
                {
                    testTxt.GetComponent<Text>().text = line;
                }
        #endif*/

        /*string path = "jar:file://" + Application.dataPath + "!/assets/Level1.level";
        UnityWebRequest request = UnityWebRequest.Get(path);
        byte[] results = request.downloadHandler.data;
        testTxt.GetComponent<Text>().text = request.downloadHandler.data.Length.ToString();*/
        //StartCoroutine(CopyFile());

        string path = "jar:file://" + Application.dataPath + "!/assets/Level1.level";
        WWW www = new WWW(path);


        Debug.Log("Done downloading");

        byte[] yourBytes = www.bytes;
        Debug.Log($"Unity HERE lenght:: {www.bytes.Length}");
        testTxt.GetComponent<Text>().text = www.bytes.Length.ToString();


        /*string fileName = Application.streamingAssetsPath + "/Level1";
        FileInfo fi = new FileInfo(fileName);

        if (fi.Exists)
        {
            // Get file size  
            long size = fi.Length;
            testTxt.GetComponent<Text>().text = size.ToString();
        }
        else
        {
            Debug.Log("File not exist!!");
        }*/
    }

    /*IEnumerator CopyFile()
    {
        string path = "jar:file://" + Application.dataPath + "!/assets/Level1.level";
        WWW www = new WWW(path);

        while (!www.isDone)
        {
            //Must yield below/wait for a frame
            yield return null;
        }

        Debug.Log("Done downloading");

        byte[] yourBytes = www.bytes;
        Debug.Log($"Unity HERE lenght:: {www.bytes.Length}");
        testTxt.GetComponent<Text>().text = www.bytes.Length.ToString();
        //Now Save it
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
