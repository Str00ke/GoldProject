using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class SO : ScriptableObject
{
    public string tString;
    public float tFloat;
    public testSOC tTestSOC;
    public string tName;

    SO()
    {
        tTestSOC = new testSOC();
    }
}