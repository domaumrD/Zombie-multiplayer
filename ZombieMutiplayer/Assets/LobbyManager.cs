using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;

    private string gameVersion = "1";
    private bool reconnet = false;

    public GameObject inputNickName;

    public string myName;
    private Player[] players = PhotonNetwork.PlayerList;
    public TMP_Text lobbyText;

    public UINicknameView uiNicknameView;
    public GameObject LobbyRoomList;
    public TMP_Text NolobbyRoomText;

    public Button createRoomBtn;
    public Button leaveRoomBtn;
    public Button checkBtn;

    public Dictionary<string, RoomInfo> cachedRooms = new Dictionary<string, RoomInfo>();

    public UIRoomList uiRoomList;
    public GameObject uiLoading;

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
        createRoomBtn.onClick.AddListener(() => { CreateRoom(); });
        leaveRoomBtn.onClick.AddListener(() => { LeaveRoom(); });

        checkBtn.onClick.AddListener(() =>
        {

            Debug.Log($"내가 로비에 있는지 확인: {PhotonNetwork.InLobby}");
            Debug.Log($"내가 룸에 있는지 확인: {PhotonNetwork.InRoom}");
            Debug.Log($"데이터 캐시 갯수 : {cachedRooms.Count}");            

            foreach (RoomInfo room in cachedRooms.Values)
            {
                Debug.Log($"{room.Name}");
            }

        });

        lobbyText.text = "Title";
        PhotonNetwork.GameVersion = gameVersion;
        inputNickName.SetActive(true);
        LobbyRoomList.SetActive(false);
        NolobbyRoomText.gameObject.SetActive(false);
        createRoomBtn.gameObject.SetActive(false);
        leaveRoomBtn.gameObject.SetActive(false);
        uiLoading.gameObject.SetActive(false);
        uiNicknameView.onClickSubmit = SetNickName;
        PhotonNetwork.ConnectUsingSettings();
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

        uiLoading.gameObject.SetActive(true);

        if (PhotonNetwork.InLobby == true)
        {
            uiLoading.gameObject.SetActive(false);
            lobbyText.text = "Lobby";
            LobbyRoomList.SetActive(true);
            NolobbyRoomText.gameObject.SetActive(true);
            createRoomBtn.gameObject.SetActive(true);
            Debug.Log("I'm in Lobby");

        }
       

        Debug.Log($"내가 로비에 있는지 확인: {PhotonNetwork.InLobby}");
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");      

        if (reconnet == true)
        {
            PhotonNetwork.JoinLobby();
        }
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
        OnRoom();
    }

   

    public void OnRoom()
    {
        leaveRoomBtn.gameObject.SetActive(true);
        Debug.Log($"{PhotonNetwork.CountOfRooms}");

        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);
            Debug.Log("현재 방 열려있는지 : " + PhotonNetwork.CurrentRoom.IsOpen);
            Debug.Log("현재 방 비공개 여부 : " + PhotonNetwork.CurrentRoom.IsVisible);

        }
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

    public void SetNickName(string nickName)
    {
        PhotonNetwork.ConnectUsingSettings();

        myName = nickName;
        PhotonNetwork.NickName = nickName;

        Connect();
    }

    public void ToJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void CreateRoom()
    {
        createRoomBtn.gameObject.SetActive(false);
        Debug.Log("방을 만듭니다.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, IsVisible = true });

    }

    public void LeaveRoom()
    {        
        Debug.Log("방에서 나갑니다");
        Debug.Log(myName);
        PhotonNetwork.LeaveRoom();
        leaveRoomBtn.gameObject.SetActive(false);
        createRoomBtn.gameObject.SetActive(true);
        lobbyText.text = "Room";
        NolobbyRoomText.gameObject.SetActive(true);
        LobbyRoomList.gameObject.SetActive(true);

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

    }
}