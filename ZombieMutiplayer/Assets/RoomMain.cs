using Photon.Pun;
using System.Xml.Linq;
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

    public UIRoomStatusList uiRoomStatusList;

    void Start()
    {
        roomText.text = "room";
        gameStartBtn.gameObject.SetActive(false);

        gameStartBtn.onClick.AddListener(() =>
        {
            PhotonNetwork.LoadLevel("Room");
        });

        leaveRoomBtn.onClick.AddListener(() => { LeaveRoom(); });
    }

    public void OnRoom()
    {
        leaveRoomBtn.gameObject.SetActive(true);
        gameStartBtn.gameObject.SetActive(false);
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

        if (PhotonNetwork.IsMasterClient)
        {
            gameStartBtn.gameObject.SetActive(true);
        }

        uiRoomStatusList.Create();
    }


    public void LeaveRoom()
    {
        Debug.Log("방에서 나갑니다");
     
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MyLobby");
       
    }
}
