using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SOToSave : MonoBehaviour
{
    public SO so;
}

[System.Serializable]
public class data
{
    //string _tString;
    //float _tFloat;
    //testSOC _tTestSoc;
    public string _tSOName;

    public data(SO _so)
    {
        //_tString = _so.tString;
        //_tFloat = _so.tFloat;
        //_tTestSoc = _so.tTestSOC;
        _tSOName = _so.name;
    }
}
