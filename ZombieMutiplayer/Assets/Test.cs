using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button btn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn.onClick.AddListener(() => { Debug.Log("Test"); PhotonNetwork.LeaveRoom(); PhotonNetwork.LoadLevel("MyLobby"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
