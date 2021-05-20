using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    List<Status> StatusList;
    public static StatusManager statusManager;
    public void UpdateStatus()
    {
        foreach(Status s in StatusList)
        {
            s.turns--;
        }
    }
}
