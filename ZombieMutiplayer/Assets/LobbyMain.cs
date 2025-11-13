using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    public UINicknameView nicknameView;
    public UILoading uiLoading;
    public UIRoomScrollview uiRoomScrollview;
    public Button createRoomButton;
    public Button leaveRoombutton;
    void Start()
    {
        uiRoomScrollview.Init();
        AddEventListeners();

        ConnectToMasterServer();

        createRoomButton.onClick.AddListener(() =>
        {
            Pun2Manager.instance.CreateRoom();
        });

        leaveRoombutton.onClick.AddListener(() =>
        {
            Pun2Manager.instance.LeaveRoom();
        });

        nicknameView.onClickSubmit = (nickname) =>
        {
            if (string.IsNullOrEmpty(nickname))
            {
                Debug.Log("nickname is empty");
            }
            else
            {
                Debug.Log($"nickname: {nickname}");
                Pun2Manager.instance.SetNickname(nickname);
                uiRoomScrollview.Show();
                createRoomButton.gameObject.SetActive(true);
                nicknameView.gameObject.SetActive(false);
            }
        };
    }

    private void ConnectToMasterServer()
    {
        uiLoading.Show();
        Pun2Manager.instance.Init();   //마스터 서버 접속
    }

    private void AddEventListeners()
    {
        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnConnectedToMaster, (eventType) =>
        {
            Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
            uiLoading.Hide();
            nicknameView.gameObject.SetActive(string.IsNullOrEmpty(PhotonNetwork.NickName));
        });

        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnJoinedLobby, (eventType) =>
        {
            Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
            uiLoading.Hide();

            if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                uiRoomScrollview.Show();
                createRoomButton.gameObject.SetActive(true);
                leaveRoombutton.gameObject.SetActive(false);
            }
        });


        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnJoinedRoom, (eventType) =>
        {
            Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
            leaveRoombutton.gameObject.SetActive(true);

            uiRoomScrollview.Hide();
            createRoomButton.gameObject.SetActive(false);
        });
    }
}