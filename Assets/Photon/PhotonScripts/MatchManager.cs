using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MatchManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1";
    public Button readyBtn;
    public string nickName;
    private int maxPlayer = 2;
    public GameObject loadingObj;
    public GameObject currentPlayerCount;


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingObj.SetActive(false);
    }

    void Start()
    {
        readyBtn.interactable = true;      
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }


    private void Update()
    {
        if (FindObjectOfType<TMP_InputField>().text == "")
        {
            readyBtn.interactable = false;
        }
        else
        {
            readyBtn.interactable = true;
        }
    }



    public void Match()
    {
        readyBtn.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            loadingObj.SetActive(true);
            nickName = FindObjectOfType<TMP_InputField>().text;
            PhotonNetwork.LocalPlayer.NickName = nickName;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayer;
            PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable(),roomOptions: roomOptions);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CancelMatching()
    {
        print("��Ī ���.");
        loadingObj.SetActive(false);

        print("�� ����.");
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerCounts()
    {
        if (currentPlayerCount.GetComponent<TextMeshProUGUI>() != null)
        {
            currentPlayerCount.GetComponent<TextMeshProUGUI>().text = $"{PhotonNetwork.CurrentRoom.PlayerCount}";
        }
    }

    public override void OnJoinedRoom()
    {
        print("�� ���� �Ϸ�.");

        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}�� �ο��� {PhotonNetwork.CurrentRoom.MaxPlayers} ��Ī ��ٸ��� ��.");
        UpdatePlayerCounts();

        loadingObj.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName} �� ����.");
        UpdatePlayerCounts();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel("Main");
            }
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    /*public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Match");
    }*/

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
