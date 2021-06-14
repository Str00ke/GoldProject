using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public GameObject prefabIconStatus;
    public List<Status> StatusList = new List<Status>();
    public static StatusManager statusManager;
    public Sprite buffStatusSprite;
    public Sprite debuffStatusSprite;
    public Sprite dotStatusSprite;
    public Sprite markStatusSprite;
    public Sprite stunStatusSprite;
    public Sprite defenceStatusSprite;
    public float statusOffset = 0.13f;
    public int statusId;

    [Header("UI Status")]
    public GameObject panelStatus;

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
        for (int i = c.statusList.Count - 1; i >= 0; i--)
        {
            if (c == c.statusList[i].statusTarget)
            {
                c.statusList[i].turnsActive--;
                if (c.statusList[i].statusType == Status.StatusTypes.Dot || c.statusList[i].statusType == Status.StatusTypes.Bleeding)
                    c.TakeDamageDots(c.statusList[i].statusElement, c.statusList[i].dmg);
            }
            if (c.statusList[i].turnsActive <= 0)
            {
                if (c == c.statusList[i].statusTarget && c.statusList[i].statusType == Status.StatusTypes.Mark)
                {
                    c.TakeDamageMark(c.statusList[i].statusElement, c.statusList[i].dmg);
                }
                c.statusList[i].RevertStatus();
                StatusList.RemoveAt(i);
            }
        }
    }

    public void AddDisplayStatus(Characters c, Status status)
    {
        GameObject temp = Instantiate(prefabIconStatus, c.statusLayoutGroup.transform);
        temp.name = "Status" + status.statusId;
        c.prefabsIconStatus.Add(temp);
        if (status.statusType == Status.StatusTypes.Dot || status.statusType == Status.StatusTypes.Bleeding)
        {
            temp.GetComponent<Image>().sprite = dotStatusSprite;
            status.statusSprite = dotStatusSprite;
        }
        else if(status.statusType == Status.StatusTypes.Mark)
        {
            temp.GetComponent<Image>().sprite = markStatusSprite;
            status.statusSprite = markStatusSprite;
        }
        else if (status.statusType == Status.StatusTypes.Stun)
        {
            temp.GetComponent<Image>().sprite = stunStatusSprite;
            status.statusSprite = stunStatusSprite;
        }
        else if (status.statusType == Status.StatusTypes.Defence)
        {
            temp.GetComponent<Image>().sprite = defenceStatusSprite;
            status.statusSprite = defenceStatusSprite;
        }
        else if(status.buffOrDebuff == Status.BuffOrDebuff.BUFF)
        {
            temp.GetComponent<Image>().sprite = buffStatusSprite;
            status.statusSprite = buffStatusSprite;
        }
        else
        {
            temp.GetComponent<Image>().sprite = debuffStatusSprite;
            status.statusSprite = debuffStatusSprite;
        }


        c.statusPerLine++;
    }
    public void DeleteDisplayStatus(Characters c, Status status)
    {
        for (int i = c.prefabsIconStatus.Count - 1; i >= 0; i--)
        {
            if ("Status" + status.statusId == c.prefabsIconStatus[i].name)
            {
                GameObject temp = c.prefabsIconStatus[i];
                c.prefabsIconStatus.Remove(temp);
                Destroy(temp);
            }
        }
    }

    public void DisplayStatusCharacter(Status[] arrStatus)
    {

    }
}
