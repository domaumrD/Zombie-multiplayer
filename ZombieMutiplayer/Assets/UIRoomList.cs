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
            TMP_Text roomText = go.GetComponentInChildren<TMP_Text>();
            roomText.text = room.Name;
        }

    }
}
