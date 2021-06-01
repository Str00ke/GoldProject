using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusTuto : MonoBehaviour
{
    public GameObject prefabIconStatus;
    public List<StatusTuto1> StatusList = new List<StatusTuto1>();
    public static StatusTuto statusTuto;
    public Sprite buffStatusSprite;
    public Sprite debuffStatusSprite;
    public Sprite dotStatusSprite;
    public Sprite markStatusSprite;
    public Sprite stunStatusSprite;
    public float statusOffset = 0.13f;
    public int statusId;

    private void Awake()
    {
        if (statusTuto == null)
        {
            statusTuto = this;
        }
        else if (statusTuto != this)
            Destroy(gameObject);
    }
    public void UpdateStatus(CharactersTuto c)
    {
        for (int i = StatusList.Count - 1; i >= 0; i--)
        {
            if (c == StatusList[i].statusTarget)
            {
                StatusList[i].turnsActive--;
                if (StatusList[i].statusType == StatusTuto1.StatusTypes.DOT && StatusList[i].statusType == StatusTuto1.StatusTypes.BLEEDING)
                    c.TakeDamageDots(StatusList[i].statusElement, StatusList[i].dmg);
            }
            if (StatusList[i].turnsActive <= 0)
            {
                if (c == StatusList[i].statusTarget && StatusList[i].statusType == StatusTuto1.StatusTypes.MARK)
                {
                    c.TakeDamageMark(StatusList[i].statusElement, StatusList[i].dmg);
                }
                Debug.Log("Status fini");
                StatusList[i].RevertStatus();
                StatusList.RemoveAt(i);
            }
        }
    }

    public void AddDisplayStatus(CharactersTuto c, StatusTuto1 status)
    {

        GameObject temp = Instantiate(prefabIconStatus);
        temp.name = "Status" + status.statusId;
        temp.transform.localPosition = new Vector3(1, 1, 1);
        temp.transform.SetParent(c.canvasChar.transform);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        if (c.statusPerLine >= c.statusPerLineMax)
        {
            c.statusLines++;
            c.statusPerLine = 0;
        }
        temp.transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * (c.prefabsIconStatus.Count - c.statusPerLineMax * c.statusLines)), c.debuffsInitialPos.y - (statusOffset * c.statusLines));
        c.prefabsIconStatus.Add(temp);
        if (status.statusType == StatusTuto1.StatusTypes.DOT || status.statusType == StatusTuto1.StatusTypes.BLEEDING)
            temp.GetComponent<Image>().sprite = dotStatusSprite;
        else if (status.statusType == StatusTuto1.StatusTypes.MARK)
            temp.GetComponent<Image>().sprite = markStatusSprite;
        else if (status.statusType == StatusTuto1.StatusTypes.STUN)
            temp.GetComponent<Image>().sprite = stunStatusSprite;
        else if (status.buffOrDebuff == StatusTuto1.BuffOrDebuff.BUFF)
            temp.GetComponent<Image>().sprite = buffStatusSprite;
        else
            temp.GetComponent<Image>().sprite = debuffStatusSprite;


        c.statusPerLine++;
    }
    public void DeleteDisplayStatus(CharactersTuto c, StatusTuto1 status)
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
            c.prefabsIconStatus[i].transform.position = c.transform.position + new Vector3(c.debuffsInitialPos.x + (statusOffset * (i - c.statusPerLineMax * c.statusLines)), c.debuffsInitialPos.y - (statusOffset * (i / c.statusPerLineMax)));
            if (c.statusPerLine >= c.statusPerLineMax)
            {
                c.statusLines++;
                c.statusPerLine = 0;
            }
            c.statusPerLine++;
        }
    }
}
