using NUnit.Framework.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public Button btn;
    public string myName;
    public List<string> nameList = new List<string>();


    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        btn.interactable = false;
        btn.onClick.AddListener(() =>
        {
            Debug.Log("룸 접속 요청");
            Connect();
        });
    }

    private void Connect()
    {
        Debug.Log($"Isconnected:  {PhotonNetwork.IsConnected}");

        if (PhotonNetwork.IsConnected)
        {
            nameList.Add(myName);

            Debug.Log("test");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("failed");
            PhotonNetwork.ConnectUsingSettings();
        }
    }   

    public override void OnConnectedToMaster()
    {
        btn.interactable = true;
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

        PhotonNetwork.NickName = myName;
        Debug.Log($"nick: {PhotonNetwork.NickName}");

        PhotonNetwork.LoadLevel("Main");
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

}
