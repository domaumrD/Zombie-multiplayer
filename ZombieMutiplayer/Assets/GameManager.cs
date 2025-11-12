using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviour
{    
    void Start()
    {
        var initpos = Random.insideUnitSphere * 5f;
        initpos.y = 0;

        GameObject go = PhotonNetwork.Instantiate("Woman", initpos, Quaternion.identity);
    }   
}
