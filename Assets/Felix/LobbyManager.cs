using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager lobbyManager;

    public RectTransform mainCanvas;

    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject DungeonsMenu;

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
    }

    public void CloseAllMenu()
    {
        OptionsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        DungeonsMenu.SetActive(false);
        Inventory.inventory.CloseInventory();
        // shop close
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
}
