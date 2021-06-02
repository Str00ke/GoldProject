using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public NItem.ItemScriptableObject itemSo;
    public int price;

    private void Awake()
    {
        if (itemSo == null)
            Destroy(gameObject);

        transform.GetChild(1).GetComponent<Image>().sprite = Inventory.inventory.raritySprites[(int)itemSo.itemRarity];

        transform.GetChild(2).GetComponent<Image>().sprite = itemSo.itemUiSprite;
        Button imgButton = transform.GetChild(2).GetComponent<Button>();
        imgButton.onClick.AddListener(() => CharacterManager.characterManager.SelectItemStats(itemSo));
        imgButton.onClick.AddListener(() => LobbyManager.lobbyManager.ShopSelectedItem(transform.GetChild(0).gameObject));

        transform.GetChild(4).GetComponent<Text>().text = "Buy " + price + "G";
        Button textButton = transform.GetChild(4).GetComponent<Button>();
        textButton.onClick.AddListener(() => LobbyManager.lobbyManager.ItemToBuy(itemSo));
        textButton.onClick.AddListener(() => LobbyManager.lobbyManager.BuyItem(price));
    }
}
