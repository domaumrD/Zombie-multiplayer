using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using Unity.VisualScripting;

public class Woman : MonoBehaviourPun
{


    private void Update()
    {
        if (photonView.IsMine == false)
            return;
    }

}
