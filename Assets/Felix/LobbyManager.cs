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
    Credits,
    Options,
    Dungeons,
    Loading
}

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager lobbyManager;

    public ELobbyState lobbyState = ELobbyState.Menu;

    public RectTransform mainCanvas;

    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject DungeonsMenu;
    public GameObject ShopMenu;

    private NItem.ItemScriptableObject itemToBuy;
    private GameObject itemsShopSelected = null;

    [Header("Loading Screen")]
    private AsyncOperation loadingAsync;
    public GameObject loadingScene;
    public Slider loadingSlider;
    public RectTransform charSpriteLoading;

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

        lobbyState = ELobbyState.Menu;

        CharacterManager.characterManager.RefreshTeamScene();
        CharacterManager.characterManager.SelectCharacterStats(CharacterManager.characterManager.selectedChar);
    }

    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
        lobbyState = ELobbyState.Options;
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
        lobbyState = ELobbyState.Credits;
    }

    public void OpenDungeons()
    {
        DungeonsMenu.SetActive(true);
        lobbyState = ELobbyState.Dungeons;

        CharacterManager.characterManager.SelectCharacterStats(CharacterManager.characterManager.selectedChar);
    }

    public void OpenShop()
    {
        ShopMenu.SetActive(true); 

        lobbyState = ELobbyState.Shop;

        CharacterManager.characterManager.SelectCharacterStats(CharacterManager.characterManager.selectedChar);
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
        loadingAsync.allowSceneActivation = false;

        float sliderSize = loadingSlider.GetComponent<RectTransform>().sizeDelta.x;

        while (!loadingAsync.isDone)
        {
            loadingSlider.value = loadingAsync.progress;

            charSpriteLoading.anchoredPosition = new Vector2((loadingSlider.value < 0.5f ? -1f : 1f) * (sliderSize / 2f) * (1 - loadingSlider.value), charSpriteLoading.anchoredPosition.y);

            if (loadingAsync.progress >= 0.9f)
            {
                loadingSlider.value = 0.99f;
                charSpriteLoading.anchoredPosition = new Vector2(sliderSize / 2f, charSpriteLoading.anchoredPosition.y);
                loadingAsync.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScene.SetActive(false);
        SwitchLobbyUI();
    }
}
