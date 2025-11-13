using NUnit.Framework.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public GameObject inputNickName;

    public string myName;
    private Player[] players = PhotonNetwork.PlayerList;
    public TMP_Text lobbyText;

    public UINicknameView uiNicknameView;
    public GameObject LobbyRoomList;
    public TMP_Text NolobbyRoomText;

    public Button createRoomBtn;

    void Start()
    {
        lobbyText.text = "Title";
        PhotonNetwork.GameVersion = gameVersion;
        inputNickName.SetActive(true);
        LobbyRoomList.SetActive(false);
        NolobbyRoomText.gameObject.SetActive(false);
        createRoomBtn.gameObject.SetActive(false);
        uiNicknameView.onClickSubmit = SetNickName;

    }

    private void Connect()
    {

        Debug.Log($"Isconnected:  {PhotonNetwork.IsConnected}");

        if (PhotonNetwork.IsConnected)
        {
            inputNickName.SetActive(false);

            Debug.Log("test");
            //PhotonNetwork.JoinRandomRoom();
          
            JoinLobby();
        }
        else
        {
            Debug.Log("failed");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
        OnJoinedLobby();
    }

    public override void OnJoinedLobby()
    {
        lobbyText.text = "Lobby";
        LobbyRoomList.SetActive(true);
        NolobbyRoomText.gameObject.SetActive(true);
        createRoomBtn.gameObject.SetActive(true);
        Debug.Log("I'm in Lobby");  
    }


    public override void OnConnectedToMaster()
    {

        //btn.gameObject.SetActive(true);
        //btn.interactable = true;
      
        Debug.Log("OnConnectedToMaster");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log($"IsMasterClient: {PhotonNetwork.IsMasterClient}");

        //PhotonNetwork.NickName = myName;

        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log($"{players[i].NickName} 입장");
        }

        //PhotonNetwork.LoadLevel("Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRoomFailed {returnCode}, {message}");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed {returnCode}, {message}");
    }

    public void SetNickName(string nickName)
    {
        myName = nickName;
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.ConnectUsingSettings();
        Connect();
    }

}
