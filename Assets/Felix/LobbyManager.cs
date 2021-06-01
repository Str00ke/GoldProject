using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager lobbyManager;

    public RectTransform mainCanvas;

    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject DungeonsMenu;
    public GameObject ShopMenu;

    private NItem.ItemScriptableObject itemToBuy;

    private void Awake()
    {
        LobbyManager[] lob = FindObjectsOfType<LobbyManager>();

        if (lob.Length > 1)
            Destroy(mainCanvas);

        lobbyManager = this;

        DontDestroyOnLoad(mainCanvas);
    }

    private void Start()
    {
        CloseAllMenu();
    }

    public void SwitchLobbyUI()
    {
        mainCanvas.transform.GetChild(0).gameObject.SetActive(!mainCanvas.transform.GetChild(0).gameObject.activeSelf);
        CloseAllMenu();
    }

    public void CloseAllMenu()
    {
        OptionsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        DungeonsMenu.SetActive(false);
        Inventory.inventory.CloseInventory();
        ShopMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
    }

    public void OpenDungeons()
    {
        DungeonsMenu.SetActive(true);
    }

    public void ItemToBuy(NItem.ItemScriptableObject item)
    {
        itemToBuy = item;
    }

    public void BuyItem(int price)
    {
        if (price > Inventory.inventory.golds)
        {
            itemToBuy = null;
            return;
        }

        if (itemToBuy == null)
            return;

        Inventory.inventory.AddGolds(-price);

        Inventory.inventory.AddItem(itemToBuy);
        itemToBuy = null;
    }
}
