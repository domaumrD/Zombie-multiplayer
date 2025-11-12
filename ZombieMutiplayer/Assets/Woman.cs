using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using Unity.VisualScripting;

public class Woman : MonoBehaviourPun
{
    float h;
    float v;

    float moveSpeed = 5f;
    public Animator animator;

    private void Update()
    {
        if (photonView.IsMine == false)
            return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);   

        if (dir != Vector3.zero)
        {           
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            animator.SetInteger("Status", 1);
        }
        else
        {
            animator.SetInteger("Status", 0);
        }
       

    }

}
