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
    public static LobbyManager Instance;
  
    private string gameVersion = "1";
    public GameObject inputNickName;

    public string myName;
    private Player[] players = PhotonNetwork.PlayerList;
    public TMP_Text lobbyText;

    public UINicknameView uiNicknameView;
    public GameObject LobbyRoomList;
    public TMP_Text NolobbyRoomText;

    public Button createRoomBtn;
    public Button checkBtn;

    public Action<List<RoomInfo>> roomChagne;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {

        roomChagne = OnRoomListUpdate;

        createRoomBtn.onClick.AddListener(() => { CreateRoom(); });
        checkBtn.onClick.AddListener(() =>
        {
            Debug.Log($"현재 캐싱된 방 갯수: {cachedRoomList.Count}");
        });

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
        Debug.Log("OnConnectedToMaster");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        lobbyText.text = "Room";       

        Debug.Log("OnJoinedRoom");
        Debug.Log($"IsMasterClient: {PhotonNetwork.IsMasterClient}");

        Debug.Log(PhotonNetwork.NickName);

        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log($"{players[i].NickName} 입장");
        }

        LobbyRoomList.SetActive(false);
        NolobbyRoomText.gameObject.SetActive(false);

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

        roomChagne(cachedRoomList);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed {returnCode}, {message}");
    }

    public void SetNickName(string nickName)
    {
        PhotonNetwork.ConnectUsingSettings();

        myName = nickName;
        PhotonNetwork.NickName = nickName;
        
        Connect();
    }

    public void CreateRoom()
    {
        createRoomBtn.gameObject.SetActive(false);
        Debug.Log("방을 만듭니다.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        cachedRoomList = roomList;
        Debug.Log($"OnRoomListUpdate rawCount: {roomList.Count}");
    }


}
