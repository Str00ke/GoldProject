using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager lobbyManager;

    public RectTransform mainCanvas;

    private void Awake()
    {
        LobbyManager[] lob = FindObjectsOfType<LobbyManager>();

        if (lob.Length > 1)
            Destroy(mainCanvas);

        lobbyManager = this;

        DontDestroyOnLoad(mainCanvas);
    }

    public void SwitchLobbyUI()
    {
        mainCanvas.transform.GetChild(0).gameObject.SetActive(!mainCanvas.transform.GetChild(0).gameObject.activeSelf);
    }
}
