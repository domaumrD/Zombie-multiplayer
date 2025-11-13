using UnityEngine;

public class UILoadingIcon : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
