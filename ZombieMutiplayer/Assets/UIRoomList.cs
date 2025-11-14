using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomList : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform contentPointion;
       
    public void Create()
    {              
        foreach (RoomInfo room in LobbyManager.Instance.cachedRooms.Values)
        {
            GameObject go = Instantiate(cellPrefab, contentPointion);
            RoomCell roomCell = go.GetComponent<RoomCell>();
            string roomName = room.Name;
            roomCell.roomText.text = room.Name;
            roomCell.joinBtn.onClick.AddListener(() => 
            {
                Debug.Log("Joined");
                LobbyManager.Instance.ToJoinRoom(roomName);
            });
        }
    }

    public void Remove()
    {
        Debug.Log("¹æ Á¦°Å!");

        for(int i = 0; i < contentPointion.childCount; i++)
        {
            Debug.Log(contentPointion.GetChild(i).gameObject.name);
            //Destroy(contentPointion.GetChild(i).gameObject);
        }
    }


}
