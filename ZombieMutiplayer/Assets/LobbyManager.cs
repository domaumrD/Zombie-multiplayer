using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public Button btn;

    public GameManager gameManager;

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

        if(PhotonNetwork.IsMasterClient == true)
        {
            Debug.Log($"[{gameManager.nickName}] 님이 입장했습니다.");
        }
        else
        {
            Debug.Log($"[{gameManager.nickName}] 님이 입장했습니다.");
        }

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
