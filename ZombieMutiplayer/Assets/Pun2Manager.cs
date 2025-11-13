using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Pun2Manager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public static Pun2Manager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Init()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();   //마스터 서버 접속 요청
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터서버에 접속 했습니다.");

        //이벤트 전송 
        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnConnectedToMaster);

        //로비 입장
        JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장 했습니다.");
        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnJoinedLobby);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"OnRoomListUpdate: {roomList.Count}");
        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnRoomListUpdate, roomList);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom : {PhotonNetwork.CurrentRoom.Name}");

        Debug.Log(PhotonNetwork.IsMasterClient);    //방장

        
        var me = PhotonNetwork.LocalPlayer;
     

        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p == me) continue;
            Debug.Log($"[{p.NickName}]님이 입장 했습니다.");
        }


        Debug.Log($"[{PhotonNetwork.NickName}]님이 입장 했습니다.");

        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnJoinedRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[{newPlayer.NickName}]님이 입장 했습니다.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRandomFailed: {returnCode}, {message}");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed : {returnCode}, {message}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[{otherPlayer.NickName}]님이 퇴장 했습니다.");
    }

    public override void OnLeftRoom()
    {
        Debug.Log($"[{PhotonNetwork.NickName}]님이 방을 나갔습니다.");

        Debug.Log($"PhotonNetwork.InLobby: {PhotonNetwork.InLobby}");
    }

    public void CreateRoom()
    {
        Debug.Log("방을 만듭니다.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}