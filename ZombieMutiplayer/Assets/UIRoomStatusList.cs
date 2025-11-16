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

        var temp = PhotonNetwork.CurrentRoom.Players;

        foreach (var player in temp.Values)
        {
            GameObject go = Instantiate(cellPrefab, contentPointion);
            RoomCell roomcell = go.GetComponent<RoomCell>();
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
