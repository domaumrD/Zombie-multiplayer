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
            Debug.Log($"현재 갯수 : {PhotonNetwork.CountOfRooms}");
            //Debug.Log($"현재 캐싱된 방 갯수: {cachedRooms.Count}");


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

    public void CreateRoom()
    {
        createRoomBtn.gameObject.SetActive(false);
        Debug.Log("방을 만듭니다.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, IsVisible = true });

    }

    public void LeaveRoom()
    {
        Debug.Log("방에서 나갑니다");
        PhotonNetwork.LeaveRoom();
        leaveRoomBtn.gameObject.SetActive(false);
        createRoomBtn.gameObject.SetActive(true);
        lobbyText.text = "Room";
        NolobbyRoomText.gameObject.SetActive(true);
        LobbyRoomList.gameObject.SetActive(true);

        uiRoomList.Remove();

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList || info.PlayerCount == 0)
            {
                // 아무도 없거나 삭제된 방은 캐시에서 제거
                cachedRooms.Remove(info.Name);
            }
            else
            {
                // 존재하는 방이면 캐시에 추가/갱신
                cachedRooms[info.Name] = info;
            }
        }

        // 2) 전체 캐시 기준으로 리스트 만들기
        List<RoomInfo> allRooms = new List<RoomInfo>(cachedRooms.Values);

        Debug.Log($"OnRoomListUpdate rawCount: {roomList.Count}, cachedCount: {allRooms.Count}");

        uiRoomList.Create();

    }
}