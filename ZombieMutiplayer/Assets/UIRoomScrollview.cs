using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIRoomScrollview : MonoBehaviour
{
    public GameObject thereIsNoRoomGo;
    public Transform content;
    public GameObject uiRoomCellViewPrefab;

    public void Init()
    {
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>((int)EventEnums.EventType.OnRoomListUpdate, (type, data) =>
        {
            Debug.Log(data.Count);
            thereIsNoRoomGo.SetActive(data.Count == 0);
        });
    }

    public void UpdateUI()
    {

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}