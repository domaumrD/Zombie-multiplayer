using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UIRoomStatusList : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform contentPointion;

    public void Create()
    {
        Remove();

        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            RoomCell roomcell = cellPrefab.GetComponent<RoomCell>();
            roomcell.userName.text = PhotonNetwork.NickName;
            roomcell.status.text = "wait";
        }
    }


    public void Remove()
    {
        Debug.Log("방인원 제거!");

        for (int i = 0; i < contentPointion.childCount; i++)
        {
            Destroy(contentPointion.GetChild(i).gameObject);
        }
    }
}
