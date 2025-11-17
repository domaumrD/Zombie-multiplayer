using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UIRoomStatusList : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform contentPointion;
    public int readyCount = 0;

    public void Create()
    {
        Remove();

        var temp = PhotonNetwork.CurrentRoom.Players;

        foreach (var player in temp.Values)
        {
            Debug.Log($"<color=green> {player.NickName} </color>");
            GameObject go = Instantiate(cellPrefab, contentPointion);
            RoomCell roomcell = go.GetComponent<RoomCell>();
            roomcell.userName.text = player.NickName;
            roomcell.status.text = "wait";
        }
       
    }

    public void Create(string leavePlayer)
    {
        Remove();

        var temp = PhotonNetwork.CurrentRoom.Players;

        foreach (var player in temp.Values)
        {
            if (leavePlayer == player.NickName)
            {
                continue;
            }

            Debug.Log($"<color=green> {player.NickName} </color>");
            GameObject go = Instantiate(cellPrefab, contentPointion);
            RoomCell roomcell = go.GetComponent<RoomCell>();
            roomcell.userName.text = player.NickName;
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

    public void SetStatus(string playerName)
    {
        readyCount = 0;

        for (int i = 0; i < contentPointion.childCount; i++)
        {
            RoomCell roomcell = contentPointion.GetChild(i).gameObject.GetComponent<RoomCell>(); 

            if(roomcell.userName.text == playerName)
            {
                if(roomcell.status.text == "wait")
                {
                    roomcell.status.text = "ready";
                    readyCount++;
                }
                else
                {
                    roomcell.status.text = "wait";
                    readyCount--;
                }
            }

        }

        Debug.Log($"ready Count : {readyCount}");


    }

}
