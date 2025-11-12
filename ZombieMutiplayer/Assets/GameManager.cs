using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class GameManager : MonoBehaviour
{
    public TMP_Text temp;

    void Start()
    {
        var initpos = Random.insideUnitSphere * 5f;
        initpos.y = 0;

        GameObject go = PhotonNetwork.Instantiate("Woman", initpos, Quaternion.identity);
        temp.text = PhotonNetwork.NickName;

    }   
}
