using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;

    private string gameVersion = "1";
    [HideInInspector]
    public bool reconnet = false;

    public string myName;
    private Player[] players = PhotonNetwork.PlayerList;

    public Dictionary<string, RoomInfo> cachedRooms = new Dictionary<string, RoomInfo>();
    public UIRoomList uiRoomList;
    public MyLobbyMain myLobbyMain;
    public RoomMain roomMain;

    private void Awake()
    {

        if (Instance == null)
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
        PhotonNetwork.AutomaticallySyncScene = true;      
        PhotonNetwork.GameVersion = gameVersion;  
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        Debug.Log($"Isconnected:  {PhotonNetwork.IsConnected}");

        if (PhotonNetwork.IsConnected)
        {
            myLobbyMain.inputNickName.SetActive(false);
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
        myLobbyMain = FindFirstObjectByType<MyLobbyMain>();
        myLobbyMain.inputNickName.gameObject.SetActive(false);
        myLobbyMain.SetLobby();

        uiRoomList = FindFirstObjectByType<UIRoomList>();
        Debug.Log($"내가 로비에 있는지 확인: {PhotonNetwork.InLobby}");
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");      

        if (reconnet == true)
        {
            PhotonNetwork.JoinLobby();
            reconnet = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        roomMain = FindFirstObjectByType<RoomMain>();
        //lobbyText.text = "Room";

        Debug.Log("OnJoinedRoom");
       
        Debug.Log(PhotonNetwork.NickName);

        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log($"{players[i].NickName} 입장");
        }

        roomMain.LobbyRoomList.SetActive(false);
        roomMain.NolobbyRoomText.gameObject.SetActive(false);

        //PhotonNetwork.LoadLevel("Main");
        roomMain.OnRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRoomFailed {returnCode}, {message}");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, IsVisible = true, IsOpen = true });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed {returnCode}, {message}");
    }
        

    public void ToJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }       

    public override void OnLeftRoom()
    {
        reconnet = true;
        Debug.Log("방에서 나가기 호출 ");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        Debug.Log("RoomList Update");

        uiRoomList.Remove();

        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList || info.PlayerCount == 0)
            {               
                cachedRooms.Remove(info.Name);
            }
            else
            {               
                cachedRooms[info.Name] = info;
            }
        }

        // 2) 전체 캐시 기준으로 리스트 만들기
        List<RoomInfo> allRooms = new List<RoomInfo>(cachedRooms.Values);
        Debug.Log($"OnRoomListUpdate rawCount: {roomList.Count}, cachedCount: {allRooms.Count}");

        uiRoomList.Create();


        myLobbyMain.SetLobbyRoomText(allRooms.Count);
    }
}