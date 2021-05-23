using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public List<Status> StatusList = new List<Status>();
    public static StatusManager statusManager;


    private void Awake()
    {
        if (statusManager == null)
        {
            statusManager = this;
        }
        else if (statusManager != this)
            Destroy(gameObject);
    }
    public void UpdateStatus(Characters c)
    {
        for(int i = StatusList.Count - 1; i >=0; i--)
        {
            if (c == StatusList[i].statusTarget)
            {
                StatusList[i].turnsActive--;
                if (StatusList[i].statusType == Status.StatusTypes.DOT)
                    c.TakeDamageDots(StatusList[i].statusElement, StatusList[i].dmg);
            }
            if (StatusList[i].turnsActive <= 0)
            {
                if(c == StatusList[i].statusTarget && StatusList[i].statusType == Status.StatusTypes.MARK)
                {
                    c.TakeDamageMark(StatusList[i].statusElement, StatusList[i].dmg);
                }
                Debug.Log("Status fini");
                StatusList[i].RevertStatus();
                StatusList.RemoveAt(i);
            }
        }
    }
}
