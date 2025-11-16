using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomMain : MonoBehaviour
{
    public GameObject LobbyRoomList;
    public TMP_Text NolobbyRoomText;
    public TMP_Text roomText;

    public Button leaveRoomBtn;
    public Button gameStartBtn;
    public Button setReadyBtn;

    public UIRoomStatusList uiRoomStatusList;

    public string curStatus;

    void Start()
    {
        roomText.text = "room";
        gameStartBtn.gameObject.SetActive(false);

        gameStartBtn.onClick.AddListener(() =>
        {
            PhotonNetwork.LoadLevel("Room");
        });

        setReadyBtn.onClick.AddListener(() =>
        {
            uiRoomStatusList.SetStatus(PhotonNetwork.NickName);
        });

        leaveRoomBtn.onClick.AddListener(() => { LeaveRoom(); });
    }

    public void OnRoom()
    {
        leaveRoomBtn.gameObject.SetActive(true);
        
        Debug.Log($"<color=red>{PhotonNetwork.CountOfRooms}</color>");

        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);
            Debug.Log("현재 방 열려있는지 : " + PhotonNetwork.CurrentRoom.IsOpen);
            Debug.Log("현재 방 비공개 여부 : " + PhotonNetwork.CurrentRoom.IsVisible);

        }

        Debug.Log($"<color=red>IsMasterClient: {PhotonNetwork.IsMasterClient}</color>");

        if (PhotonNetwork.IsMasterClient == true)
        {
            gameStartBtn.gameObject.SetActive(true);
            setReadyBtn.gameObject.SetActive(false);
        }
        else
        {
            gameStartBtn.gameObject.SetActive(false);
            setReadyBtn.gameObject.SetActive(true);
        }

        uiRoomStatusList.contentPointion.gameObject.SetActive(true);
        uiRoomStatusList.Create();
    }


    public void LeaveRoom()
    {
        Debug.Log("방에서 나갑니다");

        PhotonNetwork.LeaveRoom();
        uiRoomStatusList.Create();
        PhotonNetwork.LoadLevel("MyLobby");

    }
}
