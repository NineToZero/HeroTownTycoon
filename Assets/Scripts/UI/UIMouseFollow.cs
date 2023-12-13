using UnityEngine;

public class UIMouseFollow : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
