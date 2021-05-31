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
        for(int i = StatusList.Count - 1; i >=0; i--)
        {
            if (c == StatusList[i].statusTarget)
            {
                StatusList[i].turnsActive--;
                if (StatusList[i].statusType == Status.StatusTypes.DOT && StatusList[i].statusType == Status.StatusTypes.BLEEDING)
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

    public void AddDisplayStatus(Characters c, Status status)
    {
        GameObject temp = Instantiate(prefabIconStatus);
        temp.name = "Status" + status.statusId;
        temp.transform.localPosition = new Vector3(1, 1, 1);
        temp.transform.SetParent(c.canvasChar.transform);
        // temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        if (status.statusType == Status.StatusTypes.DOT || status.statusType == Status.StatusTypes.BLEEDING)
        {
            //temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.Count), c.debuffsInitialPos.y);
            temp.transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.Count), c.debuffsInitialPos.y);
            c.prefabsIconStatusDebuffs.Add(temp);
            temp.GetComponent<Image>().sprite = dotStatusSprite;
        }
        else if(status.statusType == Status.StatusTypes.MARK)
        {
            //temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.Count), c.debuffsInitialPos.y);
            temp.transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.Count), c.debuffsInitialPos.y);
            c.prefabsIconStatusDebuffs.Add(temp);
            temp.GetComponent<Image>().sprite = markStatusSprite;
        }
        else
        {
            if (status.buffOrDebuff == Status.BuffOrDebuff.BUFF)
            {
                //temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(c.buffsInitialPos.x - (statusOffset * c.prefabsIconStatusBuffs.Count), c.buffsInitialPos.y);
                temp.transform.position = c.transform.position + new Vector3(c.buffsInitialPos.x - (statusOffset * c.prefabsIconStatusBuffs.Count), c.buffsInitialPos.y);
                c.prefabsIconStatusBuffs.Add(temp);
                temp.GetComponent<Image>().sprite = buffStatusSprite;
            }
            else
            {
                //temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.Count), c.debuffsInitialPos.y);
                temp.transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.Count), c.debuffsInitialPos.y);
                c.prefabsIconStatusDebuffs.Add(temp);
                temp.GetComponent<Image>().sprite = debuffStatusSprite;
            }
        }
    }
    public void DeleteDisplayStatus(Characters c, Status status)
    {
        if(status.buffOrDebuff == Status.BuffOrDebuff.BUFF)
        {
            for (int i = c.prefabsIconStatusBuffs.Count - 1; i >= 0; i--)
            {
                if ("Status" + status.statusId == c.prefabsIconStatusBuffs[i].name)
                {
                    GameObject temp = c.prefabsIconStatusBuffs[i];
                    c.prefabsIconStatusBuffs.Remove(temp);
                    Destroy(temp);
                }
            }
            for (int i = c.prefabsIconStatusBuffs.Count - 1; i >= 0; i--)
            {
                // c.prefabsIconStatusBuffs[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(c.buffsInitialPos.x + (statusOffset * i), c.buffsInitialPos.y);
                c.prefabsIconStatusBuffs[i].transform.position = c.transform.position + new Vector3(c.buffsInitialPos.x + (statusOffset * i), c.buffsInitialPos.y);
            }
        }
        if (status.buffOrDebuff == Status.BuffOrDebuff.DEBUFF)
        {
            for (int i = c.prefabsIconStatusDebuffs.Count - 1; i >= 0; i--)
            {
                if ("Status" + status.statusId == c.prefabsIconStatusDebuffs[i].name)
                {
                    GameObject temp = c.prefabsIconStatusDebuffs[i];
                    c.prefabsIconStatusDebuffs.Remove(temp);
                    Destroy(temp);
                }
            }
            foreach (GameObject g in c.prefabsIconStatusDebuffs)
            {
                // g.GetComponent<RectTransform>().anchoredPosition = new Vector2(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.IndexOf(g)), c.debuffsInitialPos.y);
                g.transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * c.prefabsIconStatusDebuffs.IndexOf(g)), c.debuffsInitialPos.y);
            }
        }
    }
}
