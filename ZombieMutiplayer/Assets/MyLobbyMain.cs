using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyLobbyMain : MonoBehaviour
{
    public GameObject inputNickName;
    public string myName;
    public TMP_Text lobbyText;

    public UINicknameView uiNicknameView;
    public GameObject LobbyRoomList;
    public TMP_Text NolobbyRoomText;

    public Button createRoomBtn; 
    public Button checkBtn;

    public UIRoomList uiRoomList;
    public GameObject uiLoading;
      

    void Start()
    {
        lobbyText.text = "Title";

        inputNickName.SetActive(true);
        LobbyRoomList.SetActive(false);
       
        NolobbyRoomText.gameObject.SetActive(false);
        createRoomBtn.gameObject.SetActive(false);
      
        uiLoading.gameObject.SetActive(false);
        createRoomBtn.onClick.AddListener(() => { CreateRoom(); });
           

        lobbyText.text = "Title";
        checkBtn.onClick.AddListener(() =>
        {

            Debug.Log($"내가 로비에 있는지 확인: {PhotonNetwork.InLobby}");
            Debug.Log($"내가 룸에 있는지 확인: {PhotonNetwork.InRoom}");
            Debug.Log($"데이터 캐시 갯수 : {LobbyManager.Instance.cachedRooms.Count}");

            foreach (RoomInfo room in LobbyManager.Instance.cachedRooms.Values)
            {
                Debug.Log($"{room.Name}");
            }

        });

        uiNicknameView.onClickSubmit = SetNickName;
    }

    public void CreateRoom()
    {
        createRoomBtn.gameObject.SetActive(false);
        Debug.Log("방을 만듭니다.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, IsVisible = true });
        PhotonNetwork.LoadLevel("Room");

    }

    public void SetNickName(string nickName)
    {
        PhotonNetwork.ConnectUsingSettings();

        myName = nickName;
        PhotonNetwork.NickName = nickName;

        LobbyManager.Instance.Connect();
    }

    public void SetLobby()
    {
        uiLoading.gameObject.SetActive(true);
        inputNickName.gameObject.SetActive(false);

        if (PhotonNetwork.InLobby == true)
        {
            uiLoading.gameObject.SetActive(false);
            lobbyText.text = "Lobby";
            LobbyRoomList.SetActive(true);
            NolobbyRoomText.gameObject.SetActive(true);
            createRoomBtn.gameObject.SetActive(true);
            Debug.Log("I'm in Lobby");

        }
    }

}
