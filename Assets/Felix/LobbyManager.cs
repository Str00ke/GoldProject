using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ELobbyState
{
    Menu,
    Inventory,
    InventoryItemPartSelection,
    Shop,
    Loading
}

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager lobbyManager;

    public ELobbyState lobbyState = ELobbyState.Menu;

    public RectTransform mainCanvas;

    public GameObject ShopMenu;
    public Transform shopButton;
    public Button playButton;

    private NItem.ItemScriptableObject itemToBuy;
    private GameObject itemsShopSelected = null;

    public bool isFirstGameDone = false;

    [Header("Loading Screen")]
    private AsyncOperation loadingAsync;
    public GameObject loadingScene;
    public Image loadingSlider;

    private void Awake()
    {

        if (lobbyManager != null)
        {
            /*if (!LobbyManager.lobbyManager.mainCanvas.transform.GetChild(0).gameObject.activeSelf)
                LobbyManager.lobbyManager.SwitchLobbyUI();*/

            Destroy(mainCanvas.gameObject);
        }
        else
        {
            lobbyManager = this;
            DontDestroyOnLoad(mainCanvas.gameObject);
        }
    }

    private void Start()
    {
        isFirstGameDone = bool.Parse(PlayerPrefs.GetString("FirstGame", "false"));
        CloseAllMenu();
        //LevelManager.GetInstance().UpdateDataValues();        
        LoadData();
    }

    public void SwitchLobbyUI()
    {
        mainCanvas.transform.GetChild(0).gameObject.SetActive(!mainCanvas.transform.GetChild(0).gameObject.activeSelf);
        CloseAllMenu();
    }

    public void CloseAllMenu()
    {
        RefreshLockedButton();

        Inventory.inventory.CloseInventory();
        ShopMenu.SetActive(false);

        lobbyState = ELobbyState.Menu;

        CharacterManager.characterManager.RefreshTeamScene();
        CharacterManager.characterManager.SelectCharacterStats(CharacterManager.characterManager.selectedChar);
    }

    public void RefreshLockedButton()
    {
        if (!isFirstGameDone)
            isFirstGameDone = bool.Parse(PlayerPrefs.GetString("FirstGame", "false"));

        if (isFirstGameDone && shopButton.GetChild(0).gameObject.activeSelf)
        {
            shopButton.GetChild(0).gameObject.SetActive(false);
        }

        CharacterManager characterManager = CharacterManager.characterManager;

        if (!characterManager.AskForCharacter(0) && !characterManager.AskForCharacter(1) && !characterManager.AskForCharacter(2))
            playButton.interactable = false;
        else
            playButton.interactable = true;
    }

    public void Play()
    {
        LoadScene("Level");
    }

    public void OpenShop()
    {
        if (!isFirstGameDone)
            return;

        ShopMenu.SetActive(true); 

        lobbyState = ELobbyState.Shop;

        CharacterManager.characterManager.SelectCharacterStats(CharacterManager.characterManager.selectedChar);
        RectTransform content = ShopMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        Vector2 size;
        size.x = content.sizeDelta.x;

        int nbItems = content.transform.childCount + 5;
        while (nbItems % 5 != 0)
            nbItems--;

        size.y = nbItems / 5 * 225f - 25f;
        content.sizeDelta = size;
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

    public void ShopSelectedItem(GameObject item)
    {
        if (itemsShopSelected != null)
            itemsShopSelected.SetActive(false);

        itemsShopSelected = item;

        if (itemsShopSelected != null)
            itemsShopSelected.SetActive(true);
    }

    public void ShopUnselectItem()
    {
        if (itemsShopSelected != null)
            itemsShopSelected.SetActive(false);

        itemsShopSelected = null;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingScreen(sceneName));
        lobbyState = ELobbyState.Loading;
    }

    IEnumerator LoadingScreen(string sceneName)
    {
        loadingScene.SetActive(true);
        loadingAsync = SceneManager.LoadSceneAsync(sceneName);
        loadingAsync.allowSceneActivation = false; //Might cause Crash!!!

        while (!loadingAsync.isDone)
        {
            loadingSlider.fillAmount = loadingAsync.progress;

            if (loadingAsync.progress >= 0.9f)
            {
                loadingSlider.fillAmount = 1f;
                loadingAsync.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScene.SetActive(false);
        SwitchLobbyUI();
    }

    public void AddScoreToLeaderboard()
    {
        //int fVal = LevelData.GetSouls() + Inventory.inventory.souls;
        PlayGamesController.PostToLeaderboard(Inventory.inventory.souls);
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        SaveData();
    }

    public void SaveData()
    {
        SaveSystem.SaveInventory();
        SaveSystem.SaveMoney();
        SaveSystem.SavePlayers();
    }

    public void LoadData()
    {
        Inventory.inventory.LoadInventory();
        Inventory.inventory.LoadMoney();
        CharacterManager.characterManager.LoadCharacters();
        AddScoreToLeaderboard();
    }

    /*public void SaveInventory()
    {
        //SaveSystem.SaveInventory();
        //SaveSystem.SaveMoney();
        SaveSystem.SavePlayers();
    }

    public void LoadInventory()
    {
        Inventory.inventory.LoadInventory();
        Inventory.inventory.LoadMoney();
    }*/
}
