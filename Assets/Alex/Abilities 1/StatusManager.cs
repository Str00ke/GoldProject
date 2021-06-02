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
            Status s = StatusList[i];
            if (c == c.statusList[i].statusTarget)
            {
                c.statusList[i].turnsActive--;
                if (c.statusList[i].statusType == Status.StatusTypes.DOT || c.statusList[i].statusType == Status.StatusTypes.BLEEDING)
                    c.TakeDamageDots(c.statusList[i].statusElement, c.statusList[i].dmg);
            }
            if (c.statusList[i].turnsActive <= 0)
            {
                if (c == c.statusList[i].statusTarget && c.statusList[i].statusType == Status.StatusTypes.MARK)
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
        GameObject temp = Instantiate(prefabIconStatus);
        temp.name = "Status" + status.statusId;
        temp.transform.localPosition = new Vector3(1, 1, 1);
        temp.transform.SetParent(c.canvasChar.transform);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        if(c.statusPerLine >= c.statusPerLineMax)
        {
            c.statusLines++;
            c.statusPerLine = 0;
        }
        temp.transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * (c.prefabsIconStatus.Count - c.statusPerLineMax * c.statusLines)), c.debuffsInitialPos.y - (statusOffset  * c.statusLines));
        c.prefabsIconStatus.Add(temp);
        if (status.statusType == Status.StatusTypes.DOT || status.statusType == Status.StatusTypes.BLEEDING)
            temp.GetComponent<Image>().sprite = dotStatusSprite;
        else if(status.statusType == Status.StatusTypes.MARK)
            temp.GetComponent<Image>().sprite = markStatusSprite;
        else if (status.statusType == Status.StatusTypes.STUN)
            temp.GetComponent<Image>().sprite = stunStatusSprite;
        else if (status.statusType == Status.StatusTypes.DEFENCE)
            temp.GetComponent<Image>().sprite = defenceStatusSprite;
        else if(status.buffOrDebuff == Status.BuffOrDebuff.BUFF)
            temp.GetComponent<Image>().sprite = buffStatusSprite;
        else
            temp.GetComponent<Image>().sprite = debuffStatusSprite;


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

        c.statusLines = 0;
        c.statusPerLine = 0;
        for (int i = c.prefabsIconStatus.Count - 1; i >= 0; i--)
        {
            c.prefabsIconStatus[i].transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * (i - c.statusPerLineMax * c.statusLines)), c.debuffsInitialPos.y - (statusOffset * (i/c.statusPerLineMax)));
            if (c.statusPerLine >= c.statusPerLineMax)
            {
                c.statusLines++;
                c.statusPerLine = 0;
            }
            c.statusPerLine++;
        }
    }
}
